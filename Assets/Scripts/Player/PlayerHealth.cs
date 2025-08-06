using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("BarsUI")]
    [SerializeField] private Image _mainHealthValueImage;
    [SerializeField] private Image _mediumHealthValueImage;
    [SerializeField] private float _maxValue = 100;
    private float _value;

    [Header("BarsSpeeds")]
    [SerializeField] private float _mainBarSpeed;
    [SerializeField] private float _mediumBarSpeed;

    [Header("Animators")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _enemyAnimator;

    private PlayerController _playerController;

    [Header("GlobalUI")]
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameoverscreen;

    private Coroutine _drawHealthBarCorutine;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _value = _maxValue;

        StartDrawBarCorutine();
    }

    void Update()
    {

    }

    public void DealDamage(float damage)
    {
        _value -= Mathf.Abs(damage);
        _value = Mathf.Clamp(_value, 0, _maxValue);

        if (!_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            _playerAnimator.SetTrigger("Hit");
        }

        if (_value <= 0 && !_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            //PlayerIsDead();
        }

        StartDrawBarCorutine();
    }

    private IEnumerator DrawHealtBar()
    {
        while (_mediumHealthValueImage.fillAmount - (_value / _maxValue) > 0.001f)
        {
            _mainHealthValueImage.fillAmount = Mathf.Lerp(_mainHealthValueImage.fillAmount, _value / _maxValue, _mainBarSpeed);

            if (_mainHealthValueImage.fillAmount - (_value / _maxValue) <= 0.001f)
            {
                _mediumHealthValueImage.fillAmount = Mathf.Lerp(_mediumHealthValueImage.fillAmount, _mainHealthValueImage.fillAmount, _mediumBarSpeed);
            }
            yield return null;
        }
        _drawHealthBarCorutine = null;
    }

    private void PlayerIsDead() 
    {
        _playerAnimator.SetTrigger("Death");
        _playerController.enabled = false;

        //_gameplayUI.gameObject.SetActive(false);
        //_gameoverscreen.gameObject.SetActive(true);
    }

    public void AddHealt(float amount)
    {
        _value += Mathf.Abs(amount);
        _value = Mathf.Clamp(_value, 0, _maxValue);
        StartDrawBarCorutine();
    }

    public void AddMaxHealth(float amount)
    {
        _maxValue += Mathf.Abs(amount);
        StartDrawBarCorutine();
    }

    private void StartDrawBarCorutine()
    {
        if (_drawHealthBarCorutine == null)
        {
            _drawHealthBarCorutine = StartCoroutine(DrawHealtBar());
        }
    }
}
