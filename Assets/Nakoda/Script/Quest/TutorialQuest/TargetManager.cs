using UnityEngine;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    public ShootTarget objective;
    private HashSet<GameObject> targets = new HashSet<GameObject>();
    private int initialCount = 0;

    private void Awake()
    {
        if (objective != null)
        {
            objective.RegisterManager(this);
        }
    }

    public void RegisterTarget(GameObject target)
    {
        if (!targets.Contains(target))
        {
            targets.Add(target);
            initialCount = targets.Count;
#if UNITY_EDITOR
            Debug.Log($"Registered target: {target.name}");
#endif
        }
    }

    public void RemoveTarget(GameObject target)
    {
        if (target == null || !targets.Contains(target)) return;
        targets.Remove(target);
    }

    public GameObject GetFirstValidTarget()
    {
        foreach (var t in targets)
        {
            if (t != null)
                return t;
        }
        return null;
    }

    public int RemainingCount()
    {
        // Bersihkan nulls
        targets.RemoveWhere(t => t == null);
        return targets.Count;
    }
    public int GetTotalCount()
{
    return initialCount;
}
}
