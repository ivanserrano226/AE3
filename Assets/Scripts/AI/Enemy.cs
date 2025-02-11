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
    private Coroutine attackCoroutine;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent.SetDestination(_player.transform.position);
        _state = EnemyState.Spawned;
    }

    void Update()
    {
        if (_state == EnemyState.Dead) return;
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
                if (!isAttacking) 
                {
                    attackCoroutine = StartCoroutine(AttackCoroutine());
                }
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
        
        yield return new WaitForSeconds(0.5f);

        if (_state != EnemyState.Dead)
        {
            _player.GetComponent<Entity>().TakeDamage(Damage);
        }

        yield return new WaitForSeconds(1f);
        isAttacking = false;
        
        if (_state != EnemyState.Dead)
        {
            ChangeState(EnemyState.Chasing);
        }
    }

    private void ChangeState(EnemyState newState)
    {
        _state = newState;
    }

    protected override void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        
        ChangeState(EnemyState.Dead);
        _agent.isStopped = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GameManager.Instance.OnEnemyKilled();
        _audioSource.PlayOneShot(_enemyDeathSound);
        _animator.SetTrigger("dead");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public virtual void SpawnDamageParticle(RaycastHit hit) 
    {
        GameObject particle = Instantiate(_bloodPrefab, hit.point, Quaternion.identity);
        particle.transform.rotation = Quaternion.LookRotation(hit.normal);
        Destroy(particle, 0.3f);
    }
}