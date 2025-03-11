using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> availableQuests; // All possible quests in this level
    public List<QuestData> activeQuests; // Quests that the player is currently doing
    public List<QuestData> completedQuests; // Finished quests

    public GameObject[] lockedRegions; // Reference to areas that will be unlocked

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
            else Destroy(gameObject);
    }

    void Start()
    {
        CheckForUnlockedQuests();
    }

    public void StartQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest) || completedQuests.Contains(quest))
            return;

        // Check if all prerequisites are completed
        foreach (QuestData requiredQuest in quest.requiredQuests)
        {
            if (!completedQuests.Contains(requiredQuest))
            {
                Debug.LogWarning("Cannot start quest: " + quest.questName + " because " + requiredQuest.questName + " is not completed.");
                return;
            }
        }

        activeQuests.Add(quest);
        Debug.Log("Quest Started: " + quest.questName);
    }

    public void CompleteQuest(QuestData quest)
    {
        if (!activeQuests.Contains(quest))
            return;

        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        quest.isCompleted = true;
        Debug.Log("Quest Completed: " + quest.questName);

        if (quest.isMainQuest)
        {
            UnlockNextRegion();
        }

        CheckForUnlockedQuests();
    }

    void UnlockNextRegion()
    {
        foreach (GameObject region in lockedRegions)
        {
            region.SetActive(true); // Unlock the region
        }
        Debug.Log("New region unlocked!");
    }

    void CheckForUnlockedQuests()
    {
        foreach (QuestData quest in availableQuests)
        {
            if (!activeQuests.Contains(quest) && !completedQuests.Contains(quest))
            {
                bool prerequisitesMet = true;
                foreach (QuestData requiredQuest in quest.requiredQuests)
                {
                    if (!completedQuests.Contains(requiredQuest))
                    {
                        prerequisitesMet = false;
                        break;
                    }
                }

                if (prerequisitesMet)
                {
                    StartQuest(quest);
                }
            }
        }
    }
}
