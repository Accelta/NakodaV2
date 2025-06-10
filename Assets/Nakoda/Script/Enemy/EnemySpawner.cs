using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy & Spawn Settings")]
    public GameObject enemyPrefab;       // Enemy to spawn
    public Vector3 spawnAreaCenter;      // Center of spawn area (can be transform.position)
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10); // Size of spawn area on XZ plane

    [Header("Spawn Timing")]
    public bool autoSpawn = true;        // Spawn automatically every interval
    public float spawnInterval = 10f;    // Seconds between auto spawns

    void Start()
    {
        if (autoSpawn)
        {
            StartCoroutine(AutoSpawnRoutine());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy();
        }
    }

    IEnumerator AutoSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Enemy prefab is not assigned.");
            return;
#endif
        }

        Vector3 randomPosition = GetRandomPositionInArea();
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
#if UNITY_EDITOR
        Debug.Log($"Spawned enemy at {randomPosition}");
        #endif
    }

    Vector3 GetRandomPositionInArea()
    {
        float x = spawnAreaCenter.x + Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float y = spawnAreaCenter.y; // Usually ground level, adjust if needed
        float z = spawnAreaCenter.z + Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);

        return new Vector3(x, y, z);
    }
}
