using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;
    public float Health { get; }
    public float Damage { get; }
    public float MaxHealth { get; }
    protected abstract void Die();
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        _health += amount;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public void HealMax(float maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
    }
}