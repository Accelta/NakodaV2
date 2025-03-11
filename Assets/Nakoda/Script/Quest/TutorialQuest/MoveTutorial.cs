using UnityEngine;

[CreateAssetMenu(fileName = "MoveToObjective", menuName = "Quests/Move Tutorial")]
public class MoveTutorial : QuestObjective
{
    public Vector3 targetPosition;
    private Transform playerTransform;
    public float completionRadius = 3f; // Completion threshold

    public override void StartObjective()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Ensure the player has the 'Player' tag.");
        }
    }

    public override void CheckObjectiveCompletion()
    {
        if (playerTransform != null && Vector3.Distance(playerTransform.position, targetPosition) < completionRadius)
        {
            CompleteObjective();
        }
    }
}