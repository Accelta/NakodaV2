using UnityEngine;

public class Target : MonoBehaviour
{
    public ShootTarget linkedObjective;
     private bool isRegistered = false;

    private void Start()
    {
        if (linkedObjective != null)
        {
            var manager = FindFirstObjectByType<TargetManager>();
            if (manager != null)
                manager.RegisterTarget(gameObject);
        }
        SetVisible(false);

    }

public void ActivateAndRegister()
{
    if (isRegistered) return;

    var manager = FindFirstObjectByType<TargetManager>();
    if (manager != null)
    {
        manager.RegisterTarget(gameObject);
        isRegistered = true;
        SetVisible(true); // Activate renderer and collider
#if UNITY_EDITOR
        Debug.Log($"Target {gameObject.name} activated and registered.");
#endif
    }
    else
    {
#if UNITY_EDITOR
        Debug.LogError("TargetManager not found! Cannot register target.");
#endif
    }
}

private void SetVisible(bool visible)
{
    var renderer = GetComponent<Renderer>();
    if (renderer != null)
        renderer.enabled = visible;

    var collider = GetComponent<Collider>();
    if (collider != null)
        collider.enabled = visible;

#if UNITY_EDITOR
    Debug.Log($"Target {gameObject.name} visibility set to {visible}");
#endif
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (linkedObjective != null)
            {
                linkedObjective.TargetHit(gameObject);

#if UNITY_EDITOR
                Debug.Log("Target hit and quest linked.");
#endif
            }

            Destroy(gameObject);

#if UNITY_EDITOR
            Debug.Log("Target Destroyed");
#endif
        }
    }
}
