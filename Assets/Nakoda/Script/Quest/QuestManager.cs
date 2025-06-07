using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestData> allQuests; // List of all quests in the game
    public List<QuestData> activeQuests = new List<QuestData>();
    public List<QuestData> completedQuests = new List<QuestData>(); // <-- Add this line
    public List<GameObject> barriers; // Barriers that should be removed when certain quests are completed

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ResetAllQuestObjectives();  // ✅ Reset state when the game starts
        CheckAvailableQuests();
    }

    public void CheckAvailableQuests()
    {
        foreach (var quest in allQuests)
        {
            if (!activeQuests.Contains(quest) && !completedQuests.Contains(quest) && quest.CanStartQuest())
            {
                activeQuests.Add(quest);
                quest.StartQuest();
            }
        }
    }

    public void UpdateObjectives()
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            var quest = activeQuests[i];
            if (quest.IsQuestCompleted())
            {
                #if UNITY_EDITOR
                Debug.Log($"Quest {quest.questName} Completed!");
                #endif
                UnlockNewQuests(quest);
                RemoveBarriers(quest);
                completedQuests.Add(quest); // <-- Add to completed list
                activeQuests.RemoveAt(i);   // Optionally remove from active
            }
        }
    }

    private void UnlockNewQuests(QuestData completedQuest)
    {
        foreach (var quest in allQuests)
        {
            if (!activeQuests.Contains(quest) && !completedQuests.Contains(quest) && quest.requiredQuest == completedQuest)
            {
                activeQuests.Add(quest);
                #if UNITY_EDITOR
                Debug.Log($"New Quest Unlocked: {quest.questName}");
                #endif
                quest.StartQuest();
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
                #if UNITY_EDITOR
                Debug.Log($"Barrier Removed: {barrier.gameObject.name}");
                #endif
            }
        }
    }

    private void ResetAllQuestObjectives()
{
    foreach (var quest in allQuests)
    {
        foreach (var objective in quest.objectives)
        {
            objective.ResetObjective(); // This uses the ResetObjective() method you added to QuestObjective
        }
    }
}
}