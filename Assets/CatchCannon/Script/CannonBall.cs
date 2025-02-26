using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public int scoreValue = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat")) // Pastikan perahu memiliki tag "Boat"
        {
            ScoreManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
