using Cinemachine;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : ISaveable
{
    private CinemachineFreeLook _freeLookCamera;
    private CanvasScaler _playerCanvas;
    private CinemachineFramingTransposer _transposer;
    private GameSettingsView _view;

    private float _UISize = 2f;

    private float _freeLookCameraSpeedOffset_X = 1000f;
    private float _freeLookCameraSpeedOffset_Y = 15f;

    private float _lockOnCameraDistanseOffset = 8f;
    private float _topAndBottomRigCameraDistanseOffset = 8f;
    private float _middleRigCameraDistanseOffset = 10f;

    private float _cameraDistTarget = 0.5f;
    private float _cameraSpeedTarget = 0.5f;
    private float _uiSizeTarget = 0.5f;

    public void SaveTo(GameData data)
    {
        var settingsData = new SettingsData();

        settingsData.cameraDistanse = _cameraDistTarget;
        settingsData.cameraSensity = _cameraSpeedTarget;
        settingsData.UIsize = _uiSizeTarget;

        data.settings = settingsData;
    }

    public void LoadFrom(GameData data)
    {
        var settingsData = data.settings;

        float cameraDist = _cameraDistTarget == 0.5f ? settingsData.cameraDistanse : _cameraDistTarget;
        float cameraSpeed = _cameraSpeedTarget == 0.5f ? settingsData.cameraSensity : _cameraSpeedTarget;
        float uiSize = _uiSizeTarget == 0.5f ? settingsData.UIsize : _uiSizeTarget;

        GetCameraDistanse(cameraDist);
        GetCameraSpeed(cameraSpeed);
        GetUISize(uiSize);

        _view.LoadScrollBars(cameraDist, cameraSpeed, uiSize);
    }

    public void InitializeForGamePlayScene(BootStrap bootStrap)
    {
        _view = bootStrap.Resolve<GameSettingsView>();
        _transposer = bootStrap.Resolve<CinemachineFramingTransposer>();
        _freeLookCamera = bootStrap.Resolve<CinemachineFreeLook>();
        _playerCanvas = bootStrap.ResolveAll<CanvasScaler>().FirstOrDefault(e => e.name == "Canvas");
    }
    
    public void InitializeForMainMenuScene(BootStrap bootStrap)
    {
        _view = bootStrap.Resolve<GameSettingsView>();
    }

    public void GetUISize(float target)
    {
        _uiSizeTarget = target;
        float amoint = 0.25f + (Mathf.Clamp(target, 0.1f, 1) - 0.1f) * (0.5f / 0.9f);
        
        if (_playerCanvas != null)
            _playerCanvas.scaleFactor = _UISize * amoint;
    }

    public void GetCameraSpeed(float target)
    {
        _cameraSpeedTarget = target;
        float amoint = 0.25f + (Mathf.Clamp(target, 0.1f, 1) - 0.1f) * (0.5f / 0.9f);

        if (_freeLookCamera != null)
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = amoint * _freeLookCameraSpeedOffset_X;
            _freeLookCamera.m_YAxis.m_MaxSpeed = amoint * _freeLookCameraSpeedOffset_Y;
        }
    }

    public void GetCameraDistanse(float target)
    {
        _cameraDistTarget = target;
        float amoint = 0.25f + (Mathf.Clamp(target, 0.1f, 1) - 0.1f) * (0.5f / 0.9f);

        if (_freeLookCamera != null)
        {
            _freeLookCamera.m_Orbits[0].m_Radius = amoint * _topAndBottomRigCameraDistanseOffset;
            _freeLookCamera.m_Orbits[1].m_Radius = amoint * _middleRigCameraDistanseOffset;
            _freeLookCamera.m_Orbits[2].m_Radius = amoint * _topAndBottomRigCameraDistanseOffset;

            _transposer.m_CameraDistance = amoint * _lockOnCameraDistanseOffset;
        }
    }
}
