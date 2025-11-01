using UnityEngine;

public class EscapeMenu : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup _EscMenuUI;
    [SerializeField] private CanvasGroup _gamePlayUI;

    private bool _inOtherMenu = false;

    private UIFader _uiFader;
    private MenuesController _menuesController;
    private PlayerController _playerController;

    private Coroutine _ThisUIcoroutine;
    private Coroutine _gamePlayUIcoroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
        _playerController = bootStrap.Resolve<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) && !_inOtherMenu)
        {
            if (_menuesController.IsThereOpenedMenu())
            {
                if (_menuesController.CloseMenu(true))
                {
                    SetEnvironmentVisible(true);
                }
            }
            else
            {
                SetActive(true);
                SetEnvironmentVisible(false);
            }
        }
    }

    private void SetEnvironmentVisible(bool _bool)
    {
        Cursor.visible = !_bool;
        Cursor.lockState = _bool? CursorLockMode.Locked : CursorLockMode.None;

        _playerController.AttackLock(!_bool);

        if (_gamePlayUIcoroutine != null)
            StopCoroutine(_gamePlayUIcoroutine);
        _gamePlayUIcoroutine = StartCoroutine(_uiFader.Fading(_gamePlayUI, _bool));
    }

    public void CloseESCMenu() //for button
    {
        _menuesController.CloseMenu(true);
        SetEnvironmentVisible(true);
    }

    public void SetActive(bool _bool)
    {
        if (_ThisUIcoroutine != null)
            StopCoroutine(_ThisUIcoroutine);
        _ThisUIcoroutine = StartCoroutine(_uiFader.Fading(_EscMenuUI, _bool));

        if (_bool)
            _menuesController.SetCurrnetMenu(this);
    }

    public void InOtherMenu(bool _bool) => _inOtherMenu = _bool;
}
