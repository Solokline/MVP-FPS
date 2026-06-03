using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Здоровье игрока, получение урона, лечение, смерть.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Text healthText;
    [SerializeField] private GameObject deathScreen; // Опционально: экран смерти
    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthUI();
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }
    private void Die()
    {
        Debug.Log("Player died");
        if (deathScreen != null)
            deathScreen.SetActive(true);
        // Отключаем управление
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        enabled = false; // Отключаем скрипт FPSController
                         // Можно добавить перезагрузку сцены или меню
    }
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}";
        }
    }
}