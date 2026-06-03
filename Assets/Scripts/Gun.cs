using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float range = 100f;

    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private int totalAmmo = 90;
    [SerializeField] private float reloadTime = 2f;

    [Header("Recoil & Spread")]
    [SerializeField] private float recoilAmount = 2f;
    [SerializeField] private float recoilRecovery = 5f;
    [SerializeField] private float spread = 0.02f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("UI")]
    [SerializeField] private Text ammoText;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;
    private float currentRecoil;

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    private void OnEnable()
    {
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (currentRecoil > 0)
        {
            currentRecoil -= recoilRecovery * Time.deltaTime;
            currentRecoil = Mathf.Max(0, currentRecoil);
        }

        if (isReloading)
            return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R) &&
            currentAmmo < maxAmmo &&
            totalAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
        {
            if (totalAmmo > 0)
                StartCoroutine(Reload());

            return;
        }

        currentAmmo--;
        UpdateAmmoUI();

        if (muzzleFlash != null)
            muzzleFlash.Play();

        currentRecoil += recoilAmount;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 direction =
            (ray.direction + Random.insideUnitSphere * spread).normalized;

        Debug.DrawRay( ray.origin, direction * range, Color.red,1f);

        if (Physics.Raycast(ray.origin, direction, out RaycastHit hit, range))
        {
            EnemyTarget enemy =
                hit.collider.GetComponent<EnemyTarget>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Debug.Log("Hit: " + hit.collider.name);
        }
    }

    private System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = maxAmmo - currentAmmo;
        int ammoToAdd = Mathf.Min(neededAmmo, totalAmmo);

        currentAmmo += ammoToAdd;
        totalAmmo -= ammoToAdd;

        isReloading = false;

        UpdateAmmoUI();
    }

    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {totalAmmo}";
        }
    }

    public float GetCurrentRecoil()
    {
        return currentRecoil;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetTotalAmmo()
    {
        return totalAmmo;
    }
}