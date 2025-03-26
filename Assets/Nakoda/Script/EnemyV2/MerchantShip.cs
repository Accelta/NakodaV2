using UnityEngine;

public class MerchantShip : AIShipBase
{
    public Transform[] tradeRoutes;
    private int currentRouteIndex = 0;

    protected override void Start()
    {
        base.Start();
        if (tradeRoutes.Length > 0)
            target = tradeRoutes[currentRouteIndex];
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (target != null && Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            NextTradePoint();
        }
    }

    private void NextTradePoint()
    {
        currentRouteIndex = (currentRouteIndex + 1) % tradeRoutes.Length;
        target = tradeRoutes[currentRouteIndex];
    }

    public override void PerformBehavior()
    {
        // Additional merchant behavior like docking at harbors can be added here.
    }
}
