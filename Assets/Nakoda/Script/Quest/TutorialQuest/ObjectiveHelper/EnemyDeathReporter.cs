using UnityEngine;

public class EnemyDeathReporter : MonoBehaviour
{
    public DefeatEnemiesObjective defeatObjective;

    private void Start()
    {
        if (defeatObjective != null)
        {
            var tracker = FindFirstObjectByType<EnemyTracker>();
            if (tracker != null)
                tracker.RegisterEnemy(gameObject);
        }

        var health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    private void HandleDeath()
    {
        if (defeatObjective != null)
        {
            defeatObjective.OnEnemyDefeated(gameObject);
#if UNITY_EDITOR
            Debug.Log($"{gameObject.name} reported death to objective.");
#endif
        }
    }

    private void OnDestroy()
    {
        var health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }
}