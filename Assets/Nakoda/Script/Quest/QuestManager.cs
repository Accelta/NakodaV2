using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    
    public List<QuestData> activeQuests = new List<QuestData>();
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartQuest(QuestData quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            foreach (var objective in quest.objectives)
            {
                objective.StartObjective();
            }
            QuestUI.Instance.UpdateQuestUI(quest);
            Debug.Log($"Quest Started: {quest.questName}");
        }
    }

    public void CheckQuestProgress(QuestData quest)
    {
        if (quest.IsQuestCompleted())
        {
            CompleteQuest(quest);
        }
        else
        {
            QuestUI.Instance.UpdateQuestUI(quest);
        }
    }

    public void CompleteQuest(QuestData quest)
    {
        activeQuests.Remove(quest);
        QuestUI.Instance.ShowQuestCompleted(quest);
        Debug.Log($"Quest Completed: {quest.questName}");
    }
}
