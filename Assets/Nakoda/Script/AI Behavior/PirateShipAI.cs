using UnityEngine;

public class PirateShipAI : AIShipBase
{
    [Header("Enemy Properties")]
    public float detectionRadius = 40f;
    public float loseTargetDelay = 5f;

    private Transform player;
    private float lostTimer;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void PerformBehavior()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < detectionRadius)
        {
            currentTarget = player;
            lostTimer = 0f;
        }
        else if (currentTarget == player)
        {
            lostTimer += Time.fixedDeltaTime;
            if (lostTimer > loseTargetDelay)
            {
                ChooseNextWaypoint();
            }
        }
    }
}
