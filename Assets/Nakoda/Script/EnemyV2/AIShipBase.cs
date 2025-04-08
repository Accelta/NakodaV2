using UnityEngine;

public abstract class AIShipBase : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float acceleration = 3f;
    public float deceleration = 2f;
    public float turnSpeed = 2f;
    public float stoppingDistance = 5f;
    public float avoidanceDistance = 10f;
    public LayerMask obstacleLayer;
    
    protected Rigidbody rb;
    protected BoatBuoyancy buoyancy;
    protected Transform target;

    // Vision system parameters
    public int rayCount = 6;
    public float coneAngle = 60f;
    public float visionRange = 20f;
    public float sphereRadius = 1f;
    public bool showGizmos = true; // Toggle for Gizmos in Inspector

    private bool[] obstacleDetected;
    private float[] dynamicRayLengths;
    private float currentSpeed = 0f;
    private float smoothTurnVelocity;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        buoyancy = GetComponent<BoatBuoyancy>();
        obstacleDetected = new bool[rayCount];
        dynamicRayLengths = new float[rayCount];
        for (int i = 0; i < rayCount; i++)
        {
            dynamicRayLengths[i] = visionRange;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!buoyancy.IsInWater()) return;

        Vector3 bestDirection = GetBestDirection();

        if (bestDirection == Vector3.zero)
        {
            // Dead end, slow down and turn around gradually
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
            bestDirection = -transform.forward;
        }
        else
        {
            // Smooth acceleration and movement
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.fixedDeltaTime);
        }

        Move(bestDirection);
    }

    protected void Move(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        float newTurnSpeed = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, ref smoothTurnVelocity, 0.5f);
        rb.MoveRotation(Quaternion.Euler(0, newTurnSpeed, 0));
        rb.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
    }

    private Vector3 GetBestDirection()
    {
        Vector3 bestDirection = transform.forward;
        float bestScore = float.MinValue;
        int obstacleCount = 0;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = (-coneAngle / 2) + (coneAngle / (rayCount - 1)) * i;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            
            if (Physics.SphereCast(transform.position, sphereRadius, direction, out RaycastHit hit, visionRange, obstacleLayer))
            {
                obstacleDetected[i] = true;
                dynamicRayLengths[i] = hit.distance; // Adjust ray length to obstacle distance
                obstacleCount++;
            }
            else
            {
                obstacleDetected[i] = false;
                dynamicRayLengths[i] = visionRange; // Reset to normal length
                
                float score = Vector3.Dot(direction, transform.forward);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestDirection = direction;
                }
            }
        }

        return obstacleCount == rayCount ? Vector3.zero : bestDirection;
    }

    public abstract void PerformBehavior();

   void OnDrawGizmos()
{
    if (!showGizmos) return;

    // Ensure arrays are initialized
    if (obstacleDetected == null || dynamicRayLengths == null || obstacleDetected.Length != rayCount || dynamicRayLengths.Length != rayCount)
    {
        obstacleDetected = new bool[rayCount];
        dynamicRayLengths = new float[rayCount];

        for (int i = 0; i < rayCount; i++)
        {
            dynamicRayLengths[i] = visionRange;
        }
    }

    for (int i = 0; i < rayCount; i++)
    {
        float angle = (-coneAngle / 2) + (coneAngle / (rayCount - 1)) * i;
        Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
        Vector3 rayEnd = transform.position + direction * dynamicRayLengths[i];

        Gizmos.color = obstacleDetected[i] ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, rayEnd);
        Gizmos.DrawWireSphere(rayEnd, sphereRadius);
    }
}
}
