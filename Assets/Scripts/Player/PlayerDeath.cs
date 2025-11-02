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
    [SerializeField] float _fadeSpeed = 0.001f;
    [SerializeField] Color _vignetteColor;
    [SerializeField] float _vignetteIntensity;

    private float _defaultVignetteIntensity;
    private Color _defaultVignetteColor;

    private UIFader _uiFader;
    private PlayerController _playerController;
    private Animator _playerAnimator;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _playerController = bootStrap.Resolve<PlayerController>();
        _playerAnimator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == "Player");
    }

    private void Start()
    {
        if (!_globalVolume.profile.TryGet<Vignette>(out _vignette));
            Debug.Log("Vignette not found");
        _defaultVignetteColor = _vignette.color.value;
        _defaultVignetteIntensity = _vignette.intensity.value;

        _uiFader.Fade(_gameOverUI, false, _fadeSpeed);
        StartCoroutine(DeathEffect(false));
    }

    public void Death()
    {
        _playerController.enabled = false;
        _playerAnimator.SetTrigger("Death");

        _uiFader.Fade(_gamePlayUI, false);
        _uiFader.Fade(_gameOverUI, true, _fadeSpeed);

        StartCoroutine(DeathEffect(true));
    }

    private IEnumerator DeathEffect(bool active)
    {
        Color targetColor = active? _vignetteColor : _defaultVignetteColor;
        float targetIntensity = active? _vignetteIntensity : _defaultVignetteIntensity;

        while (_vignette.intensity.value != targetIntensity)
        {
            _vignette.intensity.value = Mathf.MoveTowards(_vignette.intensity.value, targetIntensity, _fadeSpeed * Time.deltaTime);
            _vignette.color.value = Color.Lerp(_vignette.color.value, targetColor, _fadeSpeed * Time.deltaTime);

            yield return null;
        }

        _vignette.color.value = targetColor;
    }
}
