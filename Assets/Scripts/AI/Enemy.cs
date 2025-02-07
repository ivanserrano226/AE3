using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _bloodPrefab;
    public void Damage(RaycastHit hit) 
    {
        // Instantiate the particle system at the hit point
        GameObject particle = Instantiate(_bloodPrefab, hit.point, Quaternion.identity);

        // Fix the rotation so the particle system aligns with the surface normal
        particle.transform.rotation = Quaternion.LookRotation(hit.normal);

        Destroy(particle, 0.3f);
    }
}
