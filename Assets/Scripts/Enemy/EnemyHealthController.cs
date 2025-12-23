using TMPro;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public bool isDead = false;
    private Rigidbody2D rb;
    private Animator animator;
    private TextMeshProUGUI scoreText;
    private AudioManager audioManager;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameObject scoreTextObj = GameObject.FindWithTag("ScoreText");
        if (scoreTextObj != null)
        {
            scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();
        }
        GameObject audioManagerObj = GameObject.FindWithTag("AudioManager");
        if (audioManagerObj != null)
        {
            audioManager = audioManagerObj.GetComponent<AudioManager>();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;
            Die();
            Debug.Log("Enemy Dead");
        }
    }

    public void Die()
    {
        if (audioManager != null)
        {
            audioManager.PlayEnemyDeadSound();
        }
        isDead = true;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; // Stop all physics interactions
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false; // Disable collider to prevent further interactions
            }
        }
        if (scoreText != null)
        {
            int currentScore = int.Parse(scoreText.text);
            currentScore += 1; // Increase score by 10 for each enemy killed
            scoreText.text = currentScore.ToString();
        }
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttack", false);
            animator.SetTrigger("Dead");
            Destroy(gameObject, 1f);
        }
    }
}
