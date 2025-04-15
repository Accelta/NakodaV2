using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyTag = "Enemy";  // Set this to your tag for enemies

    // Example target detection method (could be extended)
    public bool IsTargetable()
    {
        return true; // Add logic here, like health checks, etc.
    }
}
