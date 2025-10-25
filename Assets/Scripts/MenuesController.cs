using System.Collections.Generic;

public class MenuesController
{
    private Stack<IMenu> _MenusStack = new Stack<IMenu>();

    public void SetCurrnetMenu(IMenu menu)
    {
        if (!_MenusStack.Contains(menu))
        {
            CurrentMenu(false);
            _MenusStack.Push(menu);
        }
    }

    public bool CloseMenu(bool CanCloseLast)
    {
        if (_MenusStack.Count > 1)
        {
            CurrentMenu(false);
            _MenusStack.Pop();
            CurrentMenu(true);
            return false;
        }
        else if (_MenusStack.Count > 0)
        {
            if (CanCloseLast)
            {
            CurrentMenu(false);
            _MenusStack.Pop();
            }
            return true;
        }
        else
        {
            return true;
        }
    }

    private void CurrentMenu(bool _bool)
    {
        if ( _MenusStack.Count > 0)
            _MenusStack.Peek().SetActive(_bool);
    }
}
