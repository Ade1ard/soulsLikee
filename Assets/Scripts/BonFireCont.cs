using UnityEngine;

public class BonFireCont : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private Transform _cameraLookAt;

    [Header("UI")]
    [SerializeField] private GameObject _gamePlayUI;
    [SerializeField] private GameObject _bonfireUI;
    [SerializeField] private GameObject _PressToSitUI;

    private PlayerController _playerController;
    private CameraModeChanger _cameraChanger;

    private bool _NearBonFire = false;
    private bool _isSitting = false;

    void Start()
    {
        _cameraChanger = FindObjectOfType<CameraModeChanger>();
        _playerController = FindObjectOfType<PlayerController>();

        _bonfireUI.SetActive(false);
        _PressToSitUI.SetActive(false);
    }

    void Update()
    {
        if (_NearBonFire)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                SitBonfire();
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                QuitBonfire();
            }

            if (_isSitting)
            {
                Vector3 dir = transform.position - _playerController.transform.position;
                dir.y = 0;
                _playerController.transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }

    public void QuitBonfire()
    {
        _playerAnimator.SetTrigger("BonFireStandUp");
        _isSitting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _gamePlayUI.SetActive(true);
        _bonfireUI.SetActive(false);

        _cameraChanger.CameraOnBonfire(_cameraLookAt);
    }

    private void SitBonfire()
    {
        _playerAnimator.SetTrigger("BonFireSitDown");
        _playerController.IsHealing(1);
        _isSitting = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _gamePlayUI.SetActive(false);
        _bonfireUI.SetActive(true);

        _PressToSitUI.SetActive(false);

        _cameraChanger.CameraOnBonfire(_cameraLookAt);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerController.gameObject == other.gameObject)
        {
            _NearBonFire = true;
            _PressToSitUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _NearBonFire = false;
        _PressToSitUI.SetActive(false);
    }
}
