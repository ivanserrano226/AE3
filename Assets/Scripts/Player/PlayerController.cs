using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;


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
        _playerInput.actions["Attack"].performed += ctx =>_isShooting = true;
        _playerInput.actions["Attack"].canceled += ctx => _isShooting = false;
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
        // Get the movement input
        Vector2 movementInput = _playerInput.actions["Move"].ReadValue<Vector2>();

        // Convert to world-space movement relative to player rotation
        Vector3 movementDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        // Apply movement (ensuring it's physics-friendly)
        Vector3 movementVelocity = movementDirection.normalized * _movementSpeed;

        _rb.linearVelocity = new Vector3(movementVelocity.x, _rb.linearVelocity.y, movementVelocity.z); // Keeps gravity intact

    }

    void HandleRotation() 
    {
        // Get the mouse movement input
        Vector2 mouseDelta = _playerInput.actions["Look"].ReadValue<Vector2>();

        // Get the current rotation
        Quaternion currentRotation = _rb.rotation;

        Quaternion yawRotation = Quaternion.Euler(0f, mouseDelta.x * _rotationSpeed * Time.deltaTime, 0f);
        _rb.MoveRotation(currentRotation * yawRotation);

        _verticalRotation -= mouseDelta.y * _rotationSpeed * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -89f, 89f); // Prevents looking too far up/down

        Camera.main.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);

    }

    void Shoot() 
    {
        _audioSource.PlayOneShot(GameManager.Instance.ShootSound, 0.5f);

        


        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 2.0f);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null )
            {
                enemy.SpawnDamageParticle(hit);
                enemy.TakeDamage(10);
            } 
        }
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        // Check if there's a collider below the player
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


    void HandleJump() 
    {
        
        if (!IsGrounded()) return;
        _rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
    }

    protected override void Die()
    {
        SceneManager.LoadScene(1);
    }

    // 🎯 **Detectar la recolección de objetos**
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pill"))
        {
            IncreaseHealth(50);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bomb"))
        {
            StartCoroutine(TemporaryDamageBoost(15f));
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Timer"))
        {
            GameManager.Instance.AddTime(20);
            Destroy(other.gameObject);
        }
    }
   //Método para aumentar la vida**
    public void IncreaseHealth(int amount)
    {
        health += amount;
        Debug.Log("Vida aumentada: " + health);
    }

    // Método para aumentar el daño temporalmente**
    public IEnumerator TemporaryDamageBoost(float duration)
    {
        damageMultiplier = 2f;
        Debug.Log("Daño aumentado por " + duration + " segundos.");
        yield return new WaitForSeconds(duration);
        damageMultiplier = 1f;
        Debug.Log("El daño volvió a la normalidad.");
    }
    
}
