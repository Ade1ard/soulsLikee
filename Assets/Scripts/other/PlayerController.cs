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

    private float timeAttack1;
    private float timeAttack2;
    private float timelastclicked;
    private bool inAttack;

    private float timeToRun;
    private bool canRoll;

    private float timeRollMove;
    private bool inRoll;

    private Vector3 _moveVector;

    public float gravity = 9.8f;
    private float _fallVelociti = 0;

    private CharacterController _characterController;
    public Camera _camera;
    public Transform posTarget;

    void Start()
    {
        animator = FindObjectOfType<Animator>();
        _camera = FindObjectOfType<Camera>();
        _characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        timeRollMove -= 2;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - timelastclicked >= 0.8f && Input.GetKey(KeyCode.LeftShift) == false && inRoll == false && Time.time - timeRollMove > 1.1f)
        {
            timelastclicked = Time.time;
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Stand To Roll") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.65f || animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Run") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
            {
                inAttack = true;
                animator.SetTrigger("attack4");
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.45f)
                {
                    inAttack = true;
                    animator.SetTrigger("attack2");
                    timeAttack2 = Time.time;
                }
                else
                {
                    inAttack = true;
                    animator.SetTrigger("attack1");
                    timeAttack1 = Time.time;
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && Time.time - timelastclicked >= 1.4f && Input.GetKey(KeyCode.LeftShift) && inRoll == false && Time.time - timeRollMove > 0.6f)
        {
            timelastclicked = Time.time;
            animator.SetTrigger("attack3");
            inAttack = true;
            inRoll = true;
        }

        if (Input.GetMouseButton(1) && inRoll == false && inAttack == false && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") == false)
        {
            speed = 0;
            animator.SetBool("block", true);
        }
        else
        {
            animator.SetBool("block", false);
        }

        if (_moveVector == Vector3.zero)
        {
            animator.SetFloat("speed", -1); 
        }

        if (Input.GetKey(KeyCode.Space) && inAttack == false)
        {
            canRoll = true;
            timeToRun += 1 * Time.deltaTime;
            if(timeToRun >= 1.1f)
            {
                speed = speedRun;
                if (_moveVector != Vector3.zero)
                {
                    animator.SetFloat("speed", 2);
                }
            }
        }
        if (Input.GetKey(KeyCode.Space) == false && inAttack == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            speed = speedWalk;
            if(_moveVector != Vector3.zero)
            {
                animator.SetFloat("speed", 1);
            }
        }
        if(inAttack == true)
        {
            speed = 0.5f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hit1") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit2") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            inAttack = true;
        }

        if(Input.GetKey(KeyCode.Space) == false && canRoll == true)
        {
            if(timeToRun < 1.1f)
            {
                if (inRoll == false && Time.time - timeRollMove > 1.8f)
                {
                    inRoll = true;
                    animator.SetTrigger("roll");
                    timeRollMove = Time.time;
                }
            }
            timeToRun = 0;
            canRoll = false;
        }

        _moveVector = Vector3.zero;

        if(Time.time - timeRollMove < 1.47f)
        {
            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.D) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.D) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.W) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(_camera.transform.position, -_camera.transform.forward);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) == false && animator.GetCurrentAnimatorStateInfo(0).IsName("block") == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKeyDown(KeyCode.C) && _characterController.isGrounded)
        {
            _fallVelociti = -JumpForce;
        }


        if (_fallVelociti != 0)
        {
            animator.SetBool("Is Grounded", false);
        }

        if (Input.GetKeyDown(KeyCode.C) && _characterController.isGrounded)
        {
            animator.SetTrigger("Jump");
        }

        if (_characterController.isGrounded)
        {
            animator.SetBool("Is Grounded", true);
        }

        if (inAttack == true)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
            {
                inAttack = false;
            }
        }

        if (inRoll == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.77f)
            {
                inRoll = false;
            }
        }
    }

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