using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;
    public float Health { get => _health; }
    public float Damage { get => _damage; }
    public float MaxHealth { get => _maxHealth; }
    protected abstract void Die();
    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Entity taking damage");
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