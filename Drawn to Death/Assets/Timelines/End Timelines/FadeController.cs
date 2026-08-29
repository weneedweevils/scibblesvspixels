using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // Reference to the UI Image component for fading
    [SerializeField] private Color[] fadeColors; // Array of colors for fading

    public UnityEvent onFadeComplete = new UnityEvent();

    /// <summary>
    /// Set the color of the fade using a color from the fadeColors list
    /// </summary>
    public void SetFadeColor(int id)
    {
        if (id >= 0 && id < fadeColors.Length)
        {
            SetFadeColor(fadeColors[id]);
        }
        else
        {
            Debug.LogWarning("Invalid fade color ID: " + id);
        }
    }

    /// <summary>
    /// Set the color of the fade
    /// </summary>
    public void SetFadeColor(Color color)
    {
        if (fadeImage != null)
        {
            fadeImage.color = color;
        }
    }

    /// <summary>
    /// Fade alpha from 0 -> 1
    /// </summary>
    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(0f, 1f, duration));
    }

    /// <summary>
    /// Fade alpha from 1 -> 0
    /// </summary>
    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(1f, 0f, duration));
    }

    /// <summary>
    /// Fade sequence
    /// </summary>
    public IEnumerator Fade(float startAlpha, float endAlpha, float duration, float delay = 0f)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            MyUtils.SetAlpha(fadeImage, alpha);
            yield return null;
        }
        MyUtils.SetAlpha(fadeImage, endAlpha);
        onFadeComplete?.Invoke();
    }
}
