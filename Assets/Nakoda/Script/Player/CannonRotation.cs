using Unity.Cinemachine;
using UnityEngine;

public class CannonRotation : MonoBehaviour
{
    public Transform mainBody;  // Horizontal rotation
    public Transform barrel;    // Vertical rotation

    public BulletData bulletData;
    public Transform firePoint;

    public float fireRate = 1f;
    private float nextFireTime = 0f;

    public float rotationSpeed = 50f;
    public float maxHorizontalRotation = 60f;
    public float minVerticalRotation = -10f;
    public float maxVerticalRotation = 30f;
    public enum CannonSide { Front, Left, Right }
    public CannonSide cannonSide;


    [Header("Detection Area")]
    public float minHorizontalAngle = -90f; // left limit
    public float maxHorizontalAngle = 90f;  // right limit

    public float detectionRange = 50f;
    public LayerMask enemyLayer;

    public AudioSource shootingAudio;

    public bool isRotationActive = false; // Manual mode
    public bool autoMode = true;          // Turret mode

    [Header("Aiming")]
    public float aimThreshold = 5f; // degrees within which we consider “aimed”


    private float currentHorizontalRotation;
    private float currentVerticalRotation;
    private Transform target;

    [Header("Return to Default")]
    public float returnDelay = 3f;              // Time to wait before returning
    public Vector3 defaultMainRotation = Vector3.zero;  // Default rotation for the main body
    public Vector3 defaultBarrelRotation = Vector3.zero; // Default rotation for the barrel

    private float targetLostTime = -1f;
    private bool returningToDefault = false;


    void Start()
    {
        currentHorizontalRotation = mainBody.localEulerAngles.y;
        currentVerticalRotation = barrel.localEulerAngles.x;

        if (bulletData == null || bulletData.bulletPrefab == null)
            Debug.LogError("BulletData or Bullet Prefab is not assigned!");

        if (shootingAudio == null)
            Debug.LogError("Shooting AudioSource is missing!");
    }

    void Update()
    {
        if (isRotationActive)
        {
            HandleCannonRotation(); // Manual input
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + fireRate;
            }
        }
        else if (autoMode)
        {
            HandleAutoTargeting(); // Turret AI
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

    void HandleAutoTargeting()
{
    // 1) Refresh target if needed
if (target == null || Vector3.Distance(transform.position, target.position) > detectionRange)
{
    
    if (target != null)
        target = null;

    if (targetLostTime < 0f)
        targetLostTime = Time.time;
        target = FindNearestTarget();
}
else
{
    targetLostTime = -1f;
    returningToDefault = false;
}

// If target is null for a while, return to default
if (target == null)
{
    if (Time.time - targetLostTime >= returnDelay)
    {
        returningToDefault = true;
    }

    if (returningToDefault)
    {
        ReturnToDefaultRotation();
    }

    return;
}

    // 2) Compute direction & distance
    Vector3 dirToTarget = target.position - firePoint.position;
    float dist = dirToTarget.magnitude;
    if (dist < 1f) return; // dead‑zone

    // 3) Horizontal rotation (guarded)
    Vector3 flatDir = new Vector3(dirToTarget.x, 0, dirToTarget.z);
    if (flatDir.sqrMagnitude > 0.0001f)
    {
        Quaternion desiredHoriz = Quaternion.LookRotation(flatDir);
        mainBody.localRotation = Quaternion.RotateTowards(
        mainBody.localRotation,
        Quaternion.Euler(defaultMainRotation),
        rotationSpeed * Time.deltaTime
);
    }

     // 4) Calculate vertical angle (both desired and calculated)
    float desiredVerticalAngle = CalculateDesiredVerticalAngle(dirToTarget); // This calculates vertical based on the target
    float calculatedVerticalAngle = CalculateElevationAngle(dist); // This is based on projectile physics

    // 5) Blend the vertical angles (weighted average or smoothing)
    float blendedVerticalAngle = Mathf.Lerp(desiredVerticalAngle, calculatedVerticalAngle, 0.5f); // You can adjust the blend factor (0.5f) for more influence on one angle

    // 6) Smoothly rotate the barrel to the blended vertical angle
    blendedVerticalAngle = Mathf.Clamp(blendedVerticalAngle, minVerticalRotation, maxVerticalRotation);
    currentVerticalRotation = Mathf.MoveTowards(
        currentVerticalRotation,
        blendedVerticalAngle,
        rotationSpeed * Time.deltaTime
    );
    barrel.localRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);

    // 7) Only fire if aimed within threshold
    float horizAngle = Vector3.Angle(mainBody.forward, flatDir.normalized);
    float vertAngle = Mathf.Abs(currentVerticalRotation - blendedVerticalAngle);

    if (Time.time >= nextFireTime && horizAngle <= aimThreshold && vertAngle <= aimThreshold)
    {
        FireBullet();
        nextFireTime = Time.time + fireRate;
    }
}

public float CalculateDesiredVerticalAngle(Vector3 dirToTarget)
{
    // Calculate the angle between the cannon's forward vector and the target
    return Vector3.SignedAngle(mainBody.forward, dirToTarget, mainBody.right);
}

float CalculateElevationAngle(float distance)
{
    // Ensure we have the bullet data assigned
    if (bulletData == null) return 0f;

    // Use the bullet's force (speed) and gravity to calculate the angle
    float velocity = bulletData.bulletForce; // Bullet speed
    float gravity = bulletData.gravity;

    // Calculate angle of elevation based on the projectile motion formula
    float angleRad = Mathf.Asin((gravity * distance) / (velocity * velocity)) / 2;
    float angleDeg = angleRad * Mathf.Rad2Deg;

    return angleDeg;
}
public void RotateManually(float horizInput, float vertInput)
{
    float targetHoriz = currentHorizontalRotation + horizInput;
    float deltaHoriz = Mathf.DeltaAngle(0, targetHoriz);
    currentHorizontalRotation = Mathf.Clamp(deltaHoriz, -maxHorizontalRotation, maxHorizontalRotation);
    mainBody.localRotation = Quaternion.Euler(0, currentHorizontalRotation, 0);

    currentVerticalRotation = Mathf.Clamp(currentVerticalRotation + vertInput, minVerticalRotation, maxVerticalRotation);
    barrel.localRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
}

public void SetRotation(Quaternion horizRot, float verticalAngle, float speed)
{
    mainBody.localRotation = Quaternion.RotateTowards(mainBody.localRotation, horizRot, speed * Time.deltaTime);

    currentVerticalRotation = Mathf.MoveTowards(currentVerticalRotation, verticalAngle, speed * Time.deltaTime);
    currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minVerticalRotation, maxVerticalRotation);
    barrel.localRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
}

