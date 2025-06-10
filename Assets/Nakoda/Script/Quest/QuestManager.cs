using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestData> allQuests; // List of all quests in the game
    public List<QuestData> activeQuests = new List<QuestData>();
    public List<QuestData> completedQuests = new List<QuestData>(); // <-- Add this line
    public List<GameObject> barriers; // Barriers that should be removed when certain quests are completed
    private QuestData currentQuest;

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
            if (!activeQuests.Contains(quest) && !completedQuests.Contains(quest) && quest.CanStartQuest())
            {
                activeQuests.Add(quest);
                quest.StartQuest();

                if (currentQuest == null)
                {
                    currentQuest = quest;
                    QuestUIController.Instance?.ShowQuest(quest.questName, quest.questDescription);
                }
            }
        }
    }

    //     public void UpdateObjectives()
    //     {
    //         for (int i = activeQuests.Count - 1; i >= 0; i--)
    //         {
    //             var quest = activeQuests[i];
    //             if (quest.IsQuestCompleted())
    //             {
    // #if UNITY_EDITOR
    //                 Debug.Log($"Quest {quest.questName} Completed!");
    // #endif
    //                 UnlockNewQuests(quest);
    //                 RemoveBarriers(quest);
    //                 completedQuests.Add(quest); // <-- Add to completed list
    //                 activeQuests.RemoveAt(i);   // Optionally remove from active

    //                 if (currentQuest == quest)
    //                 {
    //                     QuestUIController.Instance?.HideQuest();
    //                     currentQuest = null;
    //                 }
    //             }
    //         }
    // if (currentQuest == null && activeQuests.Count > 0)
    // {
    //     currentQuest = activeQuests[0];
    //     QuestUIController.Instance?.UpdateQuest(currentQuest.questName, currentQuest.questDescription);

    //     // Immediately update progress for the new quest
    //     string progressText = GetObjectiveProgressText(currentQuest);
    //     QuestUIController.Instance?.UpdateObjectiveProgress(progressText);
    // }
    // else if (currentQuest != null)
    // {
    //     string progressText = GetObjectiveProgressText(currentQuest);
    //     QuestUIController.Instance?.UpdateObjectiveProgress(progressText);
    // }
    //     }
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
            completedQuests.Add(quest);
            activeQuests.RemoveAt(i);

            if (currentQuest == quest)
            {
                QuestUIController.Instance?.HideQuest();
                currentQuest = null;
            }
        }
    }

    // Always update currentQuest and progress after quest completion
    if (currentQuest == null && activeQuests.Count > 0)
    {
        currentQuest = activeQuests[0];
        QuestUIController.Instance?.UpdateQuest(currentQuest.questName, currentQuest.questDescription);

        // Delay progress update to next frame to allow registration
        QuestMonoHelper.Instance.StartCoroutine(DelayedProgressUpdate());
    }
    else if (currentQuest != null)
    {
        string progressText = GetObjectiveProgressText(currentQuest);
        QuestUIController.Instance?.UpdateObjectiveProgress(progressText);
    }
    else
    {
        QuestUIController.Instance?.UpdateObjectiveProgress("");
    }
}

private System.Collections.IEnumerator DelayedProgressUpdate()
{
    yield return null; // wait one frame
    string progressText = GetObjectiveProgressText(currentQuest);
    QuestUIController.Instance?.UpdateObjectiveProgress(progressText);
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

private string GetObjectiveProgressText(QuestData quest)
{
    if (quest == null || quest.objectives == null || quest.objectives.Count == 0)
        return "";

    List<string> progressLines = new List<string>();
    foreach (var obj in quest.objectives)
    {
        string progress = obj.GetProgress();
        if (!string.IsNullOrEmpty(progress))
            progressLines.Add(progress);
    }

    return string.Join("\n", progressLines);
}


#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void ResetObjectiveOnExitPlaymode()
    {
        EditorApplication.playModeStateChanged += (State) =>
        {
            if (State == PlayModeStateChange.ExitingPlayMode)
            {
                string[] guids = AssetDatabase.FindAssets("t:QuestData");
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    QuestData quest = AssetDatabase.LoadAssetAtPath<QuestData>(path);
                    if (quest != null)
                    {
                        quest.ResetObjectives();
                        EditorUtility.SetDirty(quest);
                    }
                }
                AssetDatabase.SaveAssets();
                Debug.Log("all objective in quest resetted");
            }
        };
    }
#endif
}