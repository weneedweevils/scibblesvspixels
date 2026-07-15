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

    public void SetFadeColor(Color color)
    {
        if (fadeImage != null)
        {
            fadeImage.color = color;
        }
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(0f, 1f, duration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(1f, 0f, duration));
    }

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
