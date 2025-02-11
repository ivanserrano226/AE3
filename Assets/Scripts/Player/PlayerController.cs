using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : Entity
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _gunTip;
    private float _movementSpeed = 10.0f;
    private float _fireRate = 0.3f;
    private float _lastShotTime = 0;
    private float _rotationSpeed = 15.0f;
    private float _verticalRotation = 0f;
    private bool _isShooting = false;
    public event Action<float> OnHealthChanged;

    void Start()
    {
        GameManager.Instance.OnPlayerSpawned(this);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        _playerInput.actions["Jump"].performed += OnJumpPerformed;
        _playerInput.actions["Attack"].performed += OnAttackPerformed;
        _playerInput.actions["Attack"].canceled += OnAttackCanceled;
        _playerInput.actions["Pause"].performed += OnPausePerformed;
        GameManager.Instance.OnGamePausedEvent += OnGamePaused;
        GameManager.Instance.OnGameOverEvent += OnGameOver;
    }

    void OnDisable()
    {
        _playerInput.actions["Jump"].performed -= OnJumpPerformed;
        _playerInput.actions["Attack"].performed -= OnAttackPerformed;
        _playerInput.actions["Attack"].canceled -= OnAttackCanceled;
        _playerInput.actions["Pause"].performed -= OnPausePerformed;
        GameManager.Instance.OnGamePausedEvent -= OnGamePaused;
        GameManager.Instance.OnGameOverEvent -= OnGameOver;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        HandleJump();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        _isShooting = true;
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        _isShooting = false;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePause();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();

        //Comprobamos que el jugador este manteniendo el botón de disparar
        // y que el tiempo actual menos el tiempo cuando ocurrió el anterior disparo es mayor o igual al ratio de fuego
        if (_isShooting && Time.time - _lastShotTime >= _fireRate)
        {
            _animator.SetTrigger("Shoot");
            Shoot();
            _lastShotTime = Time.time;
        }
        
    }

    void HandleMovement() 
    {
        Vector2 movementInput = _playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 movementDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        Vector3 movementVelocity = movementDirection.normalized * _movementSpeed;

        _rb.linearVelocity = new Vector3(movementVelocity.x, _rb.linearVelocity.y, movementVelocity.z);

    }

    void HandleRotation() 
    {
        Vector2 mouseDelta = _playerInput.actions["Look"].ReadValue<Vector2>();

        Quaternion currentRotation = _rb.rotation;

        Quaternion yawRotation = Quaternion.Euler(0f, mouseDelta.x * _rotationSpeed * Time.deltaTime, 0f);
        _rb.MoveRotation(currentRotation * yawRotation);

        _verticalRotation -= mouseDelta.y * _rotationSpeed * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -89f, 89f);

        Camera.main.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);

    }

    void Shoot() 
    {
        _audioSource.PlayOneShot(GameManager.Instance.ShootSound, 0.3f);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));

        int layerMask = ~LayerMask.GetMask("Walls");

        if(Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {
            if(hit.transform.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.SpawnDamageParticle(hit);
                enemy.TakeDamage(Damage);
            } 
        }
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        return Physics.Raycast(origin, direction, 1.3f);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChanged?.Invoke(Health);
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        OnHealthChanged?.Invoke(Health);
    }

    public void DamagePowerUp()
    {
        StartCoroutine(PowerUpRoutine());
    }

    private IEnumerator PowerUpRoutine()
    {
        Damage = 50;
        yield return new WaitForSeconds(5.0f);
        Damage = 20;
    }

    private void OnGamePaused(bool isPaused)
    {
        if (isPaused) _playerInput.actions["Menu"].performed += OnMenuKeyPressed;
        else _playerInput.actions["Menu"].performed -= OnMenuKeyPressed;
    }

    private void OnGameOver(GameOverStatus status)
    {
        _playerInput.enabled = false;
    }

    private void OnMenuKeyPressed(InputAction.CallbackContext ctx)
    {
        _playerInput.actions["Menu"].performed -= OnMenuKeyPressed;
        GameManager.Instance.ReturnToMenu();
    }

    void HandleJump() 
    {
        if (!IsGrounded()) return;
        _rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
    }

    protected override void Die()
    {
       GameManager.Instance.OnGameOver(GameOverStatus.Defeat);
    }

}
