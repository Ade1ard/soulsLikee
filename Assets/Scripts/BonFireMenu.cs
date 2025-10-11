using UnityEngine;

public class BonFireMenu : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup BonFireMenuUI;

    private UIFader _uiFader;
    private MenuesController _menuesController;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
    }

    public void SetActive(bool _bool)
    {
        _uiFader.Fade(BonFireMenuUI, _bool);
        if (_bool)
            _menuesController.SetCurrnetMenu(this);
    }
}
