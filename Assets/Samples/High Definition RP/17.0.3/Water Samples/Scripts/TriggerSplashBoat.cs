using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

[RequireComponent(typeof(BoatBuoyancy))]
public class TriggerSplashBoat : MonoBehaviour
{
    public GameObject prefab;
    public Transform splashPoint; // Splash effect will be triggered at this point
    private BoatBuoyancy buoyancyComponent;
    private Rigidbody rigidbodyComponent;
    private bool wasInWater = false;

    void OnEnable()
    {
        buoyancyComponent = GetComponent<BoatBuoyancy>();
        rigidbodyComponent = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        bool isInWater = buoyancyComponent.IsInWater();

        if (isInWater && !wasInWater) // Detecting transition from air to water
        {
            if (PoolManager.Instances[PoolManager.InstanceType.Splash] != null)
            {
                GameObject splashObject = PoolManager.Instances[PoolManager.InstanceType.Splash].getNextAvailable();
                
                if (splashObject != null)
                {
                    splashObject.transform.position = splashPoint.position;

                    VisualEffect splashVFX = splashObject.GetComponent<VisualEffect>();
                    splashVFX.SetFloat("Splash Radius", 1.0f); // Adjust as needed
                    splashVFX.SetVector3("Velocity", rigidbodyComponent.linearVelocity);
                    
                    splashObject.SetActive(true);
                    splashVFX.Play();
                }
            }
        }
        
        wasInWater = isInWater;
    }
}
