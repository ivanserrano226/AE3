using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerInput _playerInput;
    private float _movementSpeed = 10.0f;
    private float _rotationSpeed = 15.0f;

    private float _verticalRotation = 0f;
    public int health = 100;
    public float damageMultiplier = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerInput.actions["Jump"].performed += ctx => HandleJump();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
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

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.3f);
    }

    void HandleJump()
    {
        if (!IsGrounded()) return;
        _rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
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

    // ✅ **Método para aumentar la vida**
    public void IncreaseHealth(int amount)
    {
        health += amount;
        Debug.Log("Vida aumentada: " + health);
    }

    // ✅ **Método para aumentar el daño temporalmente**
    public IEnumerator TemporaryDamageBoost(float duration)
    {
        damageMultiplier = 2f;
        Debug.Log("Daño aumentado por " + duration + " segundos.");
        yield return new WaitForSeconds(duration);
        damageMultiplier = 1f;
        Debug.Log("El daño volvió a la normalidad.");
    }
}
