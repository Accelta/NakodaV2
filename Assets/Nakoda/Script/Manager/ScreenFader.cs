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
            Debug.LogError("CanvasGroup not assigned to ScreenFader.");
            return;
        }

        // Start fully opaque
        fadeCanvasGroup.alpha = 1f;
    }

    void Start()
    {
        StartCoroutine(FadeCanvasGroup(1f, 0f)); // Fade from black to transparent
    }

    public void FadeAndRestart()
    {
        StartCoroutine(FadeOutAndReload());
    }

    private IEnumerator FadeCanvasGroup(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
    }

private IEnumerator FadeOutAndReload()
{
    yield return FadeCanvasGroup(0f, 1f); // Fade to black
    
    // Reset quest state before loading main menu
    ResetGameState();
    
    yield return new WaitForSeconds(delayBeforeRestart);
    SceneManager.LoadScene(0);
}

private void ResetGameState()
{
    // Reset QuestManager state
    if (QuestManager.Instance != null)
    {
        QuestManager.Instance.ResetQuestState();
    }
    
#if UNITY_EDITOR
    Debug.Log("Game state reset before returning to main menu");
#endif
}
}