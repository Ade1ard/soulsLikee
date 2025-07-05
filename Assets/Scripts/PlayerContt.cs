using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContt : MonoBehaviour
{
    private Animator _animator;

    private Camera _camera;
    private CharacterController _characterController;
    [SerializeField] private Transform _targetPosition;

    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private float _runSpeed = 6;
    private float _currentSpeed;
    private float _fallVelociti;
    private Vector3 _moveVector;

    private float _timePressedButton;
    [SerializeField] private float _rollDistanse = 10;
    [SerializeField] private float _buttonPressDelay = 0.2f;

    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _characterController = FindObjectOfType<CharacterController>();
        _animator = FindObjectOfType<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _moveVector = Vector3.zero;

        if (Input.GetAxis(Vertical) != 0 || Input.GetAxis(Horizontal) != 0)
        {
            Movement();
        }
        else
        {
            _animator.SetFloat("speed", -1);
        }

        PhysicsMove();
    }

    private void Movement()
    {
        Vector3 playerDir = _targetPosition.position - transform.position;
        playerDir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDir), 15 * Time.deltaTime);
        Vector3 targetPositionDir = _camera.transform.forward * Input.GetAxis(Vertical) + _camera.transform.right * Input.GetAxis(Horizontal);
        Ray ray = new Ray(transform.position, targetPositionDir);
        _targetPosition.position = ray.GetPoint(15);
        _moveVector += transform.forward;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _timePressedButton = Time.time;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time - _timePressedButton >= _buttonPressDelay)
        {
            _currentSpeed = _runSpeed;
            _animator.SetFloat("speed", 2);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && Time.time - _timePressedButton < _buttonPressDelay)
        {
            _animator.SetTrigger("roll");
            _moveVector += transform.forward * _rollDistanse;
            _currentSpeed = 10;
        }
        else
        {
            _currentSpeed = _walkSpeed;
            _animator.SetFloat("speed", 1);
        }
    }

    private void PhysicsMove()
    {
        _characterController.Move(_moveVector * _currentSpeed * Time.deltaTime);

        if (_characterController.isGrounded == false)
        {
            _fallVelociti += _gravity * Time.fixedDeltaTime;
            _characterController.Move(Vector3.down * _fallVelociti * Time.deltaTime);
        }
        else
        {
            _fallVelociti = 0;
        }
    }
}