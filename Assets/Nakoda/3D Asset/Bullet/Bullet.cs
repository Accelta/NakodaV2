using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    public float lifetime = 5f;
    public GameObject impactVFXPrefab;
    public bool addScoreOnHit = false; // <-- Add this flag
    public int scoreBullet;

    private TrailRenderer trailRenderer;
    private ParticleSystem trailParticle;

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailParticle = GetComponentInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        if (trailRenderer != null)
            trailRenderer.Clear();

        if (trailParticle != null)
            trailParticle.Play();

        CancelInvoke();
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
        if (other.TryGetComponent<Health>(out Health targetHealth))
        {
            targetHealth.TakeDamage(damage);
        }

        if (impactVFXPrefab != null)
        {
            GameObject vfxInstance = Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfxInstance, 2f);
        }

        // Only add score if enabled
        if (addScoreOnHit && other.CompareTag("Enemy"))
        {
            ScoreManager.Instance.AddScore(scoreBullet);
        }
        gameObject.SetActive(false);
    }
}