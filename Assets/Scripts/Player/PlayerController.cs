using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerInput _playerInput;
    private float _movementSpeed = 10.0f;
    private float _rotationSpeed = 15.0f;

    private float _verticalRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _playerInput.actions["Jump"].performed += ctx => HandleJump();

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement() {
        // Get the movement input
        Vector2 movementInput = _playerInput.actions["Move"].ReadValue<Vector2>();

        // Convert to world-space movement relative to player rotation
        Vector3 movementDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        // Apply movement (ensuring it's physics-friendly)
        Vector3 movementVelocity = movementDirection.normalized * _movementSpeed;

        _rb.linearVelocity = new Vector3(movementVelocity.x, _rb.linearVelocity.y, movementVelocity.z); // Keeps gravity intact

    }

    void HandleRotation() {


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

    bool IsGrounded()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        // Check if there's a collider below the player
        return Physics.Raycast(origin, direction, 1.3f);
    }

    void HandleJump() {
        
        if (!IsGrounded()) return;
        _rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
    }
}
