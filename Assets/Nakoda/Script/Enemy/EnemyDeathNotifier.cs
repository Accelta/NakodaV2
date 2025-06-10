using UnityEngine;
using System;

public class EnemyDeathNotifier : MonoBehaviour
{
    public event Action<GameObject> OnEnemyKilled;

    public void Die()
    {
        OnEnemyKilled?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
