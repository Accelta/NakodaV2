using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [Header("Fade Panel Reference")]
    public CanvasGroup fadeCanvasGroup;

    [Header("Settings")]
    public float fadeDuration = 1f;
    public float delayBeforeRestart = 1.5f;

    void Awake()
    {
        if (fadeCanvasGroup == null)
        {
        #if UNITY_EDITOR
            Debug.LogError("CanvasGroup not assigned!");
        #endif
            return;
        }

        fadeCanvasGroup.alpha = 1f; // Start fully black
    }

    void Start()
    {
        StartCoroutine(DelayedFadeOut());
    }

    IEnumerator DelayedFadeOut()
    {
        yield return null; // Let frame render fully black first
        yield return FadeOut();
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            fadeCanvasGroup.alpha = 1f - Mathf.SmoothStep(0f, 1f, t);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f; // Final clear
    }

    public void FadeAndRestart()
    {
        StartCoroutine(FadeInAndRestart());
    }

    IEnumerator FadeInAndRestart()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            fadeCanvasGroup.alpha = Mathf.SmoothStep(0f, 1f, t);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(delayBeforeRestart);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
