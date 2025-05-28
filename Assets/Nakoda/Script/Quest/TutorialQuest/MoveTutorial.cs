using UnityEngine;

[CreateAssetMenu(fileName = "MoveToObjective", menuName = "Quests/Move Tutorial")]
public class MoveTutorial : QuestObjective
{
     public string triggerTag = "QuestTrigger";  // Tag for the trigger in the scene

    public override void StartObjective()
    {
        Debug.Log($"Objective Started: Move to the area marked by {triggerTag}");
        GameObject triggerobject = GameObject.FindWithTag(triggerTag);
        if (triggerobject != null)
        {
            var compasstarget = triggerobject.GetComponent<CompassTarget>();
            if (compasstarget != null)
            {
                CompassManager.Instance?.AddMarker(compasstarget);
            }
        }
        else
        {
            Debug.LogWarning("Trigger object not found for compass registration.");
        }
    }

    public override void CheckObjectiveCompletion()
    {
        // Completion is now handled by the trigger in the scene
    }

    public void OnPlayerEnterTrigger()
    {
        CompleteObjective();
    }
}