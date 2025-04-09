using UnityEngine;

public class SpawnerTst : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 1.5f;
    public float xRange = 6f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 1f, spawnInterval);
    }

    void SpawnObject()
    {
        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-xRange, xRange), 0, 0);
        Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
    }
}
