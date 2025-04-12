using UnityEngine;

public class SpinnerTst : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Rotation speed in degrees per second for each axis (X, Y, Z)")]
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
