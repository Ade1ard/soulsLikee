using UnityEngine;

public class BonFireCont : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _cameraLookAt;

    [Header("UI")]
    [SerializeField] private GameObject _gamePlayUI;
    [SerializeField] private GameObject _bonfireUI;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _levelUpUI;

    [Header("String")]
    [SerializeField] private string _tutorialText;

    private TutorialClueCont _tutorialClueCont;
    private PlayerController _playerController;
    private CameraModeChanger _cameraChanger;

    private bool _NearBonFire = false;
    private bool _isSitting = false;

    private bool _inMenu;
    private bool _inLevelUp;

    void Start()
    {
        _tutorialClueCont = FindObjectOfType<TutorialClueCont>();
        _cameraChanger = FindObjectOfType<CameraModeChanger>();
        _playerController = FindObjectOfType<PlayerController>();

        _levelUpUI.SetActive(false);
        _menuUI.SetActive(true);
        _bonfireUI.SetActive(false);
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
                if (_inMenu)
                {
                    QuitBonfire();
                }
                if (_inLevelUp)
                {
                    LevelUpActive(false);
                }
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

        _tutorialClueCont.TutorialGetUnvisible();

        _cameraChanger.CameraOnBonfire(_cameraLookAt);
    }

    public void LevelUpActive(bool _bool)
    {
        _menuUI.SetActive(!_bool);
        _levelUpUI.SetActive(_bool);
        _inLevelUp = _bool;
        _inMenu = !_bool;
        Debug.Log(!_bool);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerController.gameObject == other.gameObject)
        {
            _NearBonFire = true;
            _inMenu = true;
            _tutorialClueCont.TutorialGetVisible(_tutorialText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playerController.gameObject == other.gameObject)
        {
            _NearBonFire = false;
            _inMenu = false;
            _tutorialClueCont.TutorialGetUnvisible();
        }
    }
}
