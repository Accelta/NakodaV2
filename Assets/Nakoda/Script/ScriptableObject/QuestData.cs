using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Data/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questName;
    public List<QuestObjective> objectives;
    public QuestData requiredQuest; // The quest that must be completed before this one starts
    public string questDescription;

    public bool IsQuestCompleted()
    {
        return objectives.TrueForAll(obj => obj.isCompleted);
    }

    public bool CanStartQuest()
    {
        return requiredQuest == null || requiredQuest.IsQuestCompleted();
    }

    public void StartQuest()
    {
        #if UNITY_EDITOR
        Debug.Log($"Quest Started: {questName}");
        #endif
        foreach (var objective in objectives)
        {
            objective.StartObjective();
        }
    }
    // In QuestData
public void ResetObjectives()
{
    if (objectives != null)
    {
        foreach (var objective in objectives)
        {
            if (objective != null)
            {
                objective.ResetObjective();
            }
        }
    }
}


}
