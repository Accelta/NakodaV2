using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public event Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        // PlayerPrefs.SetFloat("PlayerHealth", currentHealth);
        // PlayerPrefs.Save();
        // float savedHealth = PlayerPrefs.GetFloat("PlayerHealth");
    }

    public void InitializeHealth(float healthAmount)
    {
        maxHealth = healthAmount;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {

        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} hit!");

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
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        Debug.Log("Game Over! The ship was hit.");
    }
}
