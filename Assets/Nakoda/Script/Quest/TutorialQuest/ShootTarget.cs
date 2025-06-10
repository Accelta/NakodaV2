using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ShootTargetTutorial", menuName = "Quests/Objective/Fire At Target")]
public class ShootTarget : QuestObjective
{
    private TargetManager manager;
    private CompassTarget currentTarget;
    private bool markerAdded = false;

    public void RegisterManager(TargetManager mgr)
    {
        manager = mgr;
    }

    public override void StartObjective()
    {
#if UNITY_EDITOR
        Debug.Log($"Objective Started: Fire at all targets.");
#endif
        markerAdded = false;
        TryRegisterMarker();

        if (!markerAdded)
        {
            QuestMonoHelper.Instance.StartCoroutine(WaitAndRegisterMarker());
        }
    }

    private void TryRegisterMarker()
    {
        GameObject targetObject = manager?.GetFirstValidTarget();
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

    private IEnumerator WaitAndRegisterMarker()
    {
        float timer = 0f;
        float timeout = 5f;

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

    public void TargetHit(GameObject hitObject)
    {
        if (manager == null) return;

        manager.RemoveTarget(hitObject);
        QuestUIController.Instance?.UpdateObjectiveProgress(GetProgress());

#if UNITY_EDITOR
        Debug.Log($"Target {hitObject.name} hit. Remaining: {manager.RemainingCount()}");
#endif

        if (currentTarget != null)
        {
            CompassManager.Instance?.RemoveMarker(currentTarget);
            markerAdded = false;
        }

        if (manager.RemainingCount() == 0)
        {
            CompleteObjective();
            QuestUIController.Instance?.UpdateObjectiveProgress(GetProgress());
        }
        else
        {
            TryRegisterMarker();
            if (!markerAdded)
                QuestMonoHelper.Instance.StartCoroutine(WaitAndRegisterMarker());
        }
    }

public override string GetProgress()
{
    if (manager == null) return "";
    int remaining = manager.RemainingCount();
    int total = manager.GetTotalCount();

    int shot = total - remaining;
    return $"{shot}/{total}";
}
}
