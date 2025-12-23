using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarController : MonoBehaviour
{
    private EnemyHealthController enemyHealth;
    private Slider healthSlider;
    void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealthController>();
        healthSlider = GetComponent<Slider>();
    }

    void UpdateHealthBar()
    {
        if (enemyHealth != null && healthSlider != null)
        {
            healthSlider.value = enemyHealth.currentHealth / enemyHealth.maxHealth;
        }
        if (enemyHealth != null && enemyHealth.isDead)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        UpdateHealthBar();
    }
}
