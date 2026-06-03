using UnityEngine;
public class HealthPack : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HealthPack trigger: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("HealthPack trigger: " + other.name);
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}