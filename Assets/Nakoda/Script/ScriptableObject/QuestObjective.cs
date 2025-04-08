using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    public string objectiveDescription;
    public bool isCompleted { get; private set; } = false;

    public abstract void StartObjective();
    public abstract void CheckObjectiveCompletion();

    protected void CompleteObjective()
    {
        isCompleted = true;
        Debug.Log($"Objective Completed: {objectiveDescription}");
    }
}
