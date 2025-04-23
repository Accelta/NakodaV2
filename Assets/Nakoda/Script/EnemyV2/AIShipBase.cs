using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public abstract class AIShipBase : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float acceleration = 3f;
    public float deceleration = 2f;
    public float turnSpeed = 2f;
    public float stoppingDistance = 5f;

    [Header("Obstacle Avoidance")]
    public float avoidanceDistance = 10f;
    public LayerMask obstacleLayer;
    public int rayCount = 6;
    public float coneAngle = 60f;
    public float visionRange = 20f;
    public float sphereRadius = 1f;
    public bool showGizmos = true;

    [Header("Waypoint Navigation")]
    public List<Transform> waypoints;
    private int currentWaypointIndex = -1;

    protected Rigidbody rb;
    protected BoatBuoyancy buoyancy;
    protected Transform currentTarget;

    [Header("Waypoint Visibility")]
    public bool showWaypoints = true;
    public float waypointRadius = 0.5f;
    
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
            dynamicRayLengths[i] = visionRange;

        ChooseNextWaypoint();
    }

    protected virtual void FixedUpdate()
    {
        if (!buoyancy.IsInWater()) return;

        PerformBehavior();

        if (currentTarget == null)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
            return;
        }

        Vector3 targetDir = (currentTarget.position - transform.position);
        targetDir.y = 0f;
        float distanceToTarget = targetDir.magnitude;

        if (distanceToTarget < stoppingDistance)
        {
            ChooseNextWaypoint();
            return;
        }

        Vector3 avoidanceDir = GetBestDirection(targetDir.normalized);
        if (avoidanceDir == Vector3.zero)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
            avoidanceDir = -transform.forward; // dead end fallback
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.fixedDeltaTime);
        }

        Move(avoidanceDir.normalized);
    }

    private void ChooseNextWaypoint()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        int nextIndex = Random.Range(0, waypoints.Count);
        while (waypoints.Count > 1 && nextIndex == currentWaypointIndex)
        {
            nextIndex = Random.Range(0, waypoints.Count);
        }

        currentWaypointIndex = nextIndex;
        currentTarget = waypoints[currentWaypointIndex];
    }

    protected void Move(Vector3 direction)
    {
        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
        float newYRot = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot.eulerAngles.y, ref smoothTurnVelocity, 0.5f);
        Quaternion smoothRot = Quaternion.Euler(0f, newYRot, 0f);
        rb.MoveRotation(smoothRot);

        rb.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
    }

    private Vector3 GetBestDirection(Vector3 goalDirection)
    {
        Vector3 bestDir = Vector3.zero;
        float bestScore = float.MinValue;
        int obstacleCount = 0;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = (-coneAngle / 2f) + (coneAngle / (rayCount - 1)) * i;
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;

            if (Physics.SphereCast(transform.position, sphereRadius, direction, out RaycastHit hit, visionRange, obstacleLayer))
            {
                obstacleDetected[i] = true;
                dynamicRayLengths[i] = hit.distance;
                obstacleCount++;
            }
            else
            {
                obstacleDetected[i] = false;
                dynamicRayLengths[i] = visionRange;

                // Score this direction based on alignment with goal
                float score = Vector3.Dot(direction, goalDirection);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestDir = direction;
                }
            }
        }

        return obstacleCount == rayCount ? Vector3.zero : bestDir;
    }

    protected abstract void PerformBehavior(); // to be implemented by children

    private void OnDrawGizmos()
    {
        if (!showGizmos || rayCount == 0) return;

        if (obstacleDetected == null || dynamicRayLengths == null || obstacleDetected.Length != rayCount)
        {
            obstacleDetected = new bool[rayCount];
            dynamicRayLengths = new float[rayCount];
            for (int i = 0; i < rayCount; i++)
                dynamicRayLengths[i] = visionRange;
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

        //Gizmoz visibility
        if (showWaypoints && waypoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform waypoint in waypoints)
            {
                if (waypoint !=null)
                {
                    Gizmos.DrawSphere(waypoint.position, waypointRadius);
                }
            }
        }
    }
}
