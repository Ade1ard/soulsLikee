using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _HealthValue;
    [SerializeField] private Image _HealthBar;
    [SerializeField] private float _maxValue;
    private float _value;

    [Header("BarSpeed")]
    [SerializeField] private float _healthBarSpeed;
    [SerializeField] private float _barFadeDelay;

    [Header("Animators")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _playerAnimator;

    private Coroutine _drawHealthBarCorutine;

    void Start()
    {
        BarVisible(0f);
        _value = _maxValue;
        
        StartDrawBarCorutine();
    }

    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        BarVisible(1f);
        _value -= Mathf.Abs(damage);
        _value = Mathf.Clamp(_value, 0, _maxValue);
        _animator.SetTrigger("Hit");

        if (_value <= 0)
        {
            EnemyDeath();
        }

        StartDrawBarCorutine();
    }

    private void EnemyDeath()
    {

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

    private IEnumerator DrawHealtBar()
    {
        while (_HealthValue.fillAmount - (_value / _maxValue) > 0.001f)
        {
            _HealthValue.fillAmount = Mathf.Lerp(_HealthValue.fillAmount, _value / _maxValue, _healthBarSpeed);
            yield return null;
        }
        _drawHealthBarCorutine = null;
    }

    private void StartDrawBarCorutine()
    {
        if (_drawHealthBarCorutine == null)
        {
            _drawHealthBarCorutine = StartCoroutine(DrawHealtBar());
        }
    }

    private void BarVisible(float target)
    {
        target = Mathf.Clamp(target, 0, 1);
        _HealthBar.color = new Color(_HealthBar.color.r, _HealthBar.color.g, _HealthBar.color.b, target);
        _HealthValue.color = new Color(_HealthValue.color.r, _HealthValue.color.g, _HealthValue.color.g, target);
    }
}
