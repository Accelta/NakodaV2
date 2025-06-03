using UnityEngine;

public class Target : MonoBehaviour
{
    public ShootTarget linkedObjective;
private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Target hit!");
            linkedObjective.TargetHit();
            Destroy(gameObject); // Destroy target after hit
        }
    }
}
