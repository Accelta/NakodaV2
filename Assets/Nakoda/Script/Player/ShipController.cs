// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(BoatBuoyancy))]
// public class ShipController : MonoBehaviour
// {
//     public enum SpeedState { Stopped, Slow, Normal, Fast }
//     public SpeedState currentSpeedState = SpeedState.Stopped;

//     public float slowSpeed = 5f;
//     public float normalSpeed = 15f;
//     public float fastSpeed = 25f;
//     public float acceleration = 5f;
//     public float deceleration = 5f;
//     public float turnSpeed = 2f;
//     public float rudderEffectiveness = 1f;
//     public float rudderTurnAngle = 30f;

//     private float targetSpeed = 0f;
//     private float currentSpeed = 0f;
//     private float rudderInput = 0f;
//     public float rudderSmoothSpeed = 5f;

//     private Rigidbody rb;
//     private BoatBuoyancy buoyancy;
//     private float rotation;
//     public Transform rudder;
//     public float dragInWater = 2f;
//     public float dragInAir = 0.1f;
//     public GameObject turnHelper;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         buoyancy = GetComponent<BoatBuoyancy>();
//     }

//     void FixedUpdate()
//     {
//         AdjustDrag();
//         HandleSpeed();
//         HandleTurning();
//         ApplyForces();
//         RotateRudder();
//         PreventUpsideDown();
//     }

//     void AdjustDrag()
//     {
//         if (buoyancy == null || buoyancy.floatPoints.Length == 0) return;

//         // Calculate how many float points are submerged
//         int submergedPoints = 0;
//         foreach (Transform point in buoyancy.floatPoints)
//         {
//             if (point.position.y < buoyancy.targetSurface.transform.position.y)
//                 submergedPoints++;
//         }

//         float waterFactor = (float)submergedPoints / buoyancy.floatPoints.Length;
//         rb.linearDamping = Mathf.Lerp(dragInAir, dragInWater, waterFactor);
//     }

//     void PreventUpsideDown()
//     {
//         rotation = Vector3.Angle(Vector3.up, transform.up);
//         if (rotation > 70f)
//         {
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);
//         }
//     }

//     void HandleSpeed()
//     {
//         if (Input.GetKeyDown(KeyCode.W))
//         {
//             if (currentSpeedState == SpeedState.Stopped)
//                 currentSpeedState = SpeedState.Slow;
//             else if (currentSpeedState == SpeedState.Slow)
//                 currentSpeedState = SpeedState.Normal;
//             else if (currentSpeedState == SpeedState.Normal)
//                 currentSpeedState = SpeedState.Fast;
//         }
//         else if (Input.GetKeyDown(KeyCode.S))
//         {
//             if (currentSpeedState == SpeedState.Fast)
//                 currentSpeedState = SpeedState.Normal;
//             else if (currentSpeedState == SpeedState.Normal)
//                 currentSpeedState = SpeedState.Slow;
//             else if (currentSpeedState == SpeedState.Slow)
//                 currentSpeedState = SpeedState.Stopped;
//         }

//         switch (currentSpeedState)
//         {
//             case SpeedState.Stopped: targetSpeed = 0f; break;
//             case SpeedState.Slow: targetSpeed = slowSpeed; break;
//             case SpeedState.Normal: targetSpeed = normalSpeed; break;
//             case SpeedState.Fast: targetSpeed = fastSpeed; break;
//         }

//         currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);
//     }

//     void HandleTurning()
//     {
//         rudderInput = currentSpeed > 0 ? Input.GetAxis("Horizontal") : 0f;
//     }

//     void ApplyForces()
//     {
//         if (turnHelper != null)
//         {
//             Vector3 forwardForce = turnHelper.transform.forward * currentSpeed * rb.mass;
//             rb.AddForce(forwardForce);

//             float turnTorque = rudderInput * turnSpeed * rudderEffectiveness * rb.mass;
//             rb.AddTorque(Vector3.up * turnTorque);
//         }
//     }

//     void RotateRudder()
//     {
//         if (rudder != null)
//         {
//             float targetRudderRotation = -rudderInput * rudderTurnAngle;
//             float smoothRotation = Mathf.LerpAngle(rudder.localRotation.eulerAngles.y, targetRudderRotation, Time.deltaTime * rudderSmoothSpeed);
//             rudder.localRotation = Quaternion.Euler(0f, smoothRotation, 0f);
//         }
//     }
// }
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoatBuoyancy))]
public class ShipController : MonoBehaviour
{
    public enum SpeedState { Stopped, Slow, Normal, Fast }
    public SpeedState currentSpeedState = SpeedState.Stopped;

    public float slowSpeed = 5f;
    public float normalSpeed = 15f;
    public float fastSpeed = 25f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float turnSpeed = 2f;
    public float rudderEffectiveness;
    public float rudderTurnAngle = 30f;

    private float targetSpeed = 0f;
    private float currentSpeed = 0f;
    private float rudderInput = 0f;
    public float rudderSmoothSpeed = 5f;
    private float baseRudderEffectiveness;

    private Rigidbody rb;
    private BoatBuoyancy buoyancy;
    private float rotation;
    public Transform rudder;
    public float dragInWater = 2f;
    public float dragInAir = 0.1f;
    public GameObject turnHelper;
    private new ConstantForce constantForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        buoyancy = GetComponent<BoatBuoyancy>();
        baseRudderEffectiveness = rudderEffectiveness;

