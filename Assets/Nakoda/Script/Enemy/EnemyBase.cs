// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(BoatBuoyancy))]
// [RequireComponent(typeof(Health))]
// public abstract class EnemyBase : MonoBehaviour
// {
//     public enum State { Wandering, Chasing, Engaging }
//     public State currentState = State.Wandering;

//     public Transform player;

//     // Change cannonMainBody and cannonBarrel to arrays/lists for multiple cannons
//     public Transform[] cannonMainBodies; // Main body of each cannon
//     public Transform[] cannonBarrels;    // Barrel of each cannon
//     public Transform[] firePoints;       // Fire points for each cannon

//     public float detectionRange = 50f;  
//     public float fireRate = 1f;
//     public float bulletForce = 500f;
//     public GameObject bulletPrefab;
//     public float cannonRotationSpeed = 5f;

//     private BoatBuoyancy floater;
//     public float dragUnder = 2f;
//     public float dragOver = 0.5f;

//     public float maxSpeed = 20f;
//     public float acceleration = 5f;
//     public float turnSpeed = 2f;
//     public float rudderEffectiveness = 1f;

//     private float currentSpeed = 0f;
//     private float rudderInput = 0f;
//     private Rigidbody rb;
//     private Vector3 spawnPoint;
//     private Vector3 wanderTarget;
//     private float nextFireTime = 0f;

//     protected Health healthComponent;
//     protected float attackPower;
//     protected float wanderRadius = 20f;
//     public float engageRange = 10f;

//     protected virtual void Awake()
//     {
//         rb = GetComponent<Rigidbody>();
//         floater = GetComponent<BoatBuoyancy>();
//         healthComponent = GetComponent<Health>();

//         healthComponent.OnDeath += Die;
//         AssignStats();
//         healthComponent.InitializeHealth(healthComponent.maxHealth);
//     }

//     void Start()
//     {
//         spawnPoint = transform.position;  // Enemy's initial position as spawn point
//         SetWanderTarget();
//     }

//     void FixedUpdate()
//     {
//         AdjustDrag();
//         switch (currentState)
//         {
//             case State.Wandering:
//                 WanderAroundSpawn();
//                 LookForPlayer();
//                 break;

//             case State.Chasing:
//                 ChasePlayer();
//                 RotateCannonsTowardPlayer();
//                 if (IsInEngageRange())
//                 {
//                     currentState = State.Engaging;
//                 }
//                 break;

//             case State.Engaging:
//                 EngagePlayer();
//                 RotateCannonsTowardPlayer();
//                 TryFireAtPlayer();
//                 if (!IsInEngageRange())
//                 {
//                     currentState = State.Chasing;
//                 }
//                 break;
//         }
//         MoveEnemy();
//     }

//     void WanderAroundSpawn()
//     {
//         if (Vector3.Distance(transform.position, wanderTarget) <= 1f)
//         {
//             SetWanderTarget();
//         }
//         else
//         {
//             MoveTowardsTarget(wanderTarget, GetWanderSpeed());
//         }
//     }

//     void SetWanderTarget()
//     {
//         Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
//         wanderTarget = new Vector3(spawnPoint.x + randomCircle.x, transform.position.y, spawnPoint.z + randomCircle.y);
//     }

//     void LookForPlayer()
//     {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);
//         if (distanceToPlayer <= detectionRange)
//         {
//             currentState = State.Chasing;
//         }
//     }

//     protected void ChasePlayer()
//     {
//         MoveTowardsTarget(player.position, maxSpeed);
//     }

//     protected void EngagePlayer()
//     {
//         Rigidbody playerRb = player.GetComponent<Rigidbody>();
//         if (playerRb != null)
//         {
//             MoveTowardsTarget(player.position, playerRb.linearVelocity.magnitude);
//         }
//     }

//     bool IsInEngageRange()
//     {
//         return Vector3.Distance(transform.position, player.position) <= engageRange;
//     }

//     void MoveTowardsTarget(Vector3 target, float targetSpeed)
//     {
//         Vector3 direction = (target - transform.position).normalized;
//         float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
//         float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rudderInput, turnSpeed / rudderEffectiveness);
//         transform.rotation = Quaternion.Euler(0f, angle, 0f);

//         currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
//     }

//     void MoveEnemy()
//     {
//         Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
//         rb.MovePosition(rb.position + forwardMovement);
//     }

//     // Rotate all cannons toward the player
//     protected void RotateCannonsTowardPlayer()
//     {
//         for (int i = 0; i < cannonMainBodies.Length; i++)
//         {
//             Vector3 directionToPlayer = (player.position - cannonMainBodies[i].position).normalized;
//             Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

