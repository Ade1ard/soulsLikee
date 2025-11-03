using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] CanvasGroup _gameOverUI;
    [SerializeField] CanvasGroup _gamePlayUI;
    [SerializeField] Volume _globalVolume;
    private Vignette _vignette;

    [Header("Parameters")]
    [SerializeField] float _fadeSpeed = 0.1f;
    [SerializeField] Color _vignetteColor;
    [SerializeField] float _vignetteIntensity;

    private float _defaultVignetteIntensity;
    private Color _defaultVignetteColor;

    private UIFader _uiFader;
    private PlayerController _playerController;
    private PlayerHealth _playerHealth;
    private Animator _playerAnimator;
    private EscapeMenu _escMenu;
    private SceneReboot _sceneReboot;
    private TransitionBGCont _transitionBGCont;
    private CameraModeChanger _cameraModeChanger;

    private bool _canRevive = false;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _playerController = bootStrap.Resolve<PlayerController>();
        _playerHealth = bootStrap.Resolve<PlayerHealth>();
        _playerAnimator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == "Player");
        _escMenu = bootStrap.Resolve<EscapeMenu>();
        _sceneReboot = bootStrap.Resolve<SceneReboot>();
        _transitionBGCont = bootStrap.Resolve<TransitionBGCont>();
        _cameraModeChanger = bootStrap.Resolve<CameraModeChanger>();
    }

    private void Start()
    {
        if (!_globalVolume.profile.TryGet<Vignette>(out _vignette))
            Debug.Log("Vignette not found");
        _defaultVignetteColor = _vignette.color.value;
        _defaultVignetteIntensity = _vignette.intensity.value;
    }

    private void Update()
    {
        if (_canRevive)
        {
            if (Input.anyKeyDown)
            {
                _canRevive = false;
                StartCoroutine(Reviving());
            }
        }
    }

    public void Death()
    {
        _escMenu.InOtherMenu(true);
        if (_cameraModeChanger.IsLoocked())
            _cameraModeChanger.ChangeCameraLookMod();

        _playerController.enabled = false;
        _playerAnimator.SetTrigger("Death");

        _uiFader.Fade(_gamePlayUI, false);
        StartCoroutine(ActivateDeathEffect());
    }

    private void Revive()
    {
        _escMenu.InOtherMenu(false);
        _playerAnimator.SetTrigger("Reboot");
        _playerHealth.Revive();
        _sceneReboot.RebootScene();
        _playerController.enabled = true;

        DeactivateDeathEffects();
    }

    private void DeactivateDeathEffects()
    {
        _uiFader.Fade(_gamePlayUI, true);
        _uiFader.Fade(_gameOverUI, false);

        _vignette.color.value = _defaultVignetteColor;
        _vignette.intensity.value = _defaultVignetteIntensity;
    }

    private IEnumerator ActivateDeathEffect()
    {
        yield return new WaitForSeconds(2);

        _uiFader.Fade(_gameOverUI, true, _fadeSpeed);

        while (_vignette.intensity.value != _vignetteIntensity && Vector4.Distance(_vignette.color.value, _vignetteColor) > 0.05f)
        {
            _vignette.intensity.value = Mathf.MoveTowards(_vignette.intensity.value, _vignetteIntensity, _fadeSpeed * Time.deltaTime);
            _vignette.color.value = Color.Lerp(_vignette.color.value, _vignetteColor, _fadeSpeed * Time.deltaTime);

            yield return null;
        }

        yield return new WaitWhile(() => _gameOverUI.alpha != 1);
        _canRevive = true;
    }

    private IEnumerator Reviving()
    {
        yield return StartCoroutine(_transitionBGCont.Dissolving(true));
        Revive();
        yield return StartCoroutine(_transitionBGCont.Dissolving(false));
    }
}
