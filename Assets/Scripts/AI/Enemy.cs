using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Entity
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    private bool _isMoving = false;
    private GameObject _player;
    [SerializeField] private GameObject _bloodPrefab;

    //Find the player
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent.SetDestination(_player.transform.position);
    }

    void Update()
    {
        // Only update the destination if needed
        if (!_agent.pathPending && _agent.remainingDistance > _agent.stoppingDistance)
        {
            _agent.SetDestination(_player.transform.position);
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

        _animator.SetBool("isMoving", _isMoving);
    }

    public override void TakeDamage(float damage)
    {
        _animator.SetTrigger("hit");
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        //_animator.SetTrigger("dead");
        GameManager.Instance.OnEnemyKilled();
        Destroy(gameObject);
    }

    public virtual void Attack()
    {
        _animator.SetTrigger("attack");
        _player.GetComponent<Entity>().TakeDamage(Damage);
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