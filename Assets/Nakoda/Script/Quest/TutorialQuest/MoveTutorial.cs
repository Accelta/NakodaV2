using UnityEngine;

[CreateAssetMenu(fileName = "MoveToObjective", menuName = "Quests/Move Tutorial")]
public class MoveTutorial : QuestObjective
{
     public string triggerTag = "QuestTrigger";  // Tag for the trigger in the scene

    public override void StartObjective()
    {
        Debug.Log($"Objective Started: Move to the area marked by {triggerTag}");
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