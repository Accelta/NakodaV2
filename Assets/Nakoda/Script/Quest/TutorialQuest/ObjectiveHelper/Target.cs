using UnityEngine;

// filepath: c:\Users\Axel\Documents\UnityProject\NakodaV2\Assets\Nakoda\Script\Quest\TutorialQuest\Target.cs
public class Target : MonoBehaviour
{
    public ShootTarget linkedObjective;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            #if UNITY_EDITOR
            Debug.Log("Target hit!");
            #endif
            if (linkedObjective != null)
            {
                linkedObjective.TargetHit(gameObject); // Pass this target GameObject
            }
            Destroy(gameObject); // Destroy target after hit
        }
    }
}