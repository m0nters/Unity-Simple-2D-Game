using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 10;
    PlayerHealth playerHealth;
    bool playerInZone = false;
    float damageInterval = 1.0f; // seconds
    float damageTimer = 0.0f;


    void OnTriggerEnter2D(Collider2D collision) // when enter, deal damage once
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            TakeDamage(); // deal damage right away
            playerInZone = true;
        }
    }



    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited DamageZone.");
            playerHealth = null;
            playerInZone = false;
            damageTimer = 0.0f;
        }
    }

    void Update()
    {
        if (playerInZone)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                TakeDamage();
                damageTimer = 0.0f;
            }
        }
    }

    void TakeDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.addHealth(-damageAmount);
        }
    }
}
