// using UnityEngine;

// public class CannonRotation : MonoBehaviour
// {
//     public Transform mainBody;  // The main body of the cannon (sphere for left/right)
//     public Transform barrel;    // The barrel of the cannon (cube for up/down)

//     public GameObject bulletPrefab;     // Assign the bullet prefab in the Inspector
//     public Transform firePoint;         // A point on the barrel where bullets are fired from
//     public float bulletForce = 500f;    // Force applied to the bullet when fired
//     public float fireRate = 1f;         // Time between shots
//     private float nextFireTime = 0f;    // Time until the next shot is allowed

//     public float rotationSpeed = 50f;   // Speed of the rotation
//     public float maxHorizontalRotation = 60f;  // Limit how much the cannon can turn left/right
//     public float minVerticalRotation = -10f;   // Minimum tilt of the barrel (down)
//     public float maxVerticalRotation = 30f;    // Maximum tilt of the barrel (up)
//     public float playerDamage = 20f;   // Damage dealt to the player when hit

//     private float currentHorizontalRotation;
//     private float currentVerticalRotation;
//     public bool isRotationActive = false;

//     void Start()
//     {
//         currentHorizontalRotation = mainBody.localEulerAngles.y;
//         currentVerticalRotation = barrel.localEulerAngles.x;
//     }

//     void Update()
//     {
//         if (isRotationActive)
//         {
//             HandleCannonRotation();

//             // Handle firing the bullet
//             if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
//             {
//                 FireBullet();
//                 nextFireTime = Time.time + fireRate;  // Update the next fire time
//             }
//         }
//     }

//     void HandleCannonRotation()
//     {
//         // Horizontal rotation (main body left/right)
//         float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
//         float targetHorizontalRotation = currentHorizontalRotation + horizontalInput;
//         float deltaHorizontalRotation = Mathf.DeltaAngle(0, targetHorizontalRotation);
//         currentHorizontalRotation = Mathf.Clamp(deltaHorizontalRotation, -maxHorizontalRotation, maxHorizontalRotation);
//         Quaternion horizontalRotation = Quaternion.Euler(0, currentHorizontalRotation, 0);
//         mainBody.localRotation = horizontalRotation;

//         // Vertical rotation (barrel up/down)
//         float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
//         currentVerticalRotation = Mathf.Clamp(currentVerticalRotation + verticalInput, minVerticalRotation, maxVerticalRotation);
//         Quaternion verticalRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
//         barrel.localRotation = verticalRotation;
//     }

//     void FireBullet()
//     {
//         // Create a bullet at the fire point's position and rotation
//         GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//         bullet.SetActive(true);  // Ensure the bullet is active
//         Debug.Log("Bullet Fired");

//         // Get the Rigidbody of the bullet and add force to it
//         Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
//         if (bulletRb != null)
//         {
//             // Apply force in the direction the barrel is facing
//             bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
//             Debug.Log("Bullet Force Applied");
//         }
//         else
//         {
//             Debug.LogError("Rigidbody not found on the bullet!");
//         }
//         // Set the bullet's damage for the player
//         Bullet bulletScript = bullet.GetComponent<Bullet>();
//         if (bulletScript != null)
//         {
//             bulletScript.SetDamage(playerDamage);
//         }
//     }
// }

using UnityEngine;

public class CannonRotation : MonoBehaviour
{
    public Transform mainBody;  // The main body of the cannon (sphere for left/right)
    public Transform barrel;    // The barrel of the cannon (cube for up/down)
    
    public BulletData bulletData; // Gunakan ScriptableObject untuk bullet
    public Transform firePoint; // Titik tembak
    public float fireRate = 1f; // Waktu jeda antar tembakan
    private float nextFireTime = 0f; // Timer untuk menembak

    public float rotationSpeed = 50f; // Kecepatan rotasi
    public float maxHorizontalRotation = 60f; 
    public float minVerticalRotation = -10f; 
    public float maxVerticalRotation = 30f; 

    private float currentHorizontalRotation;
    private float currentVerticalRotation;
    public bool isRotationActive = false;

    void Start()
    {
        currentHorizontalRotation = mainBody.localEulerAngles.y;
        currentVerticalRotation = barrel.localEulerAngles.x;

        // Cek apakah BulletData tidak kosong
        if (bulletData == null || bulletData.bulletPrefab == null)
        {
            Debug.LogError("BulletData atau Bullet Prefab belum diassign!");
        }
    }

    void Update()
    {
        if (isRotationActive)
        {
            HandleCannonRotation();

            // Menembak ketika klik mouse dan cooldown sudah selesai
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + fireRate; // Reset timer tembakan
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

    // void FireBullet()
    // {
    //     // Cek apakah BulletData tidak null
    //     if (bulletData == null || bulletData.bulletPrefab == null)
    //     {
    //         Debug.LogError("BulletData atau Bullet Prefab tidak ditemukan!");
    //         return;
    //     }

    //     // Spawn bullet dari ScriptableObject
    //     GameObject bullet = Instantiate(bulletData.bulletPrefab, firePoint.position, firePoint.rotation);
    //     bullet.SetActive(true);

    //     // Tambahkan force ke bullet jika punya Rigidbody
    //     Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
    //     if (bulletRb != null)
    //     {
    //         bulletRb.AddForce(firePoint.forward * bulletData.bulletForce, ForceMode.Impulse);
    //     }
    //     else
    //     {
    //         Debug.LogError($"Prefab {bulletData.bulletPrefab.name} tidak memiliki Rigidbody! Pastikan prefab memiliki Rigidbody.");
    //     }

    //     // Atur damage peluru
    //     Bullet bulletScript = bullet.GetComponent<Bullet>();
    //     // if (bulletScript != null)
    //     // {
    //     //     bulletScript.SetDamage(bulletData.bulletDamage);
    //     // }
    //     // else
    //     // {
    //     //     Debug.LogWarning($"Prefab {bulletData.bulletPrefab.name} tidak memiliki script Bullet.cs! Pastikan prefab memiliki script.");
    //     // }
    // }
    void FireBullet()
{
    if (bulletData == null || bulletData.bulletPrefab == null)
    {
        Debug.LogError("BulletData atau Bullet Prefab tidak ditemukan!");
        return;
    }

    GameObject bullet = BulletPool.Instance.GetBullet(firePoint.position, firePoint.rotation);
    bullet.SetActive(true);

    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
    if (bulletRb != null)
    {
        bulletRb.linearVelocity = Vector3.zero;  // Reset velocity sebelum menembak lagi
        bulletRb.AddForce(firePoint.forward * bulletData.bulletForce, ForceMode.Impulse);
    }
}
}
