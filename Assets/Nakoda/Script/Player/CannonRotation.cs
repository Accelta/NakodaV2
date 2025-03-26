using UnityEngine;

public class CannonRotation : MonoBehaviour
{
    public Transform mainBody;  // The main body of the cannon (sphere for left/right)
    public Transform barrel;    // The barrel of the cannon (cube for up/down)

    public BulletData bulletData; // ScriptableObject for bullet
    public Transform firePoint; // Fire point
    public float fireRate = 1f; // Time between shots
    private float nextFireTime = 0f; // Shooting cooldown timer

    public float rotationSpeed = 50f; // Rotation speed
    public float maxHorizontalRotation = 60f; 
    public float minVerticalRotation = -10f; 
    public float maxVerticalRotation = 30f; 

    private float currentHorizontalRotation;
    private float currentVerticalRotation;
    public bool isRotationActive = false;

    public AudioSource shootingAudio; // Add this for the shooting sound

    void Start()
    {
        currentHorizontalRotation = mainBody.localEulerAngles.y;
        currentVerticalRotation = barrel.localEulerAngles.x;

        // Check if BulletData is assigned
        if (bulletData == null || bulletData.bulletPrefab == null)
        {
            Debug.LogError("BulletData or Bullet Prefab is not assigned!");
        }

        // Check if AudioSource is assigned
        if (shootingAudio == null)
        {
            Debug.LogError("Shooting AudioSource is missing!");
        }
    }

    void Update()
    {
        if (isRotationActive)
        {
            HandleCannonRotation();

            // Fire when clicking mouse button and cooldown is over
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + fireRate; // Reset cooldown
            }
        }
    }

    void HandleCannonRotation()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float targetHorizontalRotation = currentHorizontalRotation + horizontalInput;
        float deltaHorizontalRotation = Mathf.DeltaAngle(0, targetHorizontalRotation);
        currentHorizontalRotation = Mathf.Clamp(deltaHorizontalRotation, -maxHorizontalRotation, maxHorizontalRotation);
        mainBody.localRotation = Quaternion.Euler(0, currentHorizontalRotation, 0);

        float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation + verticalInput, minVerticalRotation, maxVerticalRotation);
        barrel.localRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
    }

    void FireBullet()
    {
        if (bulletData == null || bulletData.bulletPrefab == null)
        {
            Debug.LogError("BulletData or Bullet Prefab not found!");
            return;
        }

        GameObject bullet = BulletPool.Instance.GetBullet(firePoint.position, firePoint.rotation);
        bullet.SetActive(true);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = Vector3.zero;  // Reset velocity before firing again
            bulletRb.AddForce(firePoint.forward * bulletData.bulletForce, ForceMode.Impulse);
        }

        // Play shooting sound
        if (shootingAudio != null)
        {
            shootingAudio.Play();
        }
    }
}
