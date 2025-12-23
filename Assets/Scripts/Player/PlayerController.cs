using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 10.0f;
    public Vector2 movement;
    public Vector2 lookDirection = new Vector2(0, -1); // player facing downward initially
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject bulletPrefab;
    private AudioManager audioManager;

    public float bulletSpeed = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();

        GameObject audioManagerObj = GameObject.FindWithTag("AudioManager");
        if (audioManagerObj != null)
        {
            audioManager = audioManagerObj.GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // set isWalking
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);
            lookDirection = movement.normalized;
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Shoot Bullet");
            LaunchBullet(lookDirection);
            if (audioManager != null)
            {
                audioManager.PlayShootSound();
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    void LaunchBullet(Vector2 direction)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletController bulletScript = bullet.GetComponent<BulletController>();
            if (bulletScript != null)
            {
                bulletScript.Shoot(direction, bulletSpeed);
            }
        }
    }
}
