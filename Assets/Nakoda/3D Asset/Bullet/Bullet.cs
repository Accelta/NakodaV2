using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    public float lifetime = 5f;
    public GameObject impactEffectPrefab;

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

        if (impactEffectPrefab != null)
        {
            GameObject effect = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }

        gameObject.SetActive(false);
    }
}
