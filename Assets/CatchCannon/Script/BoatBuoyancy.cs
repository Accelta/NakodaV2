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
public bool IsInWater()
{
    foreach (Transform point in floatPoints)
    {
        Vector3 waterPosition;
        Vector3 normal;
        Vector3 current;

        FetchWaterSurfaceData(point.position, out waterPosition, out normal, out current);
        if (point.position.y < waterPosition.y)
        {
            return true; // If at least one float point is submerged, the boat is in water
        }
    }
    return false;
}
}