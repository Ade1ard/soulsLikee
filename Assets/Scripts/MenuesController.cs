public class MenuesController
{
    private IMenu _currentMenu;

    public void SetCurrnetMenu(IMenu menu)
    {
        if (_currentMenu != null)
            _currentMenu.SetActive(false);
        _currentMenu = menu;
    }

    public void CloseMenu()
    {
        _currentMenu.SetActive(false);
    }
}
