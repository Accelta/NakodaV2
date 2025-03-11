using UnityEngine;

[CreateAssetMenu(fileName = "ShootTargetTutorial", menuName = "Quests/Fire At Target")]
public class ShootTarget : QuestObjective
{
     public GameObject targetObject;
    private int hitCount = 0;
    public int requiredHits = 1; // Adjust as needed

    public override void StartObjective()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object not assigned!");
        }
    }

    public override void CheckObjectiveCompletion()
    {
        if (hitCount >= requiredHits)
        {
            CompleteObjective();
        }
    }

    public void RegisterHit()
    {
        hitCount++;
        CheckObjectiveCompletion();
    }
}
