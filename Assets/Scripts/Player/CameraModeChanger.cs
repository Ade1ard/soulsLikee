using Cinemachine;
using UnityEngine;

public class CameraModeChanger : MonoBehaviour
{
    [Header("LockCameraSettings")]
    [SerializeField] private float _cameraLockDistance;
    [SerializeField] private float _FindEnemyRadius = 3f;
    private CinemachineVirtualCamera _lockOnCamera;
    private CinemachineFreeLook _freeLookCamera;

    private Animator _animator;
    private Camera _camera;
    private PlayerController _playerController;
    private EnemyController _enemyLockedOn;

    private bool _isCameraLocked = false;

    void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _animator = GetComponent<Animator>();
        _playerController = FindObjectOfType<PlayerController>();
        _lockOnCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _freeLookCamera = FindObjectOfType<CinemachineFreeLook>();

        _freeLookCamera.Priority = 20;
        _lockOnCamera.Priority = 0;

        InvokeRepeating("CheckDistanceToLockEnemy", 2f, 2f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            ChangeCameraLookMod();
        }
    }

    private void ChangeCameraLookMod()
    {
        if (_isCameraLocked)
        {
            _lockOnCamera.Priority = 0;
            _freeLookCamera.Priority = 20;
            _isCameraLocked = false;

            _playerController.TakeNearEnemy(null);

            _lockOnCamera.LookAt = null;
            _animator.SetBool("IsCameraLocked", false);
        }
        else
        {
            _enemyLockedOn = GetNearEnemy();

            if (_enemyLockedOn != null)
            {
                _lockOnCamera.LookAt = _enemyLockedOn._cameralookAt.transform;
                _isCameraLocked = true;

                _playerController.TakeNearEnemy(_enemyLockedOn._cameralookAt.transform);

                _lockOnCamera.Priority = 20;
                _freeLookCamera.Priority = 0;
                _animator.SetBool("IsCameraLocked", true);
            }
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
