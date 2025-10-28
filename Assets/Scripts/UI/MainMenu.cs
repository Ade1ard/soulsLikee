using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup _mainMenuUI;

    private UIFader _uiFader;
    private MenuesController _menuesController;
    private SavesManager _savesManager;

    private Coroutine _coroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
        _savesManager = bootStrap.Resolve<SavesManager>();
    }

    public void Play()
    {
        _menuesController.CloseMenu(true);
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        _menuesController.CloseMenu(true);
        _savesManager.DeleteAllSaves();
        Play();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            _menuesController.CloseMenu(false);
        }
    }

    private void Start()
    {
        gameObject.SetActive(true);
        _menuesController.SetCurrnetMenu(this);
        _savesManager.LoadGame();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetActive(bool _bool)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(_uiFader.Fading(_mainMenuUI, _bool));

        if (_bool)
            _menuesController.SetCurrnetMenu(this);
    }
}