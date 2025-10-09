using Cinemachine;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : ISaveable
{
    [Header("Objects")]
    [SerializeField] private GameObject _gameSettingsUI;
    private CinemachineFreeLook _freeLookCamera;
    private CanvasScaler _playerCanvas;
    private CinemachineFramingTransposer _transposer;

    [Header("Scrollbars")]
    public Scrollbar _scrollbarCameraDist;
    public Scrollbar _scrollbarCameraSpeed;
    public Scrollbar _scrollbarUISize;

    private float _UISize = 2f;

    private float _freeLookCameraSpeedOffset_X = 1000f;
    private float _freeLookCameraSpeedOffset_Y = 15f;

    private float _lockOnCameraDistanseOffset = 8f;

    private float _topAndBottomRigCameraDistanseOffset = 8f;
    private float _middleRigCameraDistanseOffset = 10f;

    public void SaveTo(GameData data)
    {

    }

    public void LoadFrom(GameData data)
    {

    }

    public void Initialize(BootStrap bootStrap)
    {
        _gameSettingsUI.SetActive(false);
        _transposer = bootStrap.Resolve<CinemachineFramingTransposer>();
        _freeLookCamera = bootStrap.Resolve<CinemachineFreeLook>();
        _playerCanvas = bootStrap.ResolveAll<CanvasScaler>().FirstOrDefault(e => e.name == "Canvas");
    }

    public void GetUISize()
    {
        float amoint = 0.25f + (Mathf.Clamp(_scrollbarUISize.value, 0.1f, 1) - 0.1f) * (0.5f / 0.9f);

        _playerCanvas.scaleFactor = _UISize * amoint;
    }

    public void GetCameraSpeed()
    {
        float amoint = 0.25f + (Mathf.Clamp(_scrollbarCameraSpeed.value, 0.1f, 1) - 0.1f) * (0.5f / 0.9f);


        _freeLookCamera.m_XAxis.m_MaxSpeed = amoint * _freeLookCameraSpeedOffset_X;
        _freeLookCamera.m_YAxis.m_MaxSpeed = amoint * _freeLookCameraSpeedOffset_Y;
    }

    public void GetCameraDistanse()
    {
        float amoint = 0.25f + (Mathf.Clamp(_scrollbarCameraDist.value, 0.1f, 1) - 0.1f) * (0.5f / 0.9f);

        _freeLookCamera.m_Orbits[0].m_Radius = amoint * _topAndBottomRigCameraDistanseOffset;
        _freeLookCamera.m_Orbits[1].m_Radius = amoint * _middleRigCameraDistanseOffset;
        _freeLookCamera.m_Orbits[2].m_Radius = amoint * _topAndBottomRigCameraDistanseOffset;

        _transposer.m_CameraDistance = amoint * _lockOnCameraDistanseOffset;
    }

    public void GetActive(bool _bool)
    {
        _gameSettingsUI.SetActive(_bool);
    }
}