//             // Smoothly rotate the cannon toward the player
//             cannonMainBodies[i].rotation = Quaternion.Slerp(cannonMainBodies[i].rotation, lookRotation, cannonRotationSpeed * Time.deltaTime);

//             // Optional vertical barrel rotation
//             Vector3 flatDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
//             float barrelAngle = Vector3.Angle(flatDirection, directionToPlayer);
//             cannonBarrels[i].localRotation = Quaternion.Euler(-barrelAngle, 0, 0);
//         }
//     }

//     // Fire from all cannons at the player
//     protected void TryFireAtPlayer()
//     {
//         if (Time.time >= nextFireTime)
//         {
//             for (int i = 0; i < firePoints.Length; i++)
//             {
//                 FireBullet(firePoints[i]);
//             }
//             nextFireTime = Time.time + 1f / fireRate;
//         }
//     }

//     // Fire bullet from specific fire point
//     protected void FireBullet(Transform firePoint)
//     {
//         GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//         bullet.SetActive(true);
//         Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
//         bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

//         Bullet bulletScript = bullet.GetComponent<Bullet>();
//         if (bulletScript != null)
//         {
//             bulletScript.SetDamage(attackPower);
//         }
//     }

//     public void TakeDamage(float damage)
//     {
//         healthComponent.TakeDamage(damage);
//     }

//     protected virtual void Die()
//     {
//         Destroy(gameObject);
//     }

//     void AdjustDrag()
//     {
//         rb.linearDamping = floater.underwater ? dragUnder : dragOver;
//     }

//     protected abstract void AssignStats();

//     protected abstract float GetWanderSpeed();

//     protected abstract float GetChaseSpeed();
// }


// // using UnityEngine;

// // [RequireComponent(typeof(Rigidbody))]
// // [RequireComponent(typeof(Floater))]
// // [RequireComponent(typeof(Health))]
// // [RequireComponent(typeof(EnemyMovement))]
// // [RequireComponent(typeof(EnemyCannon))]
// // public abstract class EnemyBase : MonoBehaviour
// // {
// //     public enum State { Wandering, Chasing, Engaging }
// //     public State currentState = State.Wandering;

// //     [Header("References")]
// //     public Transform player;

// //     private EnemyMovement movement;
// //     private EnemyCannon cannon;
// //     protected Health healthComponent;

// //     protected float attackPower;
// //     protected float maxSpeed;
// //     protected float fireRate;

// //     protected virtual void Awake()
// //     {
// //         movement = GetComponent<EnemyMovement>();
// //         cannon = GetComponent<EnemyCannon>();
// //         healthComponent = GetComponent<Health>();

// //         healthComponent.OnDeath += Die;
// //         AssignStats();
// //         healthComponent.InitializeHealth(healthComponent.maxHealth);

// //         movement.SetMovementStats(maxSpeed, 5f, 2f, 20f, 2f, 0.5f); // Example values for acceleration, turnSpeed, wanderRadius, dragUnder, dragOver
// //         cannon.SetCannonStats(fireRate, 500f); // Example value for bulletForce

// //         currentState = State.Wandering;
// //     }

// //     void Update()
// //     {
// //         switch (currentState)
// //         {
// //             case State.Wandering:
// //                 movement.Wander();
// //                 if (IsPlayerInRange())
// //                     currentState = State.Chasing;
// //                 break;

// //             case State.Chasing:
// //                 movement.Chase(player);
// //                 cannon.RotateToward(player);
// //                 if (IsPlayerInEngageRange())
// //                     currentState = State.Engaging;
// //                 break;

// //             case State.Engaging:
// //                 movement.Engage(player);
// //                 cannon.RotateToward(player);
// //                 cannon.TryFire();
// //                 if (!IsPlayerInEngageRange())
// //                     currentState = State.Chasing;
// //                 break;
// //         }
// //     }

// //     protected bool IsPlayerInRange()
// //     {
// //         return Vector3.Distance(transform.position, player.position) <= 50f; // Example detection range
// //     }

// //     protected bool IsPlayerInEngageRange()
// //     {
// //         return Vector3.Distance(transform.position, player.position) <= 10f; // Example engage range
// //     }

// //     public void TakeDamage(float damage)
// //     {
// //         healthComponent.TakeDamage(damage);
// //     }

// //     protected virtual void Die()
// //     {
// //         Destroy(gameObject);
// //     }

