using UnityEngine;

public abstract class Enemy : Entity
{
    [SerializeField] private GameObject _bloodPrefab;

    protected override void Die()
    {
        Destroy(gameObject);
    }

    public virtual void SpawnDamageParticle(RaycastHit hit) 
    {
        // Instantiate the particle system at the hit point
        GameObject particle = Instantiate(_bloodPrefab, hit.point, Quaternion.identity);

        // Fix the rotation so the particle system aligns with the surface normal
        particle.transform.rotation = Quaternion.LookRotation(hit.normal);

        //Scale the prefab depending on the hit object's scale
        particle.transform.localScale = hit.transform.localScale;

        Destroy(particle, 0.3f);
    }
}