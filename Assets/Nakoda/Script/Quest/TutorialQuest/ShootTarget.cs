using UnityEngine;

[CreateAssetMenu(fileName = "ShootTargetTutorial", menuName = "Quests/Fire At Target")]
public class ShootTarget : QuestObjective
{
 public string targetTag = "QuestTarget"; // Set tag for targets in the scene

    public override void StartObjective()
    {
        Debug.Log($"Objective Started: Fire at an object tagged '{targetTag}'");
    }

    public override void CheckObjectiveCompletion()
    {
        // The target itself calls TargetHit()
    }

    public void TargetHit()
    {
        CompleteObjective();
    }
}
