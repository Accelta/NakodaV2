using UnityEngine;

[CreateAssetMenu(fileName = "MoveQuestObjective", menuName = "Quests/Move Quest")]
public class MoveQuestObjective : QuestObjective
{
    [Header("Destination Settings")]
    public GameObject destinationPrefab; // Prefab to spawn
    public Vector3 spawnPosition;        // Where to spawn the objective marker
    public string playerTag = "Player";  // What tag should trigger it

    private GameObject spawnedInstance;
    private MoveQuestTrigger triggerScript;

    public override void StartObjective()
    {
        Debug.Log("MoveQuestObjective Started");

        if (destinationPrefab != null)
        {
            spawnedInstance = GameObject.Instantiate(destinationPrefab, spawnPosition, Quaternion.identity);
            triggerScript = spawnedInstance.GetComponent<MoveQuestTrigger>();

            if (triggerScript != null)
            {
                triggerScript.Initialize(this, playerTag);
            }
            else
            {
                Debug.LogError("MoveQuestTrigger script missing on destinationPrefab.");
            }
        }
        else
        {
            Debug.LogError("Destination Prefab is not assigned.");
        }
    }

    public override void CheckObjectiveCompletion()
    {
        // Not used directly; triggered externally
    }

    public void OnPlayerReachDestination()
    {
        CompleteObjective();

        if (spawnedInstance != null)
            spawnedInstance.SetActive(false);
    }

    public virtual void ResetObjective()
{
    isCompleted = false;
}
}
