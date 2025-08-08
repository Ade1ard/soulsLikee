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
            Transform EnemyLockedOn = GetNearEnemy();

            if (EnemyLockedOn != null)
            {
                _lockOnCamera.LookAt = EnemyLockedOn;
                _isCameraLocked = true;

                _playerController.TakeNearEnemy(EnemyLockedOn);

                _lockOnCamera.Priority = 20;
                _freeLookCamera.Priority = 0;
                _animator.SetBool("IsCameraLocked", true);
            }
        }
    }

    private Transform GetNearEnemy()
    {
        RaycastHit[] hits = Physics.SphereCastAll(_camera.transform.position, _FindEnemyRadius, _camera.transform.forward, _cameraLockDistance);

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
        return closestEnemy;
    }
}
