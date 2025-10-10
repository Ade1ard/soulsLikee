using UnityEngine;
using UnityEngine.UI;

public class GameSettingsView : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private CanvasGroup _gameSettingsUI;

    [Header("Scrollbars")]
    [SerializeField] private Scrollbar _scrollbarCameraDist;
    [SerializeField] private Scrollbar _scrollbarCameraSpeed;
    [SerializeField] private Scrollbar _scrollbarUISize;

    private UIFader _uiFader;
    private GameSettings _gameSettings;

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
    }

    void Start()
    {
        _gameSettingsUI.gameObject.SetActive(false);
    }

    public void SetActive(bool _bool)
    {
        _uiFader.Fade(_gameSettingsUI, _bool);
    }
}
