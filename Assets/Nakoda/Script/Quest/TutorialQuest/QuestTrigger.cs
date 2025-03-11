using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public MoveTutorial linkedObjective;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !linkedObjective.isCompleted)
        {
            linkedObjective.CheckObjectiveCompletion();
        }
    }
}
