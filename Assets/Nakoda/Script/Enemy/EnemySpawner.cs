// using UnityEngine;
// using System.Collections;

// public class EnemySpawner : MonoBehaviour
// {
//     [Header("Enemy & Spawn Settings")]
//     public GameObject enemyPrefab;       // Enemy to spawn
//     public Vector3 spawnAreaCenter;      // Center of spawn area (can be transform.position)
//     public Vector3 spawnAreaSize = new Vector3(10, 0, 10); // Size of spawn area on XZ plane

//     [Header("Spawn Timing")]
//     public bool autoSpawn = true;        // Spawn automatically every interval
//     public float spawnInterval = 10f;    // Seconds between auto spawns

//     void Start()
//     {
//         if (autoSpawn)
//         {
//             StartCoroutine(AutoSpawnRoutine());
//         }
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             SpawnEnemy();
//         }
//     }

//     IEnumerator AutoSpawnRoutine()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(spawnInterval);
//             SpawnEnemy();
//         }
//     }

//     void SpawnEnemy()
//     {
//         if (enemyPrefab == null)
//         {
// #if UNITY_EDITOR
//             Debug.LogWarning("Enemy prefab is not assigned.");
//             return;
// #endif
//         }

//         Vector3 randomPosition = GetRandomPositionInArea();
//         Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
// #if UNITY_EDITOR
//         Debug.Log($"Spawned enemy at {randomPosition}");
//         #endif
//     }

//     Vector3 GetRandomPositionInArea()
//     {
//         float x = spawnAreaCenter.x + Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
//         float y = spawnAreaCenter.y; // Usually ground level, adjust if needed
//         float z = spawnAreaCenter.z + Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);

//         return new Vector3(x, y, z);
//     }
// }
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public DefeatEnemiesObjective defeatObjective;
    public GameObject enemyPrefab;
    public int enemyCount = 3;
    public Vector3 spawnAreaOffset = Vector3.zero; // Offset from transform position
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);
    public bool showSpawnArea = true;
    public Color gizmoColor = new Color(0, 1, 0, 0.25f);

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // Call this from your quest StartObjective or QuestManager when quest becomes active
    private Vector3 GetSpawnCenter()
    {
        return transform.position + spawnAreaOffset;
    }

    private void Awake()
    {
        // Register this spawner with the objective
        if (defeatObjective != null)
        {
            defeatObjective.RegisterSpawner(this);
        }
    }
public void SpawnEnemies()
{
    ClearEnemies();

    Vector3 spawnCenter = GetSpawnCenter();

    for (int i = 0; i < enemyCount; i++)
    {
        Vector3 randomPos = new Vector3(
            spawnCenter.x + Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnCenter.y,
            spawnCenter.z + Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        
        // Set the objective BEFORE activating the enemy
        var reporter = enemy.GetComponent<EnemyDeathReporter>();
        if (reporter != null)
        {
            reporter.defeatObjective = defeatObjective;
        }

        enemy.SetActive(true); // Activate after setting objective

        spawnedEnemies.Add(enemy);

#if UNITY_EDITOR
        Debug.Log($"Spawned enemy {i+1} at position: {randomPos}");
#endif
    }

#if UNITY_EDITOR
    Debug.Log($"Spawned {enemyCount} enemies total.");
#endif
}

    // Call this when quest is not active or on cleanup
    public void HideEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
                enemy.SetActive(false);
        }
    }

    // Optional: Remove all spawned enemies
    public void ClearEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }
    private void OnDrawGizmosSelected()
    {
        if (!showSpawnArea) return;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(GetSpawnCenter(), spawnAreaSize); // Use dynamic center
    }
}