using UnityEngine;
using System;

public class EnemyDetection : MonoBehaviour
{
    public float detectionRange = 50f;
    public float engageRange = 10f;

    [Tooltip("Assign the Player Prefab here, used to find the player instance at runtime.")]
    public GameObject playerPrefab;

    private Transform playerTransform;
    public event Action OnPlayerDetected;

    void Start()
    {
        // Find the player instance in the scene by prefab name or tag
        // Assuming the player GameObject has a unique tag "Player"
        GameObject playerInstance = GameObject.FindGameObjectWithTag("Player");

        if (playerInstance != null)
        {
            playerTransform = playerInstance.transform;
        }
        else
        {
            Debug.LogWarning("Player instance not found in the scene.");
        }
    }

    public bool IsPlayerInRange()
    {
        if (playerTransform == null) return false;
        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }

    public bool IsPlayerInEngageRange()
    {
        if (playerTransform == null) return false;
        return Vector3.Distance(transform.position, playerTransform.position) <= engageRange;
    }

    void Update()
    {
        if (IsPlayerInRange())
            OnPlayerDetected?.Invoke();
    }
}
