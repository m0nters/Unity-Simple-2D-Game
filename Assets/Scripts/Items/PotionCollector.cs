using UnityEngine;

public class PotionCollector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }


        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.addHealth(20); // Heal the player by 20 health points
            Destroy(gameObject); // Destroy the potion after collection
        }
    }
}
