using UnityEngine;

public class WanderingShip : AIShipBase
{
    public float wanderRadius = 50f;
    public float waypointTolerance = 5f;
    private Vector3 targetWaypoint;

    protected override void Start()
    {
        base.Start();
        SetNewWaypoint();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (Vector3.Distance(transform.position, targetWaypoint) < waypointTolerance)
        {
            SetNewWaypoint();
        }
        else
        {
            PerformBehavior();
        }
    }

    private void SetNewWaypoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        targetWaypoint = new Vector3(randomCircle.x + transform.position.x, transform.position.y, randomCircle.y + transform.position.z);
    }

    public override void PerformBehavior()
    {
        Vector3 directionToWaypoint = (targetWaypoint - transform.position).normalized;
        if (directionToWaypoint != Vector3.zero)
        {
            Move(directionToWaypoint);
        }
    }
}