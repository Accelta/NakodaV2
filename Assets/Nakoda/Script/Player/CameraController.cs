// using UnityEngine;
// using Unity.Cinemachine;

// public class CameraController : MonoBehaviour
// {
//     public CinemachineCamera shipVirtualCamera;    
//     public CinemachineCamera cannonVirtualCamera;  
//     public CannonRotation cannonRotation;  
//     // public CameraRotation cameraRotation; // Reference to the new CameraRotation script

//     private bool isUsingCannon = false;

//     void Update()
//     {
//         HandleCameraSwitch();
//     }

//     void HandleCameraSwitch()
//     {
//         if (Input.GetKeyDown(KeyCode.F))
//         {
//             isUsingCannon = !isUsingCannon;

//             shipVirtualCamera.gameObject.SetActive(!isUsingCannon);
//             cannonVirtualCamera.gameObject.SetActive(isUsingCannon);

//             cannonRotation.isRotationActive = isUsingCannon;
//             // cameraRotation.enabled = !isUsingCannon; // Enable rotation only when not using cannon

//             Cursor.lockState = isUsingCannon ? CursorLockMode.Locked : CursorLockMode.None;
//             Cursor.visible = !isUsingCannon;
//         }
//     }
// }

using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{

    public CinemachineCamera shipVirtualCamera;
    public CannonGroup[] cannonGroups;

    private CannonGroup activeGroup;
    private bool isUsingCannon = false;
    void Update()
    {
        HandleCameraSwitch();
    }

    void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isUsingCannon)
            {
                CannonGroup candidate = FindGroupInView();
                if (candidate != null)
                {
                    isUsingCannon = true;
                    activeGroup = candidate;

                    shipVirtualCamera.gameObject.SetActive(false);
                    activeGroup.SetActive(true);

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else
            {
                ExitCannonMode();
            }
        }
    }

    void ExitCannonMode()
    {
        isUsingCannon = false;

        if (activeGroup != null)
        {
            activeGroup.SetActive(false);
            foreach (var cannon in activeGroup.cannons)
            {
                cannon.isRotationActive = false; // override internal control
                cannon.autoMode = true;
            }

        }
        

        shipVirtualCamera.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        activeGroup = null;
    }

    CannonGroup FindGroupInView()
    {
        Vector3 camForward = Camera.main.transform.forward;
        CannonGroup bestGroup = null;
        float bestDot = -1f;

        foreach (var group in cannonGroups)
        {
            Vector3 toGroup = (group.transform.position - Camera.main.transform.position).normalized;
            float dot = Vector3.Dot(camForward, toGroup);

            if (dot > bestDot && dot > 0.85f)
            {
                bestDot = dot;
                bestGroup = group;
            }
        }

        return bestGroup;
    }
}
