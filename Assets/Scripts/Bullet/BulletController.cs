using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    public float range = 8f; // max range before auto-destroy
    private Vector2 startPosition;
    private AudioManager audioManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject audioManagerObj = GameObject.FindWithTag("AudioManager");
        if (audioManagerObj != null)
        {
            audioManager = audioManagerObj.GetComponent<AudioManager>();
        }
    }


    void Update()
    {
        float distanceTraveled = Vector2.Distance(transform.position, startPosition);

        if (distanceTraveled > range)
        {
            Destroy(gameObject);
        }
    }


    public void Shoot(Vector2 direction, float speed)
    {
        startPosition = transform.position;
        rb.linearVelocity = direction.normalized * speed;

        // Rotate bullet to face the direction (assuming sprite faces right by default)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("Bullet shot in direction: " + direction + " with speed: " + speed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (audioManager != null)
        {
            audioManager.PlayBulletHitSound();
        }
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        rb.linearVelocity = Vector2.zero; // stop movement

        // run hit animation
        if (animator != null)
        {
            Debug.Log("Triggering hit animation");
            animator.SetTrigger("Hit");
        }

        // Deal damage
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthController enemyHealth = collision.gameObject.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20);
                Debug.Log("Dealt 20 damage to " + collision.gameObject.name);
            }
        }

        // Destroy LAST (or delay for full anim)
        Destroy(gameObject, 1f);
    }
}
