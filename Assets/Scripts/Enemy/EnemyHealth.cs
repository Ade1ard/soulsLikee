using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Unity.Mathematics;

public class EnemyHealth : MonoBehaviour, ISaveable, IRebootable
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

    private Animator _animator;

    [Header("LootParameters")]
    [SerializeField] private int _minMoneyDrop;
    [SerializeField] private int _maxMoneyDrop;
    [SerializeField] private float _lootDropChanse = 0.05f;
    [SerializeField] private float _lootDrobDelay = 2;

    [SerializeField] private bool _thisIsBoss;

    private EnemyController _enemyController;
    private DissolveController _dissolveController;
    private CapsuleCollider _capsuleCollider;
    private LootSpawner _lootSpawner;
    private CameraModeChanger _cameraModeChanger;

    private NavMeshAgent _navMeshAgent;

    private Coroutine _drawHealthBarCorutine;
    private Coroutine _VisibleHealthBarCorutine;

    private bool _inHyperarmor = false;
    private bool _isBarVisible = true;

    private Vector3 _startPositon;
    private quaternion _startRotation;

    private float _timeLastHit;

    public void Initialize(BootStrap bootStrap)
    {
        _lootSpawner = bootStrap.Resolve<LootSpawner>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _cameraModeChanger = bootStrap.Resolve<CameraModeChanger>();
        _dissolveController = bootStrap.ResolveAll<DissolveController>().FirstOrDefault(e => e.name == gameObject.name);
        _navMeshAgent = bootStrap.ResolveAll<NavMeshAgent>().FirstOrDefault(e => e.name == gameObject.name);
        _enemyController = bootStrap.ResolveAll<EnemyController>().FirstOrDefault(e => e.name == gameObject.name);
        _animator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == gameObject.name);

        _value = _maxValue;
        _startPositon = transform.position;
        _startRotation = transform.rotation;
    }

    private void Start()
    {
        StartDrawBarCorutine();
        if (!_thisIsBoss)
        {
            _HealthBar.color = new Color(_HealthBar.color.r, _HealthBar.color.g, _HealthBar.color.b, 0);
            _HealthValue.color = new Color(_HealthValue.color.r, _HealthValue.color.g, _HealthValue.color.g, 0);
        }
    }

    public void SaveTo(GameData gameData)
    {
        var enemyData = gameData.enemies.FirstOrDefault(e => e.enemyID == gameObject.name);

        if (enemyData == null)
        {
            enemyData = new EnemyData();
            gameData.enemies.Add(enemyData);
        }

        enemyData.enemyID = gameObject.name;
        enemyData.enemyPosition = transform.position;
        enemyData.health = _value;
        enemyData.isAlive = CheckAlive();
    }

    public void LoadFrom(GameData gameData)
    {
        foreach (EnemyData enemyData in gameData.enemies)
        {
            if (enemyData.enemyID == gameObject.name)
            {
                transform.position = enemyData.enemyPosition;
                if (enemyData.isAlive)
                {
                    AddHealt(enemyData.health);
                }
                else
                {
                    _value = 0;
                    EnemyDeath(false);
                }
                break;
            }
        }
    }

    public void Reboot()
    {
        _enemyController.enabled = true;
        _enemyController.Reboot();
        transform.position = _startPositon;
        transform.rotation = _startRotation;
        _value = _maxValue;

        _animator.Rebind();
        _dissolveController.Reboot();
        _capsuleCollider.isTrigger = false;

        StartDrawBarCorutine();
    }

    void Update()
    {
        if (Time.time - _timeLastHit > _barFadeDelay && !_thisIsBoss)
        {
            SetBarVisible(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (CheckAlive())
        {
            _timeLastHit = Time.time;
            if (!_thisIsBoss)
                SetBarVisible(true);

            _value -= Mathf.Abs(damage);
            _value = Mathf.Clamp(_value, 0, _maxValue);

            _enemyController.PlayerNoticedAfterHit();
            _enemyController.EndAttack();

            if (!_inHyperarmor)
            {
                _animator.SetTrigger("Hit");
            }

            StartHyperArmor();

            if (!CheckAlive())
            {
                EnemyDeath(true);
            }

            StartDrawBarCorutine();
        }
    }

    public void EnemyDeath(bool NeedDropLoot)
    {
        _enemyController.EndAttack();
        _enemyController.enabled = false;
        _navMeshAgent.ResetPath();
        _animator.SetTrigger("Death");
        SetBarVisible(false);
        _dissolveController.Dissolve(NeedDropLoot);
        _capsuleCollider.isTrigger = true;

        if (_cameraModeChanger.CheckLookedEnemy(_enemyController))
            _cameraModeChanger.ChangeCameraLookMod();

        if (NeedDropLoot && !_thisIsBoss)
            Invoke("DropLoot", _lootDrobDelay);
    }

    private void DropLoot()
    {
        _lootSpawner.DropLoot(transform.position, _minMoneyDrop, _maxMoneyDrop, _lootDropChanse);
    }

    public void AddHealt(float amount)
    {
        _value += Mathf.Abs(amount);
        _value = Mathf.Clamp(_value, 0, _maxValue);
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
        while (_HealthValue.fillAmount != (_value / _maxValue))
        {
            _HealthValue.fillAmount = Mathf.MoveTowards(_HealthValue.fillAmount, _value / _maxValue, _healthBarSpeed);
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
                StopCoroutine(_VisibleHealthBarCorutine);
            }
            _VisibleHealthBarCorutine = StartCoroutine(BarVisible(_bool ? 1:0));
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

    public bool CheckAlive() {return _value > 0;}
}
