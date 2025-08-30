using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraModeChanger : MonoBehaviour
{
    [Header("LockCameraSettings")]
    [SerializeField] private float _cameraLockDistance;
    [SerializeField] private float _FindEnemyRadius = 3f;

    [Header("UI")]
    [SerializeField] private Image _EnemyTargetLockUI;

    [Header("Transform")]
    [SerializeField] private Transform _defaultLookAt;
    [SerializeField] private CinemachineVirtualCamera _lockOnCamera;
    [SerializeField] private CinemachineFreeLook _freeLookCamera;

    private Animator _animator;
    private Camera _camera;
    private PlayerController _playerController;
    private EnemyController _enemyLockedOn;
    private CinemachineFramingTransposer _framingTransposer;

    private bool _isCameraLocked = false;
    private bool _isCameraOnBonfire = false;

    private void Awake()
    {
        _framingTransposer = _lockOnCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _camera = FindObjectOfType<Camera>();
        _animator = GetComponent<Animator>();
        _playerController = FindObjectOfType<PlayerController>();

        _lockOnCamera.LookAt = _defaultLookAt;
    }
    void Start()
    {
        _EnemyTargetLockUI.enabled = false;

        _freeLookCamera.Priority = 20;
        _lockOnCamera.Priority = 0;

        InvokeRepeating("CheckDistanceToLockEnemy", 2f, 2f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && !_isCameraOnBonfire)
        {
            ChangeCameraLookMod();
        }

        if (_isCameraLocked)
        {
            var screenPosition = Camera.main.WorldToScreenPoint(_enemyLockedOn._enemySpine.position);
            _EnemyTargetLockUI.rectTransform.position = screenPosition;
        }
    }

    private void ChangeCameraLookMod()
    {
        if (_isCameraLocked)
        {
            _isCameraLocked = false;
            _EnemyTargetLockUI.enabled = false;
            _lockOnCamera.Priority = 0;
            _freeLookCamera.Priority = 20;

            _playerController.TakeNearEnemy(null);

            _lockOnCamera.LookAt = null;
            _enemyLockedOn = null;
            _animator.SetBool("IsCameraLocked", false);
        }
        else
        {
            _enemyLockedOn = GetNearEnemy();

            if (_enemyLockedOn != null)
            {
                _isCameraLocked = true;
                _EnemyTargetLockUI.enabled = true;
                _lockOnCamera.LookAt = _enemyLockedOn._cameralookAt;

                _playerController.TakeNearEnemy(_enemyLockedOn._cameralookAt);

                _lockOnCamera.Priority = 20;
                _freeLookCamera.Priority = 0;
                _animator.SetBool("IsCameraLocked", true);
            }
        }
    }

    public void CameraOnBonfire(Transform bonfire)
    {
        if (_isCameraLocked)
        {
            ChangeCameraLookMod();
        }

        if (!_isCameraOnBonfire)
        {
            _isCameraOnBonfire = true;

            _lockOnCamera.LookAt = bonfire;

            _lockOnCamera.Priority = 20;
            _freeLookCamera.Priority = 0;

            _framingTransposer.m_ScreenX += 0.2f;
        }
        else
        {
            _isCameraOnBonfire = false;

            _framingTransposer.m_ScreenX -= 0.2f;

            _lockOnCamera.Priority = 0;
            _freeLookCamera.Priority = 20;

            _lockOnCamera.LookAt = null;
        }
    }

    private void CheckDistanceToLockEnemy()
    {
        if (_isCameraLocked)
        {
            if (Vector3.Distance(_enemyLockedOn._cameralookAt.transform.position, transform.position) > _cameraLockDistance)
            {
                ChangeCameraLookMod();
            }

            if (!_enemyLockedOn.CheckAlive())
            {
                ChangeCameraLookMod();
            }
        }
    }

    private EnemyController GetNearEnemy()
    {
        RaycastHit[] hits = Physics.SphereCastAll(_camera.transform.position, _FindEnemyRadius, _camera.transform.forward, _cameraLockDistance);

        EnemyController closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent(out EnemyController enemy))
            {
                if (enemy.CheckAlive())
                {
                    Vector3 direction = hit.transform.position - _camera.transform.position;
                    float distance = direction.magnitude;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
        return closestEnemy;
    }
}
