using UnityEngine;

public class WanderingShip : AIShipBase
{
    public float wanderRadius = 50f;
    public float waypointTolerance = 5f;
    public Vector3 targetWaypoint;
    public bool hasWaypoint = false;

    protected override void Start()
    {
        base.Start();
        SetNewWaypoint();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (Vector3.Distance(transform.position, targetWaypoint) < waypointTolerance || !hasWaypoint)
        {
            SetNewWaypoint();
        }
    }

    private void SetNewWaypoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        targetWaypoint = new Vector3(randomCircle.x, transform.position.y, randomCircle.y) + transform.position;
        hasWaypoint = true;
    }

    public override void PerformBehavior()
    {
        Vector3 directionToWaypoint = (targetWaypoint - transform.position).normalized;
        Move(directionToWaypoint);
    }
}