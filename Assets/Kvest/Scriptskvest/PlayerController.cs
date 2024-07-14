using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    public float JumpForce;
    public float speed;

    private Vector3 _moveVector;

    public float gravity = 9.8f;
    private float _fallVelociti = 0;

    private CharacterController _characterController;
    private Camera _camera;

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
        _moveVector = Vector3.zero;
        animator.SetFloat("speed", 0);
        animator.SetFloat("speed2", 0);

        if (Input.GetKey(KeyCode.A))
        {
            _moveVector -= transform.right;
            animator.SetFloat("speed2", -1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _moveVector += transform.right;
            animator.SetFloat("speed2", 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _moveVector -= transform.forward;
            animator.SetFloat("speed", -1);
        }

        if (Input.GetKey(KeyCode.W))
        {
            _moveVector += transform.forward;
            animator.SetFloat("speed", 1);
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