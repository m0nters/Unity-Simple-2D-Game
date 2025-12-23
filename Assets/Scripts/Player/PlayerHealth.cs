using TMPro;
using UnityEngine;
using UnityEngine.UI; // Add this for legacy Text, or 'using TMPro;' for TextMeshProUGUI

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI(); // Update the UI on start
    }
    public void addHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        isDead = currentHealth == 0;
        UpdateHealthUI(); // Update the UI whenever health changes
        Debug.Log($"Player Health: {currentHealth}/{maxHealth} (Dead: {isDead})");
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}