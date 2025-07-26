using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;

    [Header("Transform")]
    private Camera _camera;
    private CharacterController _characterController;
    [SerializeField] private Transform _targetPositionPoint;

    [Header("Phisics")]
    [SerializeField] private float _gravity = 9.8f;
    private float _fallVelociti;
    private Vector3 _moveVector;

    [Header("MoveSpeeds")]
    private float _currentSpeed;
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private float _walkSpeed = 3.5f;

    [Header("RollAndRunSettings")]
    private float _timePressedButton;
    [SerializeField] private float _buttonPressDelay = 0.2f;

    private bool _isRolling = false;
    private bool _inAttack = false;

    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _characterController = FindObjectOfType<CharacterController>();
        _animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _moveVector = Vector3.zero;

        Movement();
        Attachment();
        PhysicsMove();
    }

    private void Movement()
    {
        if ((Input.GetAxis(Vertical) != 0 || Input.GetAxis(Horizontal) != 0) && _inAttack == false)
        {
            Vector3 playerDir = _targetPositionPoint.position - transform.position;
            playerDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDir), 15 * Time.deltaTime);

            Vector3 targetPositionPointDir = _camera.transform.forward * Input.GetAxis(Vertical) + _camera.transform.right * Input.GetAxis(Horizontal);
            Ray ray = new Ray(transform.position, targetPositionPointDir);
            _targetPositionPoint.position = ray.GetPoint(15);
            _moveVector += transform.forward;
        }
        else
        {
            _animator.SetFloat("speed", -1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _timePressedButton = Time.time;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time - _timePressedButton >= _buttonPressDelay)
        {
            _currentSpeed = _runSpeed;
            _animator.SetFloat("speed", 2);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && Time.time - _timePressedButton < _buttonPressDelay && !_isRolling && _moveVector != Vector3.zero)
        {
            _isRolling = true;
            _animator.SetTrigger("roll");
        }
        else if (_moveVector != Vector3.zero)
        {
            _currentSpeed = _walkSpeed;
            _animator.SetFloat("speed", 1);
        }
    }

    private void Attachment()
    {
        if(Input.GetMouseButtonDown(0) && !_inAttack && !_isRolling)
        {
            Attack();
        }
    }

    private void Attack()
    {
        _inAttack = true;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _animator.SetTrigger("HeavyAttack");
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            _animator.SetTrigger("Attack2");
        }
        else
        {
            _animator.SetTrigger("Attack1");
        }
    }

    private void EndAttack() //called by events in animations
    {
        _inAttack = false;
    }   
    
    private void EndRoll() //called by events in animations
    {
        _isRolling = false;
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