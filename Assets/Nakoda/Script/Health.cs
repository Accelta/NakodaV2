using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged; // 👈 Add this line

    public bool isPlayer = false;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void InitializeHealth(float healthAmount)
    {
        maxHealth = healthAmount;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // 👈 Optional initial update
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f); // Clamp to 0

        Debug.Log($"{gameObject.name} hit!");
        Debug.Log($"Taking damage: {damageAmount}"); // 👈 Add this

        OnHealthChanged?.Invoke(currentHealth, maxHealth); // ✅ Fire the event here

        if (currentHealth <= 0f)
        {
            Die();
        }
        else if (currentHealth <= 0f && gameObject.CompareTag("Ship"))
        {
            PauseGame();
        }
    }

 void Die()
{
    Debug.Log($"{gameObject.name} died!");
    OnHealthChanged?.Invoke(0f, maxHealth);
    OnDeath?.Invoke();

    if (CompareTag("Player"))
    {
        ScreenFader fader = FindObjectOfType<ScreenFader>();
        if (fader != null)
        {
            fader.FadeAndRestart();
        }
    }
    else
    {
        Destroy(gameObject);
    }
}

    void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Over! The ship was hit.");
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
