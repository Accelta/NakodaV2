using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest Data")]
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
}