public float GetCurrentVerticalAngle()
{
    return currentVerticalRotation;
}
   public Transform FindNearestTarget()
{
    Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
    float closestDist = Mathf.Infinity;
    Transform nearest = null;

    Vector3 forward = mainBody.forward;

    foreach (var hit in hits)
    {
        Rigidbody rb = hit.attachedRigidbody;
        if (rb == null) continue;

        Vector3 toTarget = rb.position - transform.position;
        float dist = toTarget.magnitude;

        // Angle from cannon forward to target (horizontal)
        Vector3 flatDirection = new Vector3(toTarget.x, 0, toTarget.z);
        float angle = Vector3.SignedAngle(forward, flatDirection.normalized, Vector3.up);

        if (dist < closestDist && angle >= minHorizontalAngle && angle <= maxHorizontalAngle)
        {
            closestDist = dist;
            nearest = rb.transform;
        }
    }

    return nearest;
}



    public void FireBullet()
    {
        if (bulletData == null || bulletData.bulletPrefab == null) return;

        GameObject bullet = BulletPool.Instance.GetBullet(firePoint.position, firePoint.rotation);
        bullet.SetActive(true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(firePoint.forward * bulletData.bulletForce, ForceMode.Impulse);
        }

        if (shootingAudio != null)
            shootingAudio.Play();
    }
void OnDrawGizmosSelected()
{
    if (mainBody == null) return;

    Gizmos.color = Color.yellow;

    // Use static forward direction (world space)
    Vector3 forward = Vector3.forward; // or transform.forward for object-based

    Vector3 origin = transform.position;

    Quaternion minAngleRot = Quaternion.AngleAxis(minHorizontalAngle, Vector3.up);
    Quaternion maxAngleRot = Quaternion.AngleAxis(maxHorizontalAngle, Vector3.up);

    Vector3 minDir = minAngleRot * forward;
    Vector3 maxDir = maxAngleRot * forward;

    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(origin, origin + minDir.normalized * detectionRange);

    Gizmos.color = Color.magenta;
    Gizmos.DrawLine(origin, origin + maxDir.normalized * detectionRange);

    DrawDetectionArc(origin, forward, minHorizontalAngle, maxHorizontalAngle, detectionRange, 30);
}

void DrawDetectionArc(Vector3 origin, Vector3 forward, float minAngle, float maxAngle, float radius, int segments)
{
    Gizmos.color = new Color(1f, 1f, 0f, 0.2f); // Transparent yellow

    float angleStep = (maxAngle - minAngle) / segments;

    Vector3 prevPoint = origin + Quaternion.Euler(0, minAngle, 0) * forward * radius;

    for (int i = 1; i <= segments; i++)
    {
        float currentAngle = minAngle + angleStep * i;
        Vector3 nextPoint = origin + Quaternion.Euler(0, currentAngle, 0) * forward * radius;

        Gizmos.DrawLine(origin, nextPoint);
        Gizmos.DrawLine(prevPoint, nextPoint);

        prevPoint = nextPoint;
    }
}

void ReturnToDefaultRotation()
{
    Quaternion defaultMainRot = Quaternion.Euler(defaultMainRotation);
    mainBody.localRotation = Quaternion.RotateTowards(
        mainBody.localRotation,
        Quaternion.Euler(defaultMainRotation),
        rotationSpeed * Time.deltaTime
);

    currentVerticalRotation = Mathf.MoveTowards(
        currentVerticalRotation,
        defaultBarrelRotation.x,
        rotationSpeed * Time.deltaTime
    );
    barrel.localRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
}


}
