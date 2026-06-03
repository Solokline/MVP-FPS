using UnityEngine;
public class AmmoPack : MonoBehaviour
{
    [SerializeField] private int ammoAmount = 30;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HealthPack trigger: " + other.name);
            // Находим активное оружие (или можно пополнить все)
            // В этом примере пополняем только текущее активное оружие
            Gun activeGun = other.GetComponentInChildren<Gun>(false); // false = только активные
            if (activeGun != null)
            {
                activeGun.AddAmmo(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}