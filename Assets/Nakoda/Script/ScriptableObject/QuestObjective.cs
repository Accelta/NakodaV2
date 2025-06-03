using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    public string objectiveDescription;
    public bool isCompleted = false;

    public abstract void StartObjective();
    public abstract void CheckObjectiveCompletion();

    protected void CompleteObjective()
    {
        isCompleted = true;
        Debug.Log($"Objective Completed: {objectiveDescription}");
        QuestManager.Instance?.UpdateObjectives();
    }

    public void ResetObjective()
    {
        isCompleted = false;
    }
}
