using System.Collections;
using UnityEngine;

public class EnemyMovingAI : MonoBehaviour
{
    [Header("Target")]
    private GameObject player;  // Auto-find via tag (add [SerializeField] if manual assign needed)
    private Transform playerTransform; // to find player position
    private PlayerHealth playerHealth;
    private EnemyHealthController enemyHealth;

    [Header("Combat")]
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private float damageInterval = 1f; // Time between damage ticks
    private float damageTimer = 0f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackingRange = 1f;  // attack range
    [SerializeField] private float stopChasingRange = 0.1f; // minimum distance to stop before player

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealthController>();
    }

    private void Start()
    {
        // Auto-find player
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        GameObject audioManagerObj = GameObject.FindWithTag("AudioManager");
        if (audioManagerObj != null)
        {
            audioManager = audioManagerObj.GetComponent<AudioManager>();
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;

        // Stop all AI behavior if dead
        if (enemyHealth != null && enemyHealth.isDead)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 playerPos = playerTransform.position;
        Vector2 myPos = (Vector2)transform.position;
        Vector2 direction = (playerPos - myPos).normalized;
        float distanceToPlayer = Vector2.Distance(myPos, playerPos);
        Vector2 moveVelocity = direction * moveSpeed;

        // set animation for moving
        if (rb != null)
        {
            rb.linearVelocity = moveVelocity;
            animator.SetBool("isMoving", true);
        }

        if (moveVelocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }


        // handle attacking and animations
        if (distanceToPlayer > attackingRange) // not attack
        {
            damageTimer = 0f;
            animator.SetBool("isAttack", false);
        }
        else // attack
        {
            animator.SetBool("isAttack", true);
            if (rb != null)
            {
                if (distanceToPlayer <= stopChasingRange)
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }

            if (playerHealth != null && !playerHealth.isDead)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= damageInterval)
                {
                    TakeDamage(damagePerSecond);
                    damageTimer = 0f;
                }
            }
        }
    }

    void TakeDamage(int amount)
    {
        if (audioManager != null)
        {
            audioManager.PlayPlayerHitSound();
        }
        if (playerHealth != null)
        {
            playerHealth.addHealth(-amount);
        }
    }
}