using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyHealth : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _HealthValue;
    [SerializeField] private Image _HealthBar;
    [SerializeField] private float _maxValue;
    public float maxValue => _maxValue;
    public float _value { get; private set; }

    [Header("BarSpeed")]
    [SerializeField] private float _healthBarSpeed;
    [SerializeField] private float _healthBarFadeSpeed;
    [SerializeField] private float _barFadeDelay;

    private Animator _animator;

    [Header("Floats")]
    [SerializeField] private int _minMoneyDrop;
    [SerializeField] private int _maxMoneyDrop;
    [SerializeField] private float _lootDropChanse = 0.05f;
    [SerializeField] private float _lootDrobDelay = 2;

    [Header("LootPrefab")]
    [SerializeField] LootSouls _lootSouls;

    private EnemyController _enemyController;
    private DissolveController _dissolveController;
    private CapsuleCollider _capsuleCollider;
    private MoneyCont _moneyCont;

    private NavMeshAgent _navMeshAgent;

    private Coroutine _drawHealthBarCorutine;
    private Coroutine _VisibleHealthBarCorutine;

    private bool _isDead = false;
    private bool _inHyperarmor = false;
    private bool _isBarVisible = true;

    private float _timeLastHit;

    public void Initialize(BootStrap bootStrap)
    {
        _moneyCont = bootStrap.Resolve<MoneyCont>();
        _capsuleCollider = bootStrap.ResolveAll<CapsuleCollider>().FirstOrDefault(e => e.name == gameObject.name);
        _dissolveController = bootStrap.ResolveAll<DissolveController>().FirstOrDefault(e => e.name == gameObject.name);
        _navMeshAgent = bootStrap.ResolveAll<NavMeshAgent>().FirstOrDefault(e => e.name == gameObject.name);
        _enemyController = bootStrap.ResolveAll<EnemyController>().FirstOrDefault(e => e.name == gameObject.name);
        _animator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == gameObject.name);
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

            _enemyController.PlayerNoticedAfterHit();

            if (_inHyperarmor)
            {

            }
            else
            {
                _animator.SetTrigger("Hit");
            }

            if (_value <= 0 && !_isDead)
            {
                EnemyDeath(true);
            }

            StartDrawBarCorutine();
        }
    }

    public void EnemyDeath(bool _bool)
    {
        _isDead = true;
        _navMeshAgent.ResetPath();
        _enemyController.enabled = false;
        _animator.SetTrigger("Death");
        SetBarVisible(false);
        _dissolveController.Dissolve();
        _capsuleCollider.isTrigger = true;

        if (_bool)
        {
            Invoke("DropLoot", _lootDrobDelay); 
        }
    }

    private void DropLoot()
    {
        _moneyCont.GetMoney(Random.Range(_minMoneyDrop, _maxMoneyDrop));
        if (Random.value <= _lootDropChanse)
        {
            var DropPosition = transform.position;
            DropPosition.y = 0;
            Instantiate(_lootSouls, DropPosition, Quaternion.identity);
        }

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
            if (_VisibleHealthBarCorutine != null)
            {
                StopCoroutine(BarVisible(0));
            }
            StartCoroutine(BarVisible(_bool ? 1:0));
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

    private void StartHyperArmor() //called by events in animations
    {
        _inHyperarmor = true;
    }

    private void EndHyperArmor() //called by events in animations
    {
        _inHyperarmor = false;
    }

        public bool CheckAlive()
    {
        return !_animator.GetCurrentAnimatorStateInfo(0).IsName("Death");
    }
}
