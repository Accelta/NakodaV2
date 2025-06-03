// using UnityEngine;

// [CreateAssetMenu(fileName = "ShootTargetTutorial", menuName = "Quests/Fire At Target")]
// public class ShootTarget : QuestObjective
// {
//  public string targetTag = "QuestTarget"; // Set tag for targets in the scene
 

//     public override void StartObjective()
//     {
//         Debug.Log($"Objective Started: Fire at an object tagged '{targetTag}'");

//     }

//     public override void CheckObjectiveCompletion()
//     {
//         // The target itself calls TargetHit()
//     }

//     public void TargetHit()
//     {
//         CompleteObjective();
//     }
// }
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ShootTargetTutorial", menuName = "Quests/Fire At Target")]
public class ShootTarget : QuestObjective
{
    public string targetTag = "QuestTarget"; // Tag objek target di scene
    private CompassTarget currentTarget;
    private bool markerAdded = false;

    public override void StartObjective()
    {
        Debug.Log($"Objective Started: Fire at an object tagged '{targetTag}'");

        TryRegisterMarker(); // Coba langsung

        if (!markerAdded)
        {
            QuestMonoHelper.Instance.StartCoroutine(WaitAndRegisterMarker());
        }
    }

    private void TryRegisterMarker()
    {
        GameObject targetObject = GameObject.FindWithTag(targetTag);
        if (targetObject != null)
        {
            currentTarget = targetObject.GetComponent<CompassTarget>();
            if (currentTarget != null)
            {
                CompassManager.Instance?.AddMarker(currentTarget);
                markerAdded = true;
                Debug.Log("Compass marker registered successfully.");
            }
        }
    }

    private IEnumerator WaitAndRegisterMarker()
    {
        float timer = 0f;
        float timeout = 5f; // Max wait time

        while (!markerAdded && timer < timeout)
        {
            TryRegisterMarker();
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        if (!markerAdded)
        {
            Debug.LogWarning("Target marker not found within timeout.");
        }
    }

    public override void CheckObjectiveCompletion()
    {
        // TargetHit dipanggil dari luar
    }

    public void TargetHit()
    {
        CompleteObjective();

        if (currentTarget != null)
        {
            CompassManager.Instance?.RemoveMarker(currentTarget);
        }
    }
}
