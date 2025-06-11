// using UnityEngine;
// using System.Collections.Generic;

// [CreateAssetMenu(fileName = "DefeatEnemiesObjective", menuName = "Quests/Objective/Defeat Enemies")]
// public class DefeatEnemiesObjective : QuestObjective
// {
//     public List<GameObject> enemies; // Assign via Inspector or runtime spawn
//     private HashSet<GameObject> aliveEnemies = new();
//     private EnemyTracker tracker;
//     private CompassTarget currentTarget;
//     private EnemySpawner spawner;
    

//     public void RegisterTracker(EnemyTracker mgr)
//     {
//         tracker = mgr;
//     }
//     public override void StartObjective()
//     {
// #if UNITY_EDITOR
//         Debug.Log("Objective Started: Defeat all enemies");
// #endif

//         // aliveEnemies.Clear();
//         // foreach (var enemy in enemies)
//         // {
//         //     if (enemy != null)
//         //         aliveEnemies.Add(enemy);
//         // }
//         spawner.SpawnEnemies();
//     }

// public void OnEnemyDefeated(GameObject enemy)
//     {
//         if (tracker == null) return;

//         tracker.RemoveEnemy(enemy);
//         QuestUIController.Instance?.UpdateObjectiveProgress(GetProgress());

// #if UNITY_EDITOR
//         Debug.Log($"Enemy {enemy.name} defeated. Remaining: {tracker.RemainingCount()}");
// #endif

//         if (tracker.RemainingCount() == 0)
//         {
//             CompleteObjective();
//         }
//     }

// public override string GetProgress()
// {
//     if (tracker == null) return "";
//     int remaining = tracker.RemainingCount();
//     int total = tracker.GetTotalCount();
//     int defeated = total - remaining;
//     return $"{defeated}/{total}";
// }
//     public void RegisterSpawner(EnemySpawner spawnerRef)
// {
//     spawner = spawnerRef;
// #if UNITY_EDITOR
//     Debug.Log("EnemySpawner registered successfully.");
// #endif
// }
// }
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "DefeatEnemiesObjective", menuName = "Quests/Objective/Defeat Enemies")]
public class DefeatEnemiesObjective : QuestObjective
{
    public List<GameObject> enemies;
    private HashSet<GameObject> aliveEnemies = new();
    private EnemyTracker tracker;
    private List<CompassTarget> enemyTargets = new List<CompassTarget>(); // Store all enemy markers
    private EnemySpawner spawner;

    public void RegisterTracker(EnemyTracker mgr)
    {
        tracker = mgr;
    }

    public override void StartObjective()
    {
#if UNITY_EDITOR
        Debug.Log("Objective Started: Defeat all enemies");
#endif

        if (spawner != null)
        {
            spawner.SpawnEnemies();
            
            // Wait for enemies to spawn and register, then add compass markers to all
            QuestMonoHelper.Instance.StartCoroutine(WaitAndRegisterAllMarkers());
        }
    }

    private IEnumerator WaitAndRegisterAllMarkers()
    {
        yield return new WaitForSeconds(0.1f); // Wait for enemies to spawn
        RegisterAllEnemyMarkers();
    }

    private void RegisterAllEnemyMarkers()
    {
        if (tracker == null) return;

        // Clear existing markers
        enemyTargets.Clear();

        // Get all alive enemies and add compass markers to each
        var allEnemies = tracker.GetAllAliveEnemies();
        foreach (var enemy in allEnemies)
        {
            if (enemy != null)
            {
                var compassTarget = enemy.GetComponent<CompassTarget>();
                if (compassTarget != null)
                {
                    CompassManager.Instance?.AddMarker(compassTarget);
                    enemyTargets.Add(compassTarget);
#if UNITY_EDITOR
                    Debug.Log($"Enemy compass marker registered for {enemy.name}");
#endif
                }
            }
        }
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        if (tracker == null) return;

        tracker.RemoveEnemy(enemy);
        QuestUIController.Instance?.UpdateObjectiveProgress(GetProgress());

#if UNITY_EDITOR
        Debug.Log($"Enemy {enemy.name} defeated. Remaining: {tracker.RemainingCount()}");
#endif

        // Remove compass marker for this specific enemy
        var compassTarget = enemy.GetComponent<CompassTarget>();
        if (compassTarget != null && enemyTargets.Contains(compassTarget))
        {
            CompassManager.Instance?.RemoveMarker(compassTarget);
            enemyTargets.Remove(compassTarget);
#if UNITY_EDITOR
            Debug.Log($"Compass marker removed for defeated enemy {enemy.name}");
#endif
        }

        if (tracker.RemainingCount() == 0)
        {
        foreach (var target in enemyTargets)
        {
            if (target != null)
            {
                CompassManager.Instance?.RemoveMarker(target);
            }
        }
        enemyTargets.Clear();
            CompleteObjective();

        }
    }

    public override string GetProgress()
    {
        if (tracker == null) return "";
        int remaining = tracker.RemainingCount();
        int total = tracker.GetTotalCount();
        int defeated = total - remaining;
        return $"{defeated}/{total}";
    }

    public void RegisterSpawner(EnemySpawner spawnerRef)
    {
        spawner = spawnerRef;
#if UNITY_EDITOR
        Debug.Log("EnemySpawner registered successfully.");
#endif
    }
}