// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(BoatBuoyancy))]
// public class EnemyMovement : MonoBehaviour
// {
//     private Rigidbody rb;
//     private BoatBuoyancy floater;
//     private Vector3 wanderTarget;
//     private float currentSpeed;

//     public float maxSpeed;
//     public float acceleration;
//     public float turnSpeed;
//     public float wanderRadius;
//     public float dragUnder = 2f;
//     public float dragOver = 0.5f;

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody>();
//         floater = GetComponent<BoatBuoyancy>();
//     }

//     public void SetMovementStats(float maxSpeed, float acceleration, float turnSpeed, float wanderRadius, float dragUnder, float dragOver)
//     {
//         this.maxSpeed = maxSpeed;
//         this.acceleration = acceleration;
//         this.turnSpeed = turnSpeed;
//         this.wanderRadius = wanderRadius;
//         this.dragUnder = dragUnder;
//         this.dragOver = dragOver;
//         SetWanderTarget();
//     }

//     void FixedUpdate()
//     {
//         AdjustDrag();  // Update drag based on whether the enemy is underwater or above
//     }

//     public void Wander()
//     {
//         if (Vector3.Distance(transform.position, wanderTarget) <= 1f)
//         {
//             SetWanderTarget();
//         }
//         MoveTowardsTarget(wanderTarget, maxSpeed * 0.5f);  // Wander dengan kecepatan lebih lambat
//     }

//     public void Chase(Transform target) => MoveTowardsTarget(target.position, maxSpeed);

//     public void Engage(Transform target) => MoveTowardsTarget(target.position, maxSpeed * 0.75f);

//     private void SetWanderTarget()
//     {
//         Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
//         wanderTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
//     }

//     private void MoveTowardsTarget(Vector3 target, float targetSpeed)
//     {
//         Vector3 direction = (target - transform.position).normalized;
//         float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
//         float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentSpeed, turnSpeed);
//         transform.rotation = Quaternion.Euler(0f, angle, 0f);
//         currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
//         rb.MovePosition(rb.position + transform.forward * currentSpeed * Time.deltaTime);
//     }

//     private void AdjustDrag()
//     {
//         rb.linearDamping = floater.underwater ? dragUnder : dragOver;
//     }
// }
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoatBuoyancy))]
public class EnemyMovement : MonoBehaviour
{
    private Rigidbody rb;
    private BoatBuoyancy buoyancy;
    private Vector3 wanderTarget;
    private float currentSpeed;

    public float maxSpeed;
    public float acceleration;
    public float turnSpeed;
    public float wanderRadius;
    public float dragUnder = 2f;
    public float dragOver = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        buoyancy = GetComponent<BoatBuoyancy>();
    }

    public void SetMovementStats(float maxSpeed, float acceleration, float turnSpeed, float wanderRadius, float dragUnder, float dragOver)
    {
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.turnSpeed = turnSpeed;
        this.wanderRadius = wanderRadius;
        this.dragUnder = dragUnder;
        this.dragOver = dragOver;
        SetWanderTarget();
    }

    void FixedUpdate()
    {
        AdjustDrag();
    }

    public void Wander()
    {
        if (Vector3.Distance(transform.position, wanderTarget) <= 1f)
        {
            SetWanderTarget();
        }
        MoveTowardsTarget(wanderTarget, maxSpeed * 0.5f);
    }

    public void Chase(Transform target) => MoveTowardsTarget(target.position, maxSpeed);

    public void Engage(Transform target) => MoveTowardsTarget(target.position, maxSpeed * 0.75f);

    private void SetWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
    }

    private void MoveTowardsTarget(Vector3 target, float targetSpeed)
    {
        Vector3 direction = (target - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentSpeed, turnSpeed);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        rb.MovePosition(rb.position + transform.forward * currentSpeed * Time.deltaTime);
    }

    private void AdjustDrag()
    {
        if (buoyancy == null || buoyancy.floatPoints.Length == 0) return;

        // Count submerged float points
        int submergedPoints = 0;
        foreach (Transform point in buoyancy.floatPoints)
        {
            if (point.position.y < buoyancy.targetSurface.transform.position.y)
                submergedPoints++;
        }

        float waterFactor = (float)submergedPoints / buoyancy.floatPoints.Length;
        rb.linearDamping = Mathf.Lerp(dragOver, dragUnder, waterFactor);
    }
}
