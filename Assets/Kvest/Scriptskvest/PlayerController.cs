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

    private float timeRollMove;
    private bool inRoll;

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

        timeRollMove -= 2;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - timelastclicked >= 0.86f && Input.GetKey(KeyCode.LeftShift) == false && inRoll == false && Time.time - timeRollMove > 1.5f)
        {
            timelastclicked = Time.time;
            if (Time.time - timeAttack1 <= 1.6f && Time.time - timeAttack1 > 0.7 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            {
                animator.SetTrigger("attack2");
                timeAttack2 = Time.time;
                inAttack = true;
            }
            else
            {
                animator.SetTrigger("attack1");
                timeAttack1 = Time.time;
                inAttack = true;
            }
        }
        if (Input.GetMouseButtonDown(0) && Time.time - timelastclicked >= 1.6f && Input.GetKey(KeyCode.LeftShift) && inRoll == false && Time.time - timeRollMove > 1.5f)
        {
            timelastclicked = Time.time;
            animator.SetTrigger("attack3");
            inAttack = true;
            inRoll = true;
        }

            if (_moveVector == Vector3.zero)
        {
            animator.SetFloat("speed", -1);
        }

        if (Input.GetKey(KeyCode.Space))
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

        if(Time.time - timeRollMove < 1.47f)
        {
            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.D) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.8f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.8f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.D) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.8f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.8f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false && inAttack == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.8f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false && inAttack == false || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.8f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.W) == false && inAttack == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(_camera.transform.position, -_camera.transform.forward);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.8f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false && inAttack == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) == false && inAttack == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
            if (Input.GetKeyDown(KeyCode.Space) && inRoll == false && Time.time - timeRollMove > 1.7f)
            {
                inRoll = true;
                animator.SetTrigger("roll");
                timeRollMove = Time.time;
            }
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
            Invoke("OffAttack", 1.5f);
        }

        if (inRoll == true)
        {
            Invoke("OffRoll", 1.7f);
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

    void OffAttack()
    {
        inAttack = false;
    }
    void OffRoll()
    {
        inRoll = false;
    }
}