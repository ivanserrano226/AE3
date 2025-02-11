using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;
    public float Health { get => _health; protected set => _health = value; }
    public float Damage { get => _damage; protected set => _damage = value; }
    public float MaxHealth { get => _maxHealth; protected set => _maxHealth = value; }
    protected abstract void Die();
    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
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