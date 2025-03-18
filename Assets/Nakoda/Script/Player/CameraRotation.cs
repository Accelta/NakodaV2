using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform shipTransform; // The ship (center of rotation)
    public Transform cameraTransform; // The camera to rotate
    public float rotationSpeed = 100f; // How fast the camera rotates
    public float transitionSpeed = 2f; // Speed to return to default

    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private bool isRotatingCamera = false;
    private bool isReturningToPosition = false;

    void Start()
    {
        // Store the initial position and rotation of the camera
        originalPosition = cameraTransform.localPosition;
        originalRotation = cameraTransform.localRotation;
    }

    void Update()
    {
        HandleCameraRotation();
        HandleCameraTransitionBack();
    }

    void HandleCameraRotation()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
        {
            isRotatingCamera = true;
            isReturningToPosition = false;
        }
        if (Input.GetMouseButtonUp(1)) // Right mouse button released
        {
            isRotatingCamera = false;
            isReturningToPosition = true;
        }

        if (isRotatingCamera)
        {
            RotateAroundShip();
        }
    }

    void RotateAroundShip()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        
        // Rotate the camera around the ship on the Y-axis
        cameraTransform.RotateAround(shipTransform.position, Vector3.up, mouseX);
    }

    void HandleCameraTransitionBack()
    {
        if (isReturningToPosition)
        {
            // Smoothly move the camera back to the original position
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition, 
                originalPosition, 
                transitionSpeed * Time.deltaTime);

            // Smoothly rotate the camera back to the original rotation
            cameraTransform.localRotation = Quaternion.Slerp(
                cameraTransform.localRotation, 
                originalRotation, 
                transitionSpeed * Time.deltaTime);

            // Stop transitioning if it's close to the original position and rotation
            if (Vector3.Distance(cameraTransform.localPosition, originalPosition) < 0.01f &&
                Quaternion.Angle(cameraTransform.localRotation, originalRotation) < 0.01f)
            {
                isReturningToPosition = false;
            }
        }
    }
}
