
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoatBuoyancy))]
public class ShipController : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float turnSpeed = 2f;
    
    private float currentSpeed = 0f;
    private float rudderInput = 0f;
    private Rigidbody rb;
    private BoatBuoyancy buoyancy;
    public Transform rudder;
    public float rudderTurnAngle = 30f;
    public float rudderSmoothSpeed = 5f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        buoyancy = GetComponent<BoatBuoyancy>();
    }

    void FixedUpdate()
    {
        HandleSpeed();
        HandleTurning();
        ApplyForces();
        RotateRudder();
        PreventUpsideDown();
    }

    void HandleSpeed()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime * 0.5f);
        }
    }

    void HandleTurning()
    {
        // Ship can only turn when moving
        if (currentSpeed > 0)
        {
            rudderInput = Input.GetAxis("Horizontal");
        }
        else
        {
            rudderInput = 0f;
        }
    }

    void ApplyForces()
    {
        Vector3 forwardForce = transform.forward * currentSpeed * rb.mass;
        rb.AddForce(forwardForce);

        // Apply turning force only if the ship is moving
        if (currentSpeed > 0)
        {
            float turnTorque = rudderInput * turnSpeed * rb.mass;
            rb.AddTorque(Vector3.up * turnTorque);
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

    void PreventUpsideDown()
    {
        float rotation = Vector3.Angle(Vector3.up, transform.up);
        if (rotation > 70f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);
        }
    }
}

