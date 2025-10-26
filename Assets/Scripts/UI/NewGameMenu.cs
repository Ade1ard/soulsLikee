using UnityEngine;

public class NewGameMenu : MonoBehaviour, IMenu
{
    [SerializeField] CanvasGroup _newGameMenuUI;
    
    private UIFader _uifader;
    private MenuesController _menuesController;

    public void Initialize(BootStrap bootStrap)
    {
        _uifader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
    }

    public void CloseMenu()
    {
        _menuesController.CloseMenu(false);
    }

    public void SetActive(bool _bool)
    {
        _uifader.Fade(_newGameMenuUI, _bool);
        if (_bool)
            _menuesController.SetCurrnetMenu(this);
    }
}
