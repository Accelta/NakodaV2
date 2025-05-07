using UnityEngine;

public class PlayerCannonMinigame : MonoBehaviour
{
    public float chargeTime = 10f;
    public float cooldownTime = 20f;
    public BulletData bulletData;
    public Transform firePoint;
    public GameObject muzzleFlashPrefab;

    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;
    private bool playerInside = false;
    private bool isCoolingDown = false;

    void Update()
    {
        if (playerInside && !isCoolingDown)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer >= chargeTime)
            {
                FireBullet();
                StartCooldown();
            }
        }

        if (isCoolingDown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                isCoolingDown = false;
                cooldownTimer = 0f;
                Debug.Log("Cannon ready again.");
            }
        }
    }

    void FireBullet()
    {
        if (bulletData == null || bulletData.bulletPrefab == null || firePoint == null) return;

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

        if (muzzleFlashPrefab != null)
        {
            GameObject vfx = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(vfx, 5f);
        }
    }

    private void StartCooldown()
    {
        isCoolingDown = true;
        chargeTimer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered trigger zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            chargeTimer = 0f;
            Debug.Log("Player left trigger zone.");
        }
    }
}
