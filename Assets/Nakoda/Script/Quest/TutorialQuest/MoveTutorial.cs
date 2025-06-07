using UnityEngine;

[CreateAssetMenu(fileName = "MoveToObjective", menuName = "Quests/Objective/Move Tutorial")]
public class MoveTutorial : QuestObjective
{
    public string triggerTag = "QuestTrigger";
    private CompassTarget currentTarget; // Reference to the marker added

    public override void StartObjective()
    {
        #if UNITY_EDITOR
        Debug.Log($"Objective Started: Move to the area marked by {triggerTag}");
        #endif

        GameObject triggerObject = GameObject.FindWithTag(triggerTag);
        if (triggerObject != null)
        {
            currentTarget = triggerObject.GetComponent<CompassTarget>();
            if (currentTarget != null)
            {
                CompassManager.Instance?.AddMarker(currentTarget);
            }
        }
        else
        {
            #if UNITY_EDITOR
            Debug.LogWarning("Trigger object not found for compass registration.");
            #endif
        }
    }

    // public override void CheckObjectiveCompletion()
    // {
    //     // Handled externally
    // }

    public void OnPlayerEnterTrigger()
    {
        CompleteObjective();

        if (currentTarget != null)
        {
            CompassManager.Instance?.RemoveMarker(currentTarget);
        }
        
    }
}
