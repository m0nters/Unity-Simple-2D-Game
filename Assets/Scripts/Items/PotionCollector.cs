using UnityEngine;

public class PotionCollector : MonoBehaviour
{
    public AudioManager audioManager;
    void Start()
    {
        GameObject audioManagerObj = GameObject.FindWithTag("AudioManager");
        if (audioManagerObj != null)
        {
            audioManager = audioManagerObj.GetComponent<AudioManager>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (audioManager != null)
        {
            audioManager.PlayHealthPickupSound();
        }

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.addHealth(20); // Heal the player by 20 health points
            Destroy(gameObject); // Destroy the potion after collection
        }
    }
}
