using UnityEngine;
using UnityEngine.UI;

public class GameSettingsView : MonoBehaviour, IMenu
{
    [Header("Objects")]
    [SerializeField] private CanvasGroup _gameSettingsUI;

    [Header("Scrollbars")]
    [SerializeField] private Scrollbar _scrollbarCameraDist;
    [SerializeField] private Scrollbar _scrollbarCameraSpeed;
    [SerializeField] private Scrollbar _scrollbarUISize;

    private UIFader _uiFader;
    private MenuesController _menuesController;
    private GameSettings _gameSettings;

    private Coroutine _coroutine;

    public void CameraDistChanged()
    {
        _gameSettings.GetCameraDistanse(_scrollbarCameraDist.value);
    }

    public void CameraSpeedChanged()
    {
        _gameSettings.GetCameraSpeed(_scrollbarCameraSpeed.value);
    }

    public void UISizeChanged()
    {
        _gameSettings.GetUISize(_scrollbarUISize.value);
    }

    public void Initialize(BootStrap bootStrap)
    {
        _gameSettings = bootStrap.Resolve<GameSettings>();
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
    }

    void Start()
    {
        _gameSettingsUI.gameObject.SetActive(false);
    }

    public void SetActive(bool _bool)
    {
        if (_bool)
            _menuesController.SetCurrnetMenu(this);
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(_uiFader.Fading(_gameSettingsUI, _bool));
    }

    public void LoadScrollBars(float cameraDist, float cameraSpeed, float uiSize)
    {
        _scrollbarCameraDist.value = cameraDist;
        _scrollbarCameraSpeed.value = cameraSpeed;
        _scrollbarUISize.value = uiSize;
    }
}
