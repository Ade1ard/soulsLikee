using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private CinemachineFreeLook _freeLookCamera;
    [SerializeField] private CinemachineVirtualCamera _lockOnCamera;
    [SerializeField] private CanvasScaler _playerCanvas;
    [SerializeField] private GameObject _gameSettings;
    private CinemachineFramingTransposer _transposer;

    [Header("Scrollbars")]
    [SerializeField] private Scrollbar _scrollbarCameraDist;
    [SerializeField] private Scrollbar _scrollbarCameraSpeed;
    [SerializeField] private Scrollbar _scrollbarUISize;

    private float _UISize = 1.5f;

    private float _freeLookCameraSpeedOffset_X = 1000f;
    private float _freeLookCameraSpeedOffset_Y = 15f;

    private float _lockOnCameraDistanseOffset = 7f;

    private float _topAndBottomRigCameraDistanseOffset = 7f;
    private float _middleRigCameraDistanseOffset = 9f;

    void Start()
    {
        _gameSettings.SetActive(false);
        _transposer = _lockOnCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        
    }

    public void GetUISize()
    {
        float amoint = Mathf.Clamp(_scrollbarUISize.value, 0.3f, 1f);

        _playerCanvas.scaleFactor = _UISize * amoint;
    }

    public void GetCameraSpeed()
    {
        float amoint = Mathf.Clamp(_scrollbarCameraSpeed.value, 0.1f, 1f);

        _freeLookCamera.m_XAxis.m_MaxSpeed = amoint * _freeLookCameraSpeedOffset_X;
        _freeLookCamera.m_YAxis.m_MaxSpeed = amoint * _freeLookCameraSpeedOffset_Y;
    }

    public void GetCameraDistanse()
    {
        float amoint = Mathf.Clamp(_scrollbarCameraDist.value, 0.3f, 1f);

        _freeLookCamera.m_Orbits[0].m_Radius = amoint * _topAndBottomRigCameraDistanseOffset;
        _freeLookCamera.m_Orbits[1].m_Radius = amoint * _middleRigCameraDistanseOffset;
        _freeLookCamera.m_Orbits[2].m_Radius = amoint * _topAndBottomRigCameraDistanseOffset;

        _transposer.m_CameraDistance = amoint * _lockOnCameraDistanseOffset;
    }

    public void GetActive(bool _bool)
    {
        _gameSettings.SetActive(_bool);
    }
}
