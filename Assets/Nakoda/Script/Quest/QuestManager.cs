using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestData> allQuests; // List of all quests in the game
    public List<QuestData> activeQuests = new List<QuestData>();
    public List<GameObject> barriers; // Barriers that should be removed when certain quests are completed

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CheckAvailableQuests();
    }

public void CheckAvailableQuests()
{
    foreach (var quest in allQuests)
    {
        if (!activeQuests.Contains(quest) && quest.CanStartQuest())
        {
            activeQuests.Add(quest);
            quest.StartQuest();  // 🔥 Now starts objectives when quest starts
        }
    }
}

    public void UpdateObjectives()
    {
        foreach (var quest in activeQuests)
        {
            if (quest.IsQuestCompleted())
            {
                Debug.Log($"Quest {quest.questName} Completed!");
                UnlockNewQuests(quest);
                RemoveBarriers(quest);
            }
        }
    }

    private void UnlockNewQuests(QuestData completedQuest)
    {
        foreach (var quest in allQuests)
        {
            if (!activeQuests.Contains(quest) && quest.requiredQuest == completedQuest)
            {
                activeQuests.Add(quest);
                Debug.Log($"New Quest Unlocked: {quest.questName}");
            }
        }
    }

    private void RemoveBarriers(QuestData completedQuest)
    {
        for (int i = barriers.Count - 1; i >= 0; i--)
        {
            Barrier barrier = barriers[i].GetComponent<Barrier>();
            if (barrier != null && barrier.unlockingQuest == completedQuest)
            {
                Destroy(barriers[i]);
                barriers.RemoveAt(i);
                Debug.Log($"Barrier Removed: {barrier.gameObject.name}");
            }
        }
    }
}
