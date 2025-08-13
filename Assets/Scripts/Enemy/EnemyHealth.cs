using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _HealthValue;
    [SerializeField] private Image _HealthBar;
    [SerializeField] private float _maxValue;
    private float _value;

    [Header("BarSpeed")]
    [SerializeField] private float _healthBarSpeed;
    [SerializeField] private float _healthBarFadeSpeed;
    [SerializeField] private float _barFadeDelay;

    [Header("Animators")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _playerAnimator;

    private EnemyController _enemyController;

    private NavMeshAgent _navMeshAgent;

    private Coroutine _drawHealthBarCorutine;
    private Coroutine _VisibleHealthBarCorutine;

    private bool _isDead = false;
    private bool _inHyperarmor = false;
    private bool _isBarVisible = true;

    private float _timeLastHit;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyController = GetComponent<EnemyController>();
        SetBarVisible(false);
        _value = _maxValue;
        
        StartDrawBarCorutine();
    }

    void Update()
    {
        if (Time.time - _timeLastHit > _barFadeDelay)
        {
            SetBarVisible(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_isDead)
        {
            _timeLastHit = Time.time;
            SetBarVisible(true);

            _value -= Mathf.Abs(damage);
            _value = Mathf.Clamp(_value, 0, _maxValue);

            if (_inHyperarmor)
            {

            }
            else
            {
                _animator.SetTrigger("Hit");
            }

            if (_value <= 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                EnemyDeath();
            }

            StartDrawBarCorutine();
        }
    }

    private void EnemyDeath()
    {
        _animator.SetTrigger("Death");
        _enemyController.enabled = false;
        _navMeshAgent.ResetPath();
        SetBarVisible(false);
        _isDead = true;
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

    private IEnumerator DrawHealtBar()
    {
        while (_HealthValue.fillAmount - (_value / _maxValue) > 0.001f)
        {
            _HealthValue.fillAmount = Mathf.Lerp(_HealthValue.fillAmount, _value / _maxValue, _healthBarSpeed);
            yield return null;
        }
        _drawHealthBarCorutine = null;
    }

    private void SetBarVisible(bool _bool)
    {
        if (_bool != _isBarVisible)
        {
            _isBarVisible = _bool;
            if (_VisibleHealthBarCorutine == null)
            {
                _VisibleHealthBarCorutine = StartCoroutine(BarVisible(_bool ? 1 : 0));
            }
        }
    }

    private IEnumerator BarVisible(float amount)
    {
        while (_HealthBar.color.a != amount)
        {
            float newAlpha = Mathf.MoveTowards(_HealthBar.color.a, amount, _healthBarFadeSpeed * Time.deltaTime);
            _HealthBar.color = new Color(_HealthBar.color.r, _HealthBar.color.g, _HealthBar.color.b, newAlpha);
            _HealthValue.color = new Color(_HealthValue.color.r, _HealthValue.color.g, _HealthValue.color.g, newAlpha);
            yield return null;
        }
        _VisibleHealthBarCorutine = null;
    }

    private void StartAttack() //called by events in animations
    {
        _inHyperarmor = true;
    }

    private void EndAttack() //called by events in animations
    {
        _inHyperarmor = false;
    }
}
