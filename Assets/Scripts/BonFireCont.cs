using UnityEngine;

public class BonFireCont : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _cameraLookAt;

    [Header("UI")]
    [SerializeField] private CanvasGroup _gamePlayUI;

    [Header("String")]
    [SerializeField] private string _tutorialText;

    private TutorialClueCont _tutorialClueCont;
    private PlayerController _playerController;
    private CameraModeChanger _cameraChanger;
    private UIFader _uiFader;
    private MenuesController _menuesController;
    private BonFireMenu _bonFireMenu;
    private EscapeMenu _escapeMenu;
    private JsonSaveSystem _saveSystem;
    private SceneReboot _sceneReboot;

    private bool _NearBonFire = false;
    private bool _isSitting = false;

    public void Initialize(BootStrap bootStrap)
    {
        _saveSystem = bootStrap.Resolve<JsonSaveSystem>();
        _tutorialClueCont = bootStrap.Resolve<TutorialClueCont>();
        _cameraChanger = bootStrap.Resolve<CameraModeChanger>();
        _playerController = bootStrap.Resolve<PlayerController>();
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
        _bonFireMenu = bootStrap.Resolve<BonFireMenu>();
        _escapeMenu = bootStrap.Resolve<EscapeMenu>();
        _sceneReboot = bootStrap.Resolve<SceneReboot>();
    }

    void Update()
    {
        if (_NearBonFire)
        {
            if (Input.GetKeyUp(KeyCode.F) && !_isSitting)
            {
                SitBonfire();
            }

            if (Input.GetKeyUp(KeyCode.Tab) && _isSitting)
            {
                if (_menuesController.CloseMenu(true))
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
        _isSitting = false;
        Invoke("EscMenuCanOpen", 2);

        _playerAnimator.SetTrigger("BonFireStandUp");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _uiFader.Fade(_gamePlayUI, true);

        _cameraChanger.CameraOnBonfire(_cameraLookAt, false);

        _saveSystem.SaveGame();
    }

    private void SitBonfire()
    {
        _isSitting = true;
        _escapeMenu.InOtherMenu(true);
        _escapeMenu.CloseESCMenu();

        _playerController.IsHealing(1);
        _playerAnimator.SetTrigger("BonFireSitDown");

        _sceneReboot.RebootScene();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _uiFader.Fade(_gamePlayUI, false);
        _bonFireMenu.SetActive(true);
        _bonFireMenu.GetCurrentBonFire(this);

        _tutorialClueCont.TutorialGetUnvisible();

        _cameraChanger.CameraOnBonfire(_cameraLookAt, true);

        _saveSystem.SaveGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerController.gameObject == other.gameObject)
        {
            _NearBonFire = true;
            _tutorialClueCont.TutorialGetVisible(_tutorialText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playerController.gameObject == other.gameObject)
        {
            _NearBonFire = false;
            _tutorialClueCont.TutorialGetUnvisible();
        }
    }

    private void EscMenuCanOpen() => _escapeMenu.InOtherMenu(false);
}
