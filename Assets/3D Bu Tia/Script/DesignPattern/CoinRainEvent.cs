using UnityEngine;

public class CoinRainEvent : MonoBehaviour
{
    public GameObject fallingObjectPrefab;
    public float spawnAreaWidth = 10f;
    public float spawnHeight = 10f;
    public int objectsToSpawn = 20;
    public float spawnInterval = 0.1f;

    private void OnEnable()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnCoinRain += TriggerCoinRain;
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnCoinRain -= TriggerCoinRain;
    }

    public void TriggerCoinRain()
    {
        StartCoroutine(SpawnFallingObjects());
    }

    private System.Collections.IEnumerator SpawnFallingObjects()
    {
        for (int i = 0; i < objectsToSpawn; i++)
        {
            float x = transform.position.x + Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
            Vector3 spawnPosition = new Vector3(x, transform.position.y + spawnHeight, transform.position.z);
            Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}