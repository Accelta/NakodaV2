using UnityEngine;
using System.Collections.Generic;

public class EnemyTracker : MonoBehaviour
{
    public DefeatEnemiesObjective objective;
    private HashSet<GameObject> enemies = new HashSet<GameObject>();
     private int initialCount = 0;

    private void Awake()
    {
        if (objective != null)
        {
            objective.RegisterTracker(this);
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            
#if UNITY_EDITOR
            Debug.Log($"Registered enemy: {enemy.name}");
#endif
initialCount = enemies.Count;
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy == null || !enemies.Contains(enemy)) return;

        enemies.Remove(enemy);
    }

    public int RemainingCount()
    {
        enemies.RemoveWhere(e => e == null);
        return enemies.Count;
    }

    public int GetTotalCount()
    {
        return initialCount;
    }
public List<GameObject> GetAllAliveEnemies()
{
    // Clean up any null references first
    enemies.RemoveWhere(e => e == null);
    
    // Return all alive enemies
    var aliveList = new List<GameObject>();
    foreach (var enemy in enemies)
    {
        if (enemy != null && enemy.activeInHierarchy)
        {
            aliveList.Add(enemy);
        }
    }
    return aliveList;
}
}