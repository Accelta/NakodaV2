using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MoveQuestTrigger : MonoBehaviour
{
    private MoveQuestObjective questObjective;
    private string triggerTag;

    public void Initialize(MoveQuestObjective objective, string tagToCheck)
    {
        questObjective = objective;
        triggerTag = tagToCheck;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag) && questObjective != null)
        {
            questObjective.OnPlayerReachDestination();
        }
    }
}
