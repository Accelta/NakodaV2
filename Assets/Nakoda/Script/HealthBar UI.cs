using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health targetHealth;
    public Image healthFillImage;
    public float smoothSpeed = 5f; // Controls the smoothness of the transition

    private float targetFillAmount;

    void Start()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged += UpdateHealthBar;

            // ✅ Force initial update manually
            UpdateHealthBar(targetHealth.GetCurrentHealth(), targetHealth.maxHealth);
        }
    }

    void UpdateHealthBar(float current, float max)
    {
        // Calculate the target fill amount
        targetFillAmount = Mathf.Clamp01(current / max);

        // Update the color based on the health percentage
        UpdateHealthBarColor(targetFillAmount);
    }

    void Update()
    {
        // Smoothly transition to the target fill amount
        healthFillImage.fillAmount = Mathf.Lerp(healthFillImage.fillAmount, targetFillAmount, smoothSpeed * Time.deltaTime);
    }

    void UpdateHealthBarColor(float fillAmount)
    {
        // Define color based on the health percentage
        if (fillAmount > 0.5f)
        {
            healthFillImage.color = Color.Lerp(Color.yellow, Color.green, fillAmount * 2f); // Green when above 50%
        }
        else
        {
            healthFillImage.color = Color.Lerp(Color.red, Color.yellow, fillAmount * 2f); // Yellow to Red when below 50%
        }
    }

    void OnDestroy()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
