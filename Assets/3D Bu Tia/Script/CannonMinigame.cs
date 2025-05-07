using UnityEngine;

public class CannonMinigame : MonoBehaviour

{
    public Transform firePoint;
    public BulletData bulletData;

    public float minFireDelay = 1f;
    public float maxFireDelay = 3f;

    private float nextFireTime;

    public GameObject muzzleFlashPrefab;



    void Start()
    {
        ResetFireTime();
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireBullet();
            ResetFireTime();
        }
    }

    void FireBullet()
    {
        if (bulletData == null || bulletData.bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);

        // 👉 Set the damage using BulletData
         Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
        bulletScript.SetDamage(bulletData.bulletDamage);
        }


        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(firePoint.forward * bulletData.bulletForce, ForceMode.Impulse);
        }
        // 🔥 Instantiate VFX prefab if assigned
    if (muzzleFlashPrefab != null)
    {
        GameObject vfx = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        Destroy(vfx, 5f); // Optional: destroy after 2 seconds
    }
    }

    void ResetFireTime()
    {
        nextFireTime = Time.time + Random.Range(minFireDelay, maxFireDelay);
    }
}