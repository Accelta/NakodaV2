using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject cannonballPrefab;
    public Transform[] cannonPositions;
    public float shootInterval = 2f;
    public float shootForce = 10f;

    void Start()
    {
        InvokeRepeating(nameof(ShootCannonballs), 1f, shootInterval);
    }

    void ShootCannonballs()
    {
        foreach (Transform cannon in cannonPositions)
        {
            GameObject cannonball = Instantiate(cannonballPrefab, cannon.position, Quaternion.identity);
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();
            rb.AddForce(cannon.forward * shootForce, ForceMode.Impulse);
        }
    }
}
