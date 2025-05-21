using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    public bool isPlayer = false;

    [SerializeField] private float destructionDelay = 10f;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private List<GameObject> toBeDestroyedList = new List<GameObject>();

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void InitializeHealth(float healthAmount)
    {
        maxHealth = healthAmount;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log($"{gameObject.name} hit!");
        Debug.Log($"Taking damage: {damageAmount}");

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();

           
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        OnHealthChanged?.Invoke(0f, maxHealth);
        OnDeath?.Invoke();
        
        // Disable cannon if exists
            CannonRotation cannon = GetComponent<CannonRotation>();
                if (cannon != null)
                {
                    cannon.enabled = false;
                    Debug.Log($"{gameObject.name}'s CannonRotation disabled on death.");
                }

        if (CompareTag("Player"))
        {
            ScreenFader fader = FindFirstObjectByType<ScreenFader>();
            if (fader != null)
            {
                fader.FadeAndRestart();
            }
        }
        else
        {


            if (!toBeDestroyedList.Contains(gameObject))
                toBeDestroyedList.Add(gameObject);

            BoatBuoyancy buoyancy = GetComponent<BoatBuoyancy>();
            if (buoyancy != null)
            {
                buoyancy.buoyancyStrength = 0.5f;
                buoyancy.dragInWater = 5f;

                int total = buoyancy.floatPoints.Length;
                if (total >= 2)
                {
                    buoyancy.floatPoints[total - 1].gameObject.SetActive(false);
                    buoyancy.floatPoints[total - 2].gameObject.SetActive(false);
                }
            }

            StartCoroutine(DelayedDestruction());
        }
    }

    IEnumerator DelayedDestruction()
    {
        yield return new WaitForSeconds(destructionDelay);

        foreach (var obj in toBeDestroyedList)
        {
            if (obj != null)
            {
                if (explosionVFX != null)
                    Instantiate(explosionVFX, obj.transform.position, Quaternion.identity);

                Destroy(obj);
            }
        }

        toBeDestroyedList.Clear();
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
