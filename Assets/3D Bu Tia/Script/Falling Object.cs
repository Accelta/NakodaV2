using UnityEngine;

public class FallingObjectTst : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Caught!");
            // PointManagerTst.instance.AddScore(1); // Updated name here
            ScoreManager.Instance.AddScore(10);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Missed!");
            Destroy(gameObject);
        }
    }
}
