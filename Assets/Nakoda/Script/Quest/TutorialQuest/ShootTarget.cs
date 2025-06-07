using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShootTargetTutorial", menuName = "Quests/Objective/Fire At Target")]
public class ShootTarget : QuestObjective
{
    public List<GameObject> targetobjects; // Assign in Inspector
    private HashSet<GameObject> targetSet = new HashSet<GameObject>();
    private CompassTarget currentTarget;
    private bool markerAdded = false;

    public override void StartObjective()
    {
#if UNITY_EDITOR
        Debug.Log($"Objective Started: Fire at all targets ({targetobjects.Count})");
#endif
        targetSet.Clear();
        foreach (var obj in targetobjects)
        {
            if (obj != null)
                targetSet.Add(obj);
        }

        TryRegisterMarker();

        if (!markerAdded)
        {
            QuestMonoHelper.Instance.StartCoroutine(WaitAndRegisterMarker());
        }
    }

    private void TryRegisterMarker()
    {
        GameObject targetObject = GetFirstValidTarget();
        if (targetObject != null)
        {
            currentTarget = targetObject.GetComponent<CompassTarget>();
            if (currentTarget != null)
            {
                CompassManager.Instance?.AddMarker(currentTarget);
                markerAdded = true;
#if UNITY_EDITOR
                Debug.Log("Compass marker registered successfully.");
#endif
            }
        }
    }

    private GameObject GetFirstValidTarget()
    {
        foreach (var obj in targetSet)
        {
            if (obj != null)
                return obj;
        }
        return null;
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

#if UNITY_EDITOR
        if (!markerAdded)
        {
            Debug.LogWarning("Target marker not found within timeout.");
        }
#endif
    }

    // public override void CheckObjectiveCompletion()
    // {
    //     // TargetHit is called externally
    // }

    // Call this when a target is hit
    public void TargetHit(GameObject hitObject)
    {
        if (targetSet.Contains(hitObject))
        {
            targetSet.Remove(hitObject);
#if UNITY_EDITOR
            Debug.Log($"Target {hitObject.name} hit and removed. Remaining: {targetSet.Count}");
#endif
            if (currentTarget != null)
            {
                CompassManager.Instance?.RemoveMarker(currentTarget);
                markerAdded = false;
            }

            if (targetSet.Count == 0)
            {
                CompleteObjective();
            }
            else
            {
                TryRegisterMarker();
                if (!markerAdded)
                    QuestMonoHelper.Instance.StartCoroutine(WaitAndRegisterMarker());
            }
        }
    }
}