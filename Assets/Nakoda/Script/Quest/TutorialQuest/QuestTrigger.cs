using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public MoveTutorial linkedObjective;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered quest area!");
            linkedObjective.OnPlayerEnterTrigger();
        }
    }
}
