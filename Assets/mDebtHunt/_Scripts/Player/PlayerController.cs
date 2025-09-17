using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private readonly InputSystem _inputSystem;
    
    [Header("Movement")]
    [Space(10)]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    
    [Header("Looking")]
    [Space(10)]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _maxLookX = 80f;
    [SerializeField] private float _minLookX = -80f;
    
    private CharacterController _controller;
    
    private Vector3 _velocity;
    private bool _isGrounded;
    private float _xRotation;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _inputSystem.JumpChanged += OnJump;
    }

    private void OnDisable()
    {
        _inputSystem.JumpChanged -= OnJump;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        HandleLook();
    }
    
    private void HandleMovement()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        Vector3 move = transform.right * _inputSystem.MoveDirection.x + transform.forward * _inputSystem.MoveDirection.y;
        _controller.Move(move * (_moveSpeed * Time.deltaTime));
        
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
    
    private void HandleLook()
    {
        float mouseX = _inputSystem.LookDirection.x * _mouseSensitivity / 10;
        float mouseY = _inputSystem.LookDirection.y * _mouseSensitivity / 10;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minLookX, _maxLookX);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnJump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}