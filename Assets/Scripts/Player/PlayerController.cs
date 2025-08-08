using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;

    [Header("Objects")]
    [SerializeField] private Transform _targetPositionPoint;
    private Camera _camera;
    private CharacterController _characterController;
    private PlayerSword _sword;
    private StaminaPlayerController _stamina;

    [Header("Phisics")]
    [SerializeField] private float _gravity = 9.8f;
    private float _fallVelociti;
    private Vector3 _moveVector;

    [Header("MoveSpeeds")]
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private float _lockOnWalkSpeed = 2;
    [SerializeField] private float _changeAnimationsSpeed = 1.6f;
    private float _currentSpeed;

    [Header("RollAndRunSettings")]
    [SerializeField] private float _buttonPressDelay = 0.2f;
    private float _timePressedButton;

    [Header("LockCameraSettings")]
    private Transform _enemyLockedOn;
    private bool _isCameraLocked = false;

    private bool _isRolling = false;
    private bool _inAttack = false;
    private bool _isSprinting = false;

    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    private void Start()
    {
        _stamina = GetComponent<StaminaPlayerController>();
        _camera = FindObjectOfType<Camera>();
        _characterController = FindObjectOfType<CharacterController>();
        _sword = FindObjectOfType<PlayerSword>();
        _animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!_isCameraLocked)
        {
            FreeLookMovement();
        }
        else
        {
            LockOnMovement();
        }

        Attachment();
        PhysicsMove();
    }

    private void FreeLookMovement()
    {
        _moveVector = Vector3.zero;

        if ((Input.GetAxis(Vertical) != 0 || Input.GetAxis(Horizontal) != 0))
        {
            if (!_inAttack)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetPlayerDirection()), 15 * Time.deltaTime);
                GetTargetPointPosition();
                _moveVector += transform.forward;
            }

            RoolingSprinting();

            if (!_isSprinting)
            {
                _currentSpeed = _walkSpeed;
                _animator.SetFloat("speed", Mathf.Lerp(_animator.GetFloat("speed"), 1, _changeAnimationsSpeed * Time.deltaTime));
            }
        }
        else
        {
            _animator.SetFloat("speed", Mathf.Lerp(_animator.GetFloat("speed"), 0, _changeAnimationsSpeed * Time.deltaTime));
        }
    }

    private void LockOnMovement()
    {
        _moveVector = Vector3.zero;

        if (!_isRolling && !_isSprinting)
        {
            Vector3 PlayerDir = _enemyLockedOn.position - transform.position;
            PlayerDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerDir), 15 * Time.deltaTime);
        }

        if ((Input.GetAxis(Vertical) != 0 || Input.GetAxis(Horizontal) != 0))
        {
            if (!_inAttack)
            {
                _moveVector += _camera.transform.forward * Input.GetAxis(Vertical) + _camera.transform.right * Input.GetAxis(Horizontal);
            }

            RoolingSprinting();

            if (!_isSprinting)
            {
                _currentSpeed = _lockOnWalkSpeed;
                _animator.SetFloat("speed", 1);
                _animator.SetFloat("H_Speed", Mathf.Lerp(_animator.GetFloat("H_Speed"), Input.GetAxis(Horizontal), _changeAnimationsSpeed * Time.deltaTime));
                _animator.SetFloat("V_Speed", Mathf.Lerp(_animator.GetFloat("V_Speed"), Input.GetAxis(Vertical), _changeAnimationsSpeed * Time.deltaTime));
            }
        }
        else
        {
            _animator.SetFloat("speed", 0);
        }
    }

    private void RoolingSprinting()
    {
        if (_isCameraLocked)
        {
            GetTargetPointPosition();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _timePressedButton = Time.time;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time - _timePressedButton >= _buttonPressDelay && _stamina.CheckStamina() >= 2f)
        {
            if (_isCameraLocked)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetPlayerDirection()), 15 * Time.deltaTime);
                _animator.SetBool("IsCameraLocked", false);
            }

            _isSprinting = true;
            _currentSpeed = _runSpeed;
            _animator.SetFloat("speed", Mathf.Lerp(_animator.GetFloat("speed"), 2, _changeAnimationsSpeed * Time.deltaTime));
            _stamina.SpentStamina(_stamina.GetCost("run") * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && Time.time - _timePressedButton < _buttonPressDelay && !_isRolling && _stamina.CheckStamina() >= 2f)
        {
            if (_isCameraLocked)
            {
                transform.rotation = Quaternion.LookRotation(GetPlayerDirection());
            }

            _isSprinting = true;
            _isRolling = true;
            _animator.SetTrigger("roll");
            _stamina.SpentStamina(_stamina.GetCost("roll"));
        }
        else 
        {
            if (_isCameraLocked)
            {
                _animator.SetBool("IsCameraLocked", true);
            }
            _isSprinting = false;
        }
    }

    private void Attachment()
    {
        if(Input.GetMouseButtonDown(0) && !_inAttack && !_isRolling && _stamina.CheckStamina() >= 2f)
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
            _stamina.SpentStamina(_stamina.GetCost("heavyAttack"));
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            _animator.SetTrigger("Attack2");
            _stamina.SpentStamina(_stamina.GetCost("attack"));
        }
        else
        {
            _animator.SetTrigger("Attack1");
            _stamina.SpentStamina(_stamina.GetCost("attack"));
        }
    }

    private void EndAttack() //called by events in animations
    {
        _inAttack = false;
        _sword.EndAttack();
    }

    private void StartAttack() //called by events in animations
    {
        _sword.StartAttack();
    }
    
    private void EndRoll() //called by events in animations
    {
        _isRolling = false;
    }

    private void GetTargetPointPosition()
    {
        Vector3 targetPositionPointDir = _camera.transform.forward * Input.GetAxis(Vertical) + _camera.transform.right * Input.GetAxis(Horizontal);
        Ray ray = new Ray(transform.position, targetPositionPointDir);
        _targetPositionPoint.position = ray.GetPoint(15);
    }

    private Vector3 GetPlayerDirection()
    {
        Vector3 playerDir = _targetPositionPoint.position - transform.position;
        playerDir.y = 0;
        return playerDir;
    }

    public void TakeNearEnemy(Transform NearEnemy)
    {
        if (NearEnemy != null)
        {
            _enemyLockedOn = NearEnemy;
            _isCameraLocked = true;
        }
        else
        {
            _isCameraLocked = false;
            _enemyLockedOn = null;
        }
    }

    private void PhysicsMove()
    {
        _characterController.Move(_moveVector * _currentSpeed * Time.deltaTime);

        if (!_characterController.isGrounded)
        {
            _fallVelociti += _gravity * Time.deltaTime;
            _characterController.Move(Vector3.down * _fallVelociti * Time.deltaTime);
        }
        else
        {
            _fallVelociti = 0;
        }
    }
}