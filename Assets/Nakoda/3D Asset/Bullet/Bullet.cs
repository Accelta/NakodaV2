// using UnityEngine;

// public class Bullet : MonoBehaviour
// {
//     private float damage;
//     public float lifetime = 5f; // Lifetime of the bullet in seconds

// void OnEnable()
//     {
//         Invoke("DeactivateBullet", lifetime);
//     }
//      void DeactivateBullet()
//     {
//         BulletPool.Instance.ReturnBullet(gameObject);
//     }

//     // void Start()
//     // {
//     //     // Schedule the bullet to be destroyed after its lifetime
//     //     Destroy(gameObject, lifetime);
//     // }

//     // Method to set damage from the enemy or player
//     public void SetDamage(float damageAmount)
//     {
//         damage = damageAmount;
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         // Check if the collided object is an enemy
//         if (other.TryGetComponent<EnemyBase>(out EnemyBase enemy))
//         {
//             // Apply damage to the enemy
//             enemy.TakeDamage(damage);
//         }

//         // Check if the collided object is the player
//         if (other.TryGetComponent<Health>(out Health playerHealth))
//         {
//             // Apply damage to the player
//             playerHealth.TakeDamage(damage);
//         }

//         // Destroy the bullet after hitting something
//         Destroy(gameObject);
//     }
// }
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    public float lifetime = 5f; // Lama hidup peluru
    public GameObject impactEffectPrefab; // Efek impact saat terkena sesuatu
    private TrailRenderer trailRenderer; // Trail dari bullet
    private ParticleSystem trailParticle; // Partikel asap jika menggunakan Particle System

    void Awake()
    {
        // Ambil komponen Trail Renderer jika ada
        trailRenderer = GetComponent<TrailRenderer>();
        // Ambil Particle System jika ada
        trailParticle = GetComponentInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        // Pastikan peluru kembali ke posisi awal sebelum digunakan
        if (trailRenderer != null)
            trailRenderer.Clear(); // Hapus jejak lama dari Trail Renderer

        if (trailParticle != null)
            trailParticle.Play(); // Nyalakan asap jika menggunakan Particle System

        // Atur waktu hidup peluru sebelum kembali ke pool
        Invoke(nameof(DeactivateBullet), lifetime);
    }

    void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    void OnTriggerEnter(Collider other)
    {
        // Jika terkena musuh
        if (other.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            enemy.TakeDamage(damage);
        }

        // Jika terkena player
        if (other.TryGetComponent<Health>(out Health playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }

        // Tampilkan efek impact
        if (impactEffectPrefab != null)
        {
            GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(impactEffect, 1f); // Hancurkan efek impact setelah 1 detik
        }

        // Matikan peluru setelah terkena sesuatu
        gameObject.SetActive(false);
    }
}
