using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string questDescription;
    public List<QuestObjective> objectives;
    public bool isMainQuest; // Determines if it affects progression
    public bool isCompleted; // Tracks completion status

    public List<QuestData> requiredQuests; // Quests that must be completed first
}