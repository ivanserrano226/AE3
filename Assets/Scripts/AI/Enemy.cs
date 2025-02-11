using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Spawned, Chasing, Attacking, Dead }

public class Enemy : Entity
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bloodPrefab;
    [SerializeField] private AudioClip _enemyAttackSound;
    [SerializeField] private AudioClip _enemyDeathSound;
    private EnemyState _state;
    private GameObject _player;
    private bool isAttacking = false;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent.SetDestination(_player.transform.position);
        _state = EnemyState.Spawned;
    }

    void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (_state)
        {
            case EnemyState.Spawned:
                ChangeState(EnemyState.Chasing);
                break;

            case EnemyState.Chasing:
                _animator.SetBool("isMoving", true);
                _agent.SetDestination(_player.transform.position);

                if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    ChangeState(EnemyState.Attacking);
                }
                break;

            case EnemyState.Attacking:
                _animator.SetBool("isMoving", false);
                if (!isAttacking) StartCoroutine(AttackCoroutine());
                break;

            case EnemyState.Dead: 
                break;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        
        transform.LookAt(_player.transform);
        _audioSource.PlayOneShot(_enemyAttackSound);
        _animator.SetTrigger("attack");
        
        yield return new WaitForSeconds(0.5f); // Wait for animation to reach damage frame
        _player.GetComponent<Entity>().TakeDamage(Damage);

        yield return new WaitForSeconds(1f); // Attack cooldown
        isAttacking = false;
        
        ChangeState(EnemyState.Chasing); // Resume chasing after attack
    }

    private void ChangeState(EnemyState newState)
    {
        _state = newState;
    }

    public override void TakeDamage(float damage)
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + 50.0f * Time.deltaTime * -transform.forward);
        _animator.SetTrigger("hit");
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        ChangeState(EnemyState.Dead);
        GetComponent<Collider>().enabled = false;
        GameManager.Instance.OnEnemyKilled();
        _audioSource.PlayOneShot(_enemyDeathSound);
        _animator.SetTrigger("dead");
        yield return new WaitForSeconds(3f);

        Destroy(gameObject, 10f);
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