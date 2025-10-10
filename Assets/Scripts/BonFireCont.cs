using UnityEngine;

public class BonFireCont : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _cameraLookAt;

    [Header("UI")]
    [SerializeField] private CanvasGroup _gamePlayUI;
    [SerializeField] private CanvasGroup _menuUI;

    [Header("String")]
    [SerializeField] private string _tutorialText;

    private TutorialClueCont _tutorialClueCont;
    private PlayerController _playerController;
    private CameraModeChanger _cameraChanger;
    private UIFader _uiFader;

    private bool _NearBonFire = false;
    private bool _isSitting = false;

    private bool _inMenu;

    public void Initialize(BootStrap bootStrap)
    {
        _tutorialClueCont = bootStrap.Resolve<TutorialClueCont>();
        _cameraChanger = bootStrap.Resolve<CameraModeChanger>();
        _playerController = bootStrap.Resolve<PlayerController>();
        _uiFader = bootStrap.Resolve<UIFader>();
    }

    void Update()
    {
        if (_NearBonFire)
        {
            if (Input.GetKeyUp(KeyCode.F) && !_isSitting)
            {
                SitBonfire();
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                if (_inMenu)
                {
                    QuitBonfire();
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

        _uiFader.Fade(_menuUI, false);
        _uiFader.Fade(_gamePlayUI, true);

        _cameraChanger.CameraOnBonfire(_cameraLookAt);
    }

    private void SitBonfire()
    {
        _playerAnimator.SetTrigger("BonFireSitDown");
        _playerController.IsHealing(1);
        _isSitting = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _uiFader.Fade(_menuUI, true);
        _uiFader.Fade(_gamePlayUI, false);

        _tutorialClueCont.TutorialGetUnvisible();

        _cameraChanger.CameraOnBonfire(_cameraLookAt);
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

    private void Start()
    {
        _uiFader.Fade(_menuUI, false);
    }
}
