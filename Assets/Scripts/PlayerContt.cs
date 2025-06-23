using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContt : MonoBehaviour
{
    private Camera _camera;
    private CharacterController _characterController;
    [SerializeField] private Transform _targetPosition;

    [SerializeField] private float _gravity = 9.8f;
    private float _currentSpeed = 2;
    private float _fallVelociti = 0;
    private Vector3 _moveVector;

    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _characterController = FindObjectOfType<CharacterController>();

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
    }

    void FixedUpdate()
    {
        _characterController.Move(_moveVector * _currentSpeed * Time.fixedDeltaTime);

        _fallVelociti += _gravity * Time.fixedDeltaTime;
        _characterController.Move(Vector3.down * _fallVelociti * Time.fixedDeltaTime);

        if (_characterController.isGrounded)
        {
            _fallVelociti = 0;
        }
    }
}