using UnityEngine;

[CreateAssetMenu(fileName = "MoveToObjective", menuName = "Quests/Objective/Move Tutorial")]
public class MoveTutorial : QuestObjective
{
    public string triggerTag = "QuestTrigger";
    private CompassTarget currentTarget; // Reference to the marker added
     public bool isActive = false; // ✅ Status aktif

    public override void StartObjective()
    {
#if UNITY_EDITOR
        Debug.Log($"Objective Started: Move to the area marked by {triggerTag}");
#endif  
         isActive = true;
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
    public bool IsActive()
    {
        return isActive;
    }
    // public override void CheckObjectiveCompletion()
    // {
    //     // Handled externally
    // }

    public void OnPlayerEnterTrigger()
    {
         if (!isActive) return;
        CompleteObjective();

        if (currentTarget != null)
        {
            CompassManager.Instance?.RemoveMarker(currentTarget);

        }
         isActive = false;
    }
}
