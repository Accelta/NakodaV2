using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BoatBuoyancy : MonoBehaviour
{
    public WaterSurface waterSurface; // Referensi ke Water System Unity
    public Transform[] floatPoints; // Titik-titik apung pada perahu
    public float buoyancyForce = 10f;
    public float waterDrag = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        ApplyBuoyancy();
    }

    void ApplyBuoyancy()
    {
        if (waterSurface == null)
        {
            Debug.LogError("WaterSurface belum diassign!");
            return;
        }

        foreach (Transform point in floatPoints)
        {
            float waterHeight = QueryWaterHeight(point.position);
            if (point.position.y < waterHeight)
            {
                float depth = waterHeight - point.position.y;
                Vector3 force = Vector3.up * (depth * buoyancyForce);
                rb.AddForceAtPosition(force, point.position, ForceMode.Acceleration);
                rb.AddForce(-rb.linearVelocity * waterDrag, ForceMode.Acceleration);
            }
        }
    }

    float QueryWaterHeight(Vector3 position)
    {
        return Shader.GetGlobalFloat("_WaterHeight");
    }
}
