using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using Game.Items;

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
    public int health = 100;
    public float damageMultiplier = 1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerInput.actions["Jump"].performed += ctx => HandleJump();
        _playerInput.actions["Attack"].performed += ctx => _isShooting = true;
        _playerInput.actions["Attack"].canceled += ctx => _isShooting = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
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
        Quaternion yawRotation = Quaternion.Euler(0f, mouseDelta.x * _rotationSpeed * Time.deltaTime, 0f);
        _rb.MoveRotation(_rb.rotation * yawRotation);
        _verticalRotation -= mouseDelta.y * _rotationSpeed * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -89f, 89f);
        Camera.main.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }

    void Shoot()
    {
        _audioSource.PlayOneShot(GameManager.Instance.ShootSound, 0.5f);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 2.0f);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SpawnDamageParticle(hit);
                enemy.TakeDamage(10);
            }
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.3f);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChanged?.Invoke(health);
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        OnHealthChanged?.Invoke(health);
    }

    void HandleJump()
    {
        if (!IsGrounded()) return;
        _rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
    }

    protected override void Die()
    {
        SceneManager.LoadScene(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            item.Use(this);
            Destroy(other.gameObject);
        }
    }

    public void IncreaseHealth(int amount)
    {
        health += amount;
        if (health > 100) health = 100;
        Debug.Log("Vida aumentada: " + health);
        OnHealthChanged?.Invoke(health);
    }

    public IEnumerator TemporaryDamageBoost(float duration)
    {
        damageMultiplier = 2f;
        Debug.Log("Daño aumentado por " + duration + " segundos.");
        yield return new WaitForSeconds(duration);
        damageMultiplier = 1f;
        Debug.Log("El daño volvió a la normalidad.");
    }
}
