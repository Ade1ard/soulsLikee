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

    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1.5f;

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
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("attack1", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("attack2", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            animator.SetBool("attack3", false);
            noClicks = 0;
        }

        if(Time.time - lastClickedTime > maxComboDelay)
        {
            noClicks = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        if (_moveVector == Vector3.zero)
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

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.D) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position) + (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.D) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position) - (transform.position - _camera.transform.position), _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, -_camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(transform.position, _camera.transform.right);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.W) == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) == false)
        {
            Vector3 dir = posTarget.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            Ray ray = new Ray(_camera.transform.position, -_camera.transform.forward);
            posTarget.position = ray.GetPoint(15);

            _moveVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) == false)
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

    void OnClick()
    {
        lastClickedTime = Time.time;
        noClicks += 1;
        if(noClicks == 1)
        {
            animator.SetBool("attack1", true);
        }
        noClicks = Mathf.Clamp(noClicks, 0, 3);

        if(noClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", true);
        }

        if(noClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("attack2", false);
            animator.SetBool("attack3", true);
        }
    }
}