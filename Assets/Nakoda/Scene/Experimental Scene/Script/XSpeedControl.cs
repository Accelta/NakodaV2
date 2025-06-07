using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class XSpeedControl : MonoBehaviour
{
    public enum SpeedMode { Reverse, Stop, Half, Full }
    public SpeedMode currentMode = SpeedMode.Stop;
    public float reverseSpeed = -3f;
    public float halfSpeed = 5f;
    public float fullSpeed = 10f;
    public TextMeshProUGUI speedometerText;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float speed = GetCurrentSpeed();
        Vector3 move = transform.right * speed; // Assuming ship front is X-axis
        rb.AddForce(move, ForceMode.Force);
        UpdateUI(speed);
    }

    float GetCurrentSpeed()
    {
        return currentMode switch
        {
            SpeedMode.Reverse => reverseSpeed,
            SpeedMode.Half => halfSpeed,
            SpeedMode.Full => fullSpeed,
            _ => 0f,
        };
    }

    void UpdateUI(float speed)
    {
        if (speedometerText)
        {
            float knots = rb.linearVelocity.magnitude * 1.94384f;
            speedometerText.text = $"{knots:F1} knots";
        }
    }
}