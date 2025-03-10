using UnityEngine;

[CreateAssetMenu(menuName = "Quest System/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string questDescription;
    public QuestObjective[] objectives;

    public bool IsQuestCompleted()
    {
        foreach (var objective in objectives)
        {
            if (!objective.isCompleted)
                return false;
        }
        return true;
    }
}
