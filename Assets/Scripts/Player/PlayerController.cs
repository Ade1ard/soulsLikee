using Cinemachine;
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
    private float _currentSpeed;

    [Header("RollAndRunSettings")]
    [SerializeField] private float _buttonPressDelay = 0.2f;
    private float _timePressedButton;

    [Header("LockCameraSettings")]
    [SerializeField] private float _cameraLockDistance;
    private CinemachineVirtualCamera _lockOnCamera;
    private Transform _enemyLockedOn;
    private bool _isCameraLocked = false;

    private bool _isRolling = false;
    private bool _inAttack = false;

    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    private void Start()
    {
        _lockOnCamera = FindObjectOfType<CinemachineVirtualCamera>();
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
        if (Input.GetMouseButtonDown(2))
        {
            ChooseCameraLookMod();
        }

        if (_isCameraLocked)
        {
            LockOnMovement();
        }
        else
        {
            FreeLookMovement();
        }

        Attachment();
        PhysicsMove();
    }

    private void FreeLookMovement()
    {
        _moveVector = Vector3.zero;

        if ((Input.GetAxis(Vertical) != 0 || Input.GetAxis(Horizontal) != 0) && !_inAttack)
        {
            Vector3 playerDir = _targetPositionPoint.position - transform.position;
            playerDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDir), 15 * Time.deltaTime);

            Vector3 targetPositionPointDir = _camera.transform.forward * Input.GetAxis(Vertical) + _camera.transform.right * Input.GetAxis(Horizontal);
            Ray ray = new Ray(transform.position, targetPositionPointDir);
            _targetPositionPoint.position = ray.GetPoint(15);
            _moveVector += transform.forward;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                _timePressedButton = Time.time;
            }

            if (Input.GetKey(KeyCode.Space) && Time.time - _timePressedButton >= _buttonPressDelay && _stamina.CheckStamina() >= 2f)
            {
                _currentSpeed = _runSpeed;
                _animator.SetFloat("speed", 2);
                _stamina.SpentStamina(_stamina.GetCoast("run") * Time.deltaTime);
            }
            else if (Input.GetKeyUp(KeyCode.Space) && Time.time - _timePressedButton < _buttonPressDelay && !_isRolling && _stamina.CheckStamina() >= 2f)
            {
                _isRolling = true;
                _animator.SetTrigger("roll");
                _stamina.SpentStamina(_stamina.GetCoast("roll"));
            }
            else
            {
                _currentSpeed = _walkSpeed;
                _animator.SetFloat("speed", 1);
            }
        }
        else
        {
            _animator.SetFloat("speed", -1);
        }
    }

    private void LockOnMovement()
    {
        Vector3 playerDir = _enemyLockedOn.transform.position - transform.position;
        playerDir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDir), 15 * Time.deltaTime);
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
            _stamina.SpentStamina(_stamina.GetCoast("heavyAttack"));
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            _animator.SetTrigger("Attack2");
            _stamina.SpentStamina(_stamina.GetCoast("attack"));
        }
        else
        {
            _animator.SetTrigger("Attack1");
            _stamina.SpentStamina(_stamina.GetCoast("attack"));
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

    private void ChooseCameraLookMod()
    {
        if (_isCameraLocked)
        {
            _isCameraLocked = false;
            _enemyLockedOn = null;
            _lockOnCamera.Priority = 9;
            _lockOnCamera.LookAt = null;
        }
        else
        {
            GetNearEnemy();
            if (_enemyLockedOn != null)
            {
                _isCameraLocked = true;
                _lockOnCamera.Priority = 11;
                _lockOnCamera.LookAt = _enemyLockedOn;
            }
        }
    }

    private void GetNearEnemy()
    {
        RaycastHit[] hits = Physics.SphereCastAll(_camera.transform.position, 2f, _camera.transform.forward, _cameraLockDistance);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent(out EnemyController enemy))
            {
                Vector3 direction = hit.transform.position - _camera.transform.position;
                float distance = direction.magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy._cameralookAt.transform;
                }
            }
        }
        _enemyLockedOn = closestEnemy;
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