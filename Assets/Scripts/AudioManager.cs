using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXAudioSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusicClip;
    public AudioClip shootClip;
    public AudioClip bulletHitClip;
    public AudioClip enemyDeadClip;
    public AudioClip healthPickupClip;
    public AudioClip playerHitClip;

    void Start()
    {
        // Play background music on start
        if (musicSource != null && backgroundMusicClip != null)
        {
            musicSource.clip = backgroundMusicClip;
            musicSource.loop = true;
            musicSource.Play();
            musicSource.volume = 0.1f;
        }
    }

    public void PlayShootSound()
    {
        if (SFXAudioSource != null && shootClip != null)
        {
            SFXAudioSource.PlayOneShot(shootClip);
            SFXAudioSource.volume = 0.3f;
        }
    }

    public void PlayBulletHitSound()
    {
        if (SFXAudioSource != null && bulletHitClip != null)
        {
            SFXAudioSource.PlayOneShot(bulletHitClip);
        }
    }

    public void PlayEnemyDeadSound()
    {
        if (SFXAudioSource != null && enemyDeadClip != null)
        {
            SFXAudioSource.PlayOneShot(enemyDeadClip);
        }
    }

    public void PlayHealthPickupSound()
    {
        if (SFXAudioSource != null && healthPickupClip != null)
        {
            SFXAudioSource.PlayOneShot(healthPickupClip);
        }
    }

    public void PlayPlayerHitSound()
    {
        if (SFXAudioSource != null && playerHitClip != null)
        {
            SFXAudioSource.PlayOneShot(playerHitClip);
        }
    }

}