// //     protected abstract void AssignStats();
// //     protected abstract float GetChaseSpeed();
// //     protected abstract float GetWanderSpeed();
// // }
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoatBuoyancy))]
[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour
{
    public enum State { Wandering, Chasing, Engaging }
    public State currentState = State.Wandering;

    public Transform player;

    public Transform[] cannonMainBodies;
    public Transform[] cannonBarrels;
    public Transform[] firePoints;

    public float detectionRange = 50f;
    public float fireRate = 1f;
    public float bulletForce = 500f;
    public GameObject bulletPrefab;
    public float cannonRotationSpeed = 5f;

    private BoatBuoyancy buoyancy;
    private Rigidbody rb;
    private Vector3 wanderTarget;
    private float nextFireTime = 0f;

    public float dragUnder = 2f;
    public float dragOver = 0.5f;

    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float turnSpeed = 2f;
    public float rudderEffectiveness = 1f;

    private float currentSpeed = 0f;
    private float rudderInput = 0f;

    protected Health healthComponent;
    protected float attackPower;
    protected float wanderRadius = 20f;
    public float engageRange = 10f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        buoyancy = GetComponent<BoatBuoyancy>();
        healthComponent = GetComponent<Health>();

        healthComponent.OnDeath += Die;
        AssignStats();
        healthComponent.InitializeHealth(healthComponent.maxHealth);
    }

    void Start()
    {
        wanderTarget = GetNewWanderTarget();
    }

    void FixedUpdate()
    {
        AdjustDrag();

        switch (currentState)
        {
            case State.Wandering:
                Wander();
                LookForPlayer();
                break;
            case State.Chasing:
                ChasePlayer();
                RotateCannonsTowardPlayer();
                if (IsInEngageRange()) currentState = State.Engaging;
                break;
            case State.Engaging:
                EngagePlayer();
                RotateCannonsTowardPlayer();
                TryFireAtPlayer();
                if (!IsInEngageRange()) currentState = State.Chasing;
                break;
        }
        MoveEnemy();
    }

    void Wander()
    {
        if (Vector3.Distance(transform.position, wanderTarget) <= 1f)
            wanderTarget = GetNewWanderTarget();
        
        MoveTowardsTarget(wanderTarget, GetWanderSpeed());
    }

    Vector3 GetNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        return new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
    }

    void LookForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
            currentState = State.Chasing;
    }

    protected void ChasePlayer()
    {
        MoveTowardsTarget(player.position, maxSpeed);
    }

    protected void EngagePlayer()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        MoveTowardsTarget(player.position, playerRb != null ? playerRb.linearVelocity.magnitude : maxSpeed);
    }

    bool IsInEngageRange()
    {
        return Vector3.Distance(transform.position, player.position) <= engageRange;
    }

    void MoveTowardsTarget(Vector3 target, float targetSpeed)
    {
        Vector3 direction = (target - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rudderInput, turnSpeed / rudderEffectiveness);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    void MoveEnemy()
    {
        Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }

    protected void RotateCannonsTowardPlayer()
    {
        for (int i = 0; i < cannonMainBodies.Length; i++)
        {
            Vector3 directionToPlayer = (player.position - cannonMainBodies[i].position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            cannonMainBodies[i].rotation = Quaternion.Slerp(cannonMainBodies[i].rotation, lookRotation, cannonRotationSpeed * Time.deltaTime);

            Vector3 flatDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
            float barrelAngle = Vector3.Angle(flatDirection, directionToPlayer);
            cannonBarrels[i].localRotation = Quaternion.Euler(-barrelAngle, 0, 0);
        }
    }

    protected void TryFireAtPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            foreach (Transform firePoint in firePoints)
                FireBullet(firePoint);
            
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    protected void FireBullet(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null) bulletScript.SetDamage(attackPower);
    }

    public void TakeDamage(float damage)
    {
        healthComponent.TakeDamage(damage);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    void AdjustDrag()
    {
        if (buoyancy == null || buoyancy.floatPoints.Length == 0) return;

        int submergedPoints = 0;
        foreach (Transform point in buoyancy.floatPoints)
        {
            if (point.position.y < buoyancy.targetSurface.transform.position.y)
                submergedPoints++;
        }

        float waterFactor = (float)submergedPoints / buoyancy.floatPoints.Length;
        rb.linearDamping = Mathf.Lerp(dragOver, dragUnder, waterFactor);
    }

    protected abstract void AssignStats();
    protected abstract float GetWanderSpeed();
    protected abstract float GetChaseSpeed();
}
