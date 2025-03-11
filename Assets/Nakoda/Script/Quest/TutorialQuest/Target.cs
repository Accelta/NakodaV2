using UnityEngine;

public class Target : MonoBehaviour
{
    public ShootTarget linkedObjective;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !linkedObjective.isCompleted)
        {
            linkedObjective.RegisterHit();
        }
    }
}