    constantForce = gameObject.AddComponent<ConstantForce>();
    constantForce.force = Vector3.zero; // Initially no extra force
    }

    void FixedUpdate()
    {
        AdjustDrag();
        HandleSpeed();
        HandleTurning();
        ApplyForces();
        RotateRudder();
        PreventUpsideDown();
        ApplyWeightEffect();
         AdjustSpeedInAir();
    }

    // void AdjustDrag()
    // {
    //     if (buoyancy == null || buoyancy.floatPoints.Length == 0) return;

    //     int submergedPoints = 0;
    //     foreach (Transform point in buoyancy.floatPoints)
    //     {
    //         if (point.position.y < buoyancy.targetSurface.transform.position.y)
    //             submergedPoints++;
    //     }

    //     float waterFactor = (float)submergedPoints / buoyancy.floatPoints.Length;
    //     rb.linearDamping = Mathf.Lerp(dragInAir, dragInWater, waterFactor);
    // }
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

    // Increase drag when moving fast to prevent "flying"
    float dynamicDrag = dragInWater;
    if (currentSpeedState == SpeedState.Fast)
    {
        dynamicDrag *= 3f; // Increase drag when at fast speed
    }

    rb.linearDamping = Mathf.Lerp(dragInAir, dynamicDrag, waterFactor);
}
    void PreventUpsideDown()
    {
        rotation = Vector3.Angle(Vector3.up, transform.up);
        if (rotation > 70f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);
        }
    }

    void HandleSpeed()
    {
        SpeedState previousSpeedState = currentSpeedState;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentSpeedState == SpeedState.Stopped)
                currentSpeedState = SpeedState.Slow;
            else if (currentSpeedState == SpeedState.Slow)
                currentSpeedState = SpeedState.Normal;
            else if (currentSpeedState == SpeedState.Normal)
                currentSpeedState = SpeedState.Fast;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentSpeedState == SpeedState.Fast)
                currentSpeedState = SpeedState.Normal;
            else if (currentSpeedState == SpeedState.Normal)
                currentSpeedState = SpeedState.Slow;
            else if (currentSpeedState == SpeedState.Slow)
                currentSpeedState = SpeedState.Stopped;
        }

        if (currentSpeedState != previousSpeedState)
        {
            AdjustRudderEffectiveness(previousSpeedState, currentSpeedState);
        }

        switch (currentSpeedState)
        {
            case SpeedState.Stopped: targetSpeed = 0f; break;
            case SpeedState.Slow: targetSpeed = slowSpeed; break;
            case SpeedState.Normal: targetSpeed = normalSpeed; break;
            case SpeedState.Fast: targetSpeed = fastSpeed; break;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);
    }

    void AdjustRudderEffectiveness(SpeedState oldState, SpeedState newState)
    {
        if (oldState < newState)
        {
            rudderEffectiveness += 0.1f;
        }
        else if (oldState > newState)
        {
            rudderEffectiveness -= 0.1f;
        }
    }

    void HandleTurning()
    {
        rudderInput = currentSpeed > 0 ? Input.GetAxis("Horizontal") : 0f;
    }

    // void ApplyForces()
    // {
    //     if (turnHelper != null)
    //     {
    //         Vector3 forwardForce = turnHelper.transform.forward * currentSpeed * rb.mass;
    //         rb.AddForce(forwardForce);

    //         float turnTorque = rudderInput * turnSpeed * rudderEffectiveness * rb.mass;
    //         rb.AddTorque(Vector3.up * turnTorque);
    //     }
    // }
void ApplyForces()
{
    if (turnHelper != null)
    {
        Vector3 forwardForce = turnHelper.transform.forward * currentSpeed * rb.mass;
        rb.AddForce(forwardForce);

        float turnTorque = rudderInput * turnSpeed * rudderEffectiveness * rb.mass;
        rb.AddTorque(Vector3.up * turnTorque);
    }

    // Add downward force to keep the ship stable in water
    if (buoyancy != null && buoyancy.IsInWater()) 
    {
        rb.AddForce(Vector3.down * rb.mass * 2f); // Increase downward force
    }

    // Limit upward force if it's too high
    if (rb.linearVelocity.y > 2f) 
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 2f, rb.linearVelocity.z);
    }
}
    void RotateRudder()
    {
        if (rudder != null)
        {
            float targetRudderRotation = -rudderInput * rudderTurnAngle;
            float smoothRotation = Mathf.LerpAngle(rudder.localRotation.eulerAngles.y, targetRudderRotation, Time.deltaTime * rudderSmoothSpeed);
            rudder.localRotation = Quaternion.Euler(0f, smoothRotation, 0f);
        }
    }
    void ApplyWeightEffect()
{
    if (buoyancy != null && buoyancy.IsInWater())
    {
        // If in water, reset extra downward force
        constantForce.force = Vector3.zero;
    }
    else
    {
        // If in air, apply strong downward force to bring the ship down
        constantForce.force = Vector3.down * rb.mass * 3f;
    }
}

void AdjustSpeedInAir()
{
    if (buoyancy != null && !buoyancy.IsInWater()) // If the ship is airborne
    {
        // Reduce speed gradually to simulate air resistance
        currentSpeed = Mathf.Lerp(currentSpeed, 0f, 2f * Time.deltaTime);

        // Increase drag to slow down further
        rb.linearDamping = 10f; 
    }
    else
    {
        // Reset drag when back in water
        rb.linearDamping = 1f;
    }
}
}
