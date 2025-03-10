using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;

    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public Transform objectivesContainer;
    public GameObject objectivePrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateQuestUI(QuestData quest)
    {
        questTitle.text = quest.questName;
        questDescription.text = quest.questDescription;

        // Clear old objectives
        foreach (Transform child in objectivesContainer)
        {
            Destroy(child.gameObject);
        }

        // Display new objectives
        foreach (var objective in quest.objectives)
        {
            GameObject objUI = Instantiate(objectivePrefab, objectivesContainer);
            objUI.GetComponent<TextMeshProUGUI>().text = objective.objectiveDescription + 
                (objective.isCompleted ? " ✅" : " ❌");
        }
    }

    public void ShowQuestCompleted(QuestData quest)
    {
        questTitle.text = $"✔ {quest.questName} Completed!";
    }
}
