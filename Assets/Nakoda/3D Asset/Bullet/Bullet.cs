using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    public float lifetime = 5f;
    public GameObject impactVFXPrefab; // ✅ Changed to GameObject

    private TrailRenderer trailRenderer;
    private ParticleSystem trailParticle;

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailParticle = GetComponentInChildren<ParticleSystem>();
    }

    public void Initialize(float damageAmount, GameObject impactVFX)
{
    damage = damageAmount;
    impactVFXPrefab = impactVFX;
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
        Debug.Log("Instantiating VFX at " + transform.position);
        GameObject vfxInstance = Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);
        Destroy(vfxInstance, 5f);
    }
    else
    {
        Debug.LogWarning("No impact VFX prefab assigned!");
    }

    gameObject.SetActive(false);
}

}
