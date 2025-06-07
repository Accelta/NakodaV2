using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefeatEnemiesObjective", menuName = "Quests/Objective/Defeat Enemies")]
public class DefeatEnemiesObjective : QuestObjective
{
    public List<GameObject> enemies; // Assign via Inspector or runtime spawn
    private HashSet<GameObject> aliveEnemies = new();
    private CompassTarget currentTarget;

    public override void StartObjective()
    {
#if UNITY_EDITOR
        Debug.Log("Objective Started: Defeat all enemies");
#endif
        aliveEnemies.Clear();
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                aliveEnemies.Add(enemy);
        }
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Remove(enemy);
#if UNITY_EDITOR
            Debug.Log($"Enemy {enemy.name} defeated. Remaining: {aliveEnemies.Count}");
#endif
            if (aliveEnemies.Count == 0)
            {
                CompleteObjective();
            }
        }
    }

    // public override void CheckObjectiveCompletion()
    // {
    //     // TargetHit is called externally
    // }
    
    
}
