using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    public float JumpForce;
    public float speed;

    public float speedWalk;
    public float speedRun;

    private Vector3 _moveVector;

    public float gravity = 9.8f;
    private float _fallVelociti = 0;

    private CharacterController _characterController;
    public Camera _camera;
    public Transform posTarget;

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(_moveVector == Vector3.zero)
        {
            animator.SetFloat("speed", -1);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = speedRun;
            if (_moveVector != Vector3.zero)
            {
                animator.SetFloat("speed", 2);
            }
        }
        else
        {
            speed = speedWalk;
            if(_moveVector != Vector3.zero)
            {
                animator.SetFloat("speed", 1);
            }
        }

        _moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.W) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(_camera.transform.position, -_camera.transform.forward);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _fallVelociti = -JumpForce;
        }


        if (_fallVelociti != 0)
        {
            animator.SetBool("Is Grounded", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            animator.SetTrigger("Jump");
        }

        if (_characterController.isGrounded)
        {
            animator.SetBool("Is Grounded", true);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _characterController.Move(_moveVector * speed * Time.fixedDeltaTime);

        _fallVelociti += gravity * Time.fixedDeltaTime;
        _characterController.Move(Vector3.down * _fallVelociti * Time.fixedDeltaTime);

        if (_characterController.isGrounded)
        {
            _fallVelociti = 0;
        }
    }
}