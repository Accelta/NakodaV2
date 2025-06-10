using UnityEngine;

public class Target : MonoBehaviour
{
    public ShootTarget linkedObjective;

    private void Start()
    {
        if (linkedObjective != null)
        {
            var manager = FindFirstObjectByType<TargetManager>();
            if (manager != null)
                manager.RegisterTarget(gameObject);
        }
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
