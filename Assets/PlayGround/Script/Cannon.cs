using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bulletPrefab;  // The bullet prefab to shoot
    public Transform firePoint;      // The position where bullets are spawned

    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
