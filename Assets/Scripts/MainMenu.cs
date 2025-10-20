using UnityEngine;

public class MainMenu : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup _mainMenuUI;

    private UIFader _uiFader;
    private MenuesController _menuesController;

    private Coroutine _coroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
    }

    private void Start()
    {
        SetActive(true);
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
