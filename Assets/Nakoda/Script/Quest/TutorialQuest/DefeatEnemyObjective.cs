using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefeatEnemiesObjective", menuName = "Quests/Objective/Defeat Enemies")]
public class DefeatEnemiesObjective : QuestObjective
{
    public List<GameObject> enemies; // Assign via Inspector or runtime spawn
    private HashSet<GameObject> aliveEnemies = new();
    private EnemyTracker tracker;
    private CompassTarget currentTarget;
    

    public void RegisterTracker(EnemyTracker mgr)
    {
        tracker = mgr;
    }
    public override void StartObjective()
    {
#if UNITY_EDITOR
        Debug.Log("Objective Started: Defeat all enemies");
#endif

        // aliveEnemies.Clear();
        // foreach (var enemy in enemies)
        // {
        //     if (enemy != null)
        //         aliveEnemies.Add(enemy);
        // }
    }

public void OnEnemyDefeated(GameObject enemy)
    {
        if (tracker == null) return;

        tracker.RemoveEnemy(enemy);
        QuestUIController.Instance?.UpdateObjectiveProgress(GetProgress());

#if UNITY_EDITOR
        Debug.Log($"Enemy {enemy.name} defeated. Remaining: {tracker.RemainingCount()}");
#endif

        if (tracker.RemainingCount() == 0)
        {
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
    
}
