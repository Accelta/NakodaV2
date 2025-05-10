using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class CannonGroup : MonoBehaviour
{
    [Header("Cannon Group Settings")]
    public CinemachineCamera groupCamera;
    public CannonRotation[] cannons;
    public bool isAutoMode = false;
    public float rotationSpeed = 50f;
    public float fireRate = 1f;
    public float aimThreshold = 5f;

    private float nextFireTime = 0f;
    private bool isActive = false;
    public enum cannonGroups {Front, Left, Right}
    public cannonGroups groupname;
    
    void Update()
    {
        if (!isActive) return;

        if (!isAutoMode)
        {
            HandleManualControl();
        }
        else
        {
            HandleAutoTargeting();
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;

        foreach (var cannon in cannons)
        {
            cannon.isRotationActive = false; // override internal control
            cannon.autoMode = false;
        }

        if (groupCamera != null)
            groupCamera.gameObject.SetActive(active);
    }

    void HandleManualControl()
    {
        float horizInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float vertInput = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        foreach (var cannon in cannons)
        {
            cannon.RotateManually(horizInput, vertInput);
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            foreach (var cannon in cannons)
            {
                cannon.FireBullet();
            }
            nextFireTime = Time.time + fireRate;
        }
    }

    void HandleAutoTargeting()
    {
        Transform target = null;
        float closestDist = Mathf.Infinity;

        foreach (var cannon in cannons)
        {
            Transform found = cannon.FindNearestTarget();
            if (found != null)
            {
                float distance = Vector3.Distance(cannon.transform.position, found.position);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    target = found;
                }
            }
        }

        if (target == null) return;

        Vector3 dirToTarget = target.position - cannons[0].firePoint.position;
        float dist = dirToTarget.magnitude;

        Vector3 flatDir = new Vector3(dirToTarget.x, 0, dirToTarget.z);
        Quaternion horizRot = Quaternion.LookRotation(flatDir.normalized);
        float vertAngle = cannons[0].CalculateDesiredVerticalAngle(dirToTarget);

        foreach (var cannon in cannons)
        {
            cannon.SetRotation(horizRot, vertAngle, rotationSpeed);
        }

        // Check aim threshold for firing
        float horizError = Vector3.Angle(cannons[0].mainBody.forward, flatDir.normalized);
        float vertError = Mathf.Abs(cannons[0].GetCurrentVerticalAngle() - vertAngle);

        if (Time.time >= nextFireTime && horizError <= aimThreshold && vertError <= aimThreshold)
        {
            foreach (var cannon in cannons)
            {
                cannon.FireBullet();
            }
            nextFireTime = Time.time + fireRate;
        }
    }
}
