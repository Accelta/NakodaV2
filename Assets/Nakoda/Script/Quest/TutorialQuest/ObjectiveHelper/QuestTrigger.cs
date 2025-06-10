using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public MoveTutorial linkedObjective;

    private void OnTriggerEnter(Collider other )
    {
        if (other.CompareTag("Player") && linkedObjective != null && linkedObjective.IsActive())
        {
#if UNITY_EDITOR
            Debug.Log("Player entered quest area!");
#endif
            linkedObjective.OnPlayerEnterTrigger();
           this.gameObject.SetActive(false); // Disable the trigger after use
        }
    }
}
