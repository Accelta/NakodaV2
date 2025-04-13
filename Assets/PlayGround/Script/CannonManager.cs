using UnityEngine;
using System.Collections.Generic;

public class CannonManager : MonoBehaviour
{
    public List<Cannon> cannons; // List of cannon scripts
    public float reloadTime = 1f;  // Time between random shots

    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootRandomCannon();
            nextFireTime = Time.time + reloadTime; // Set the next fire time
        }
    }

    void ShootRandomCannon()
    {
        if (cannons.Count == 0) return; // Safety check

        int randomIndex = Random.Range(0, cannons.Count); // Pick a random cannon
        cannons[randomIndex].Shoot(); // Call the Shoot method of the selected cannon
    }
}
