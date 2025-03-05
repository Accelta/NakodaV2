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
        int randomIndex = Random.Range(0, cannonPositions.Length);
        Transform selectedCannon = cannonPositions[randomIndex];
        
        GameObject cannonball = Instantiate(cannonballPrefab, selectedCannon.position, Quaternion.identity);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.AddForce(selectedCannon.forward * shootForce, ForceMode.Impulse);
    }
}
