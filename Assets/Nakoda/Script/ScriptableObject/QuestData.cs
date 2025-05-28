using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questName;
    public List<QuestObjective> objectives;
    public QuestData requiredQuest;
    public string questDescription;

    private int currentObjectiveIndex = 0;

    public bool IsQuestCompleted()
    {
        return objectives.TrueForAll(obj => obj != null && obj.isCompleted);
    }

    public bool CanStartQuest()
    {
        return requiredQuest == null || requiredQuest.IsQuestCompleted();
    }

    public void StartQuest()
    {
        Debug.Log($"Quest Started: {questName}");
        currentObjectiveIndex = 0;

        if (objectives.Count > 0 && objectives[0] != null)
        {
            objectives[0].StartObjective();
        }
    }

    public void CheckCurrentObjective()
    {
        if (currentObjectiveIndex >= objectives.Count) return;

        var current = objectives[currentObjectiveIndex];
        if (current == null || current.isCompleted) return;

        current.CheckObjectiveCompletion();

        if (current.isCompleted)
        {
            currentObjectiveIndex++;

            if (currentObjectiveIndex < objectives.Count)
            {
                objectives[currentObjectiveIndex].StartObjective();
            }
            else
            {
                Debug.Log($"Quest Completed: {questName}");
            }
        }
    }
}
