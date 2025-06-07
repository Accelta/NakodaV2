using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIAutoFade : MonoBehaviour
{
    CanvasGroup canvasGroup;

    float idleTime = 0f;
    float activeTime = 0f;
    float fadeDuration = 1f;

    bool isFading = false;
    bool isVisible = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        bool hasInput = Input.anyKey || 
                       (Input.mousePresent && 
                       (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0));

        if (hasInput)
        {
            activeTime += Time.deltaTime;
            idleTime = 0f;

            // Fade out only after 10s of active input
            if (activeTime >= 10f && isVisible && !isFading)
            {
                StartCoroutine(FadeOut());
            }
        }
        else
        {
            idleTime += Time.deltaTime;
            activeTime = 0f;

            // Fade in after 10s of no input
            if (idleTime >= 5f && !isVisible && !isFading)
            {
                StartCoroutine(FadeIn());
            }
        }
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        isVisible = true;
        isFading = false;
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        isVisible = false;
        isFading = false;
    }
}
