using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashEffect : MonoBehaviour
{
    public static FlashEffect Instance { get; private set; }

    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration = 0.3f;
    [SerializeField] private Color flashColor = new Color(1f, 1f, 1f, 0.5f); // White semi-transparent

    private void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Make sure the flash starts invisible
        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    public void Flash(Color color, float duration)
    {
        StartCoroutine(FlashCoroutine(color, duration));
    }

    private IEnumerator FlashCoroutine()
    {
        return FlashCoroutine(flashColor, flashDuration);
    }

    private IEnumerator FlashCoroutine(Color color, float duration)
    {
        if (flashImage == null) yield break;

        // Fade in quickly
        float elapsed = 0f;
        float fadeInTime = duration * 0.3f;
        
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, color.a, elapsed / fadeInTime);
            flashImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Fade out
        elapsed = 0f;
        float fadeOutTime = duration * 0.7f;
        
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(color.a, 0f, elapsed / fadeOutTime);
            flashImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent
        flashImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}
