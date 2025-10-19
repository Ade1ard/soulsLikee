using UnityEngine;

public class BonFireMenu : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup BonFireMenuUI;

    private UIFader _uiFader;
    private MenuesController _menuesController;

    private Coroutine _coroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
    }

    public void SetActive(bool _bool)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(_uiFader.Fading(BonFireMenuUI, _bool));

        _uiFader.Fade(BonFireMenuUI, _bool);
        if (_bool)
            _menuesController.SetCurrnetMenu(this);
    }

    private BonFireCont _bonFireCont;
    public void GetCurrentBonFire(BonFireCont bonFireCont) => _bonFireCont = bonFireCont;

    public void QuitButton()
    {
        _bonFireCont.QuitBonfire();
        _menuesController.CloseMenu();
    }
}
