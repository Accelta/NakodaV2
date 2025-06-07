// using UnityEngine;
// using UnityEngine.Rendering.HighDefinition;

// [RequireComponent(typeof(Rigidbody))]
// public class XBuoyancy : MonoBehaviour
// {
//     [Header("Buoyancy Settings")]
//     public Transform[] floatingPoints;
//     public float baseBuoyancyStrength = 10f;
//     public float dragInWater = 2f;
//     public float dragInAir = 0.1f;

//     [Header("Water Surface Auto-Detection")]
//     public WaterSurface waterSurface;

//     private Rigidbody rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();

//         if (waterSurface == null)
//         {
//             waterSurface = FindAnyObjectByType<WaterSurface>();
//             if (waterSurface == null)
//                 Debug.LogError("No WaterSurface found in the scene.");
//             else
//                 Debug.Log("WaterSurface auto-assigned to: " + waterSurface.name);
//         }
//     }

//     void FixedUpdate()
//     {
//         if (waterSurface == null || floatingPoints.Length == 0) return;

//         int submergedCount = 0;
//         float adjustedBuoyancy = baseBuoyancyStrength / Mathf.Max(rb.mass, 1f);

//         foreach (var point in floatingPoints)
//         {
//             Vector3 waterPosition, waterNormal, currentDirection;
//             FetchWaterSurfaceData(point.position, out waterPosition, out waterNormal, out currentDirection);

//             float waterHeight = waterPosition.y;
//             float pointHeight = point.position.y;

//             if (pointHeight < waterHeight)
//             {
//                 submergedCount++;

//                 float depth = waterHeight - pointHeight;
//                 Vector3 force = Vector3.up * depth * adjustedBuoyancy;
//                 rb.AddForceAtPosition(force, point.position, ForceMode.Acceleration);
//             }
//         }

//         float waterContactRatio = (float)submergedCount / floatingPoints.Length;
//         rb.linearDamping = Mathf.Lerp(dragInAir, dragInWater, waterContactRatio);
//         rb.angularDamping = Mathf.Lerp(0f, dragInWater, waterContactRatio);
//     }

//     private void FetchWaterSurfaceData(Vector3 point, out Vector3 positionWS, out Vector3 normalWS, out Vector3 currentDirectionWS)
//     {
//         WaterSearchParameters searchParams = new WaterSearchParameters
//         {
//             startPositionWS = point + Vector3.up * 5f,
//             targetPositionWS = point,
//             error = 0.01f,
//             maxIterations = 8,
//             includeDeformation = true,
//             outputNormal = true
//         };

//         WaterSearchResult result = new WaterSearchResult();
//         positionWS = point;
//         normalWS = Vector3.up;
//         currentDirectionWS = Vector3.zero;

//         if (waterSurface.ProjectPointOnWaterSurface(searchParams, out result))
//         {
//             positionWS = result.projectedPositionWS;
//             normalWS = result.normalWS;
//             currentDirectionWS = result.currentDirectionWS;
//         }
//     }

//     public bool IsPartiallySubmerged()
//     {
//         foreach (var point in floatingPoints)
//         {
//             Vector3 waterPos, norm, curr;
//             FetchWaterSurfaceData(point.position, out waterPos, out norm, out curr);
//             if (point.position.y < waterPos.y)
//                 return true;
//         }
//         return false;
//     }
// }
