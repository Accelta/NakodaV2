// using UnityEngine;
// using UnityEngine.Rendering.HighDefinition;

// public class BoatBuoyancy : MonoBehaviour
// {
//     public WaterSurface waterSurface; // Referensi ke Water System Unity
//     public Transform[] floatPoints; // Titik-titik apung pada perahu
//     public float buoyancyForce = 10f;
//     public float waterDrag = 2f;
//     WaterSearchParameters search;
//     WaterSearchResult waterSearchResult;

//     private Rigidbody rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.useGravity = true;
//     }

//     void FixedUpdate()
//     {
//         ApplyBuoyancy();
//     }

//     void ApplyBuoyancy()
//     {
//         if (waterSurface == null)
//         {
//             Debug.LogError("WaterSurface belum diassign!");
//             return;
//         }

//         foreach (Transform point in floatPoints)
//         {
//             float waterHeight = QueryWaterHeight(point.position);
//             if (point.position.y < waterHeight)
//             {
//                 float depth = waterHeight - point.position.y;
//                 Vector3 force = Vector3.up * (depth * buoyancyForce);
//                 rb.AddForceAtPosition(force, point.position, ForceMode.Acceleration);
//                 rb.AddForce(-rb.linearVelocity * waterDrag, ForceMode.Acceleration);
//             }
//         }
//     }

//     float QueryWaterHeight(Vector3 position)
//     {
//         return Shader.GetGlobalFloat("_WaterHeight");
//     }
// }
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Rigidbody))]
public class BoatBuoyancy : MonoBehaviour
{
    public Transform[] floatPoints; // Assign float points on the boat
    public WaterSurface targetSurface;
    public float buoyancyStrength = 10f;
    public float dragInWater = 2f;
    public float dragInAir = 0.1f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.useGravity = false; // Gravity is simulated with buoyancy
    }

    void FixedUpdate()
    {
        if (targetSurface == null) return;

        int submergedPoints = 0;
        foreach (Transform point in floatPoints)
        {
            Vector3 waterPosition;
            Vector3 normal;
            Vector3 current;
            
            // Get water height at the point
            FetchWaterSurfaceData(point.position, out waterPosition, out normal, out current);
            float waterHeight = waterPosition.y;

            if (point.position.y < waterHeight)
            {
                submergedPoints++;

                // Calculate buoyant force
                float depth = waterHeight - point.position.y;
                Vector3 buoyancyForce = Vector3.up * depth * buoyancyStrength;
                rb.AddForceAtPosition(buoyancyForce, point.position, ForceMode.Acceleration);
            }
        }

        // Adjust drag based on how many points are submerged
        float waterFactor = (float)submergedPoints / floatPoints.Length;
        rb.linearDamping = Mathf.Lerp(dragInAir, dragInWater, waterFactor);
    }

    private void FetchWaterSurfaceData(Vector3 point, out Vector3 positionWS, out Vector3 normalWS, out Vector3 currentDirectionWS)
    {
        WaterSearchParameters searchParameters = new WaterSearchParameters();
        WaterSearchResult searchResult = new WaterSearchResult();

        searchParameters.startPositionWS = searchResult.candidateLocationWS;
        searchParameters.targetPositionWS = point;
        searchParameters.error = 0.01f;
        searchParameters.maxIterations = 8;
        searchParameters.includeDeformation = true;
        searchParameters.outputNormal = true;

        positionWS = searchResult.candidateLocationWS;
        normalWS = Vector3.up;
        currentDirectionWS = Vector3.zero;

        if (targetSurface.ProjectPointOnWaterSurface(searchParameters, out searchResult))
        {
            positionWS = searchResult.projectedPositionWS;
            normalWS = searchResult.normalWS;
            currentDirectionWS = searchResult.currentDirectionWS;
        }
    }
}