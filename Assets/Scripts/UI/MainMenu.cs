using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup _mainMenuUI;

    private UIFader _uiFader;
    private MenuesController _menuesController;
    private SavesManager _savesManager;
    private TransitionBGCont _transitionBGCont;

    private Coroutine _setActiveCoroutine;
    private Coroutine _gamePlayLoadingCoroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
        _savesManager = bootStrap.Resolve<SavesManager>();
        _transitionBGCont = bootStrap.Resolve<TransitionBGCont>();
    }

    public void Play()
    {
        _menuesController.CloseMenu(true);
        if (_gamePlayLoadingCoroutine == null)
            _gamePlayLoadingCoroutine = StartCoroutine(GamePlayLoading());
    }

    public void NewGame()
    {
        _menuesController.CloseMenu(true);
        _savesManager.DeleteAllSaves();
        Play();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.Escape))
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
        if (_setActiveCoroutine != null)
            StopCoroutine(_setActiveCoroutine);
        _setActiveCoroutine = StartCoroutine(_uiFader.Fading(_mainMenuUI, _bool));

        if (_bool)
            _menuesController.SetCurrnetMenu(this);
    }

    private IEnumerator GamePlayLoading()
    {
        yield return _transitionBGCont.Dissolve(true);

        _gamePlayLoadingCoroutine = null;
        SceneManager.LoadScene(1);
    }
}