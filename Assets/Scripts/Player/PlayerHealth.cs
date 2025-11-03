using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, ISaveable, IRebootable
{
    [Header("BarsUI")]
    [SerializeField] private Image _mainHealthValueImage;
    [SerializeField] private Image _mediumHealthValueImage;
    public float _maxValue {  get; private set; }
    public float _value { get; private set; } = 100;

    [Header("BarsSpeeds")]
    [SerializeField] private float _mainBarSpeed;
    [SerializeField] private float _mediumBarSpeed;

    private Animator _playerAnimator;
    private CharacterController _characterController;
    private PlayerDeath _playerDeath;

    private Coroutine _drawHealthBarCorutine;

    private bool _inHyperarmor = false;
    private bool _invulnerability = false;

    private Vector3 _lastRevivePosition;

    public void Initialize(BootStrap bootStrap)
    {
        _playerAnimator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == gameObject.name);
        _characterController = GetComponent<CharacterController>();
        _playerDeath = bootStrap.Resolve<PlayerDeath>();
    }

    private void Start()
    {
        StartDrawBarCorutine();
    }

    public void SaveTo(GameData gameData)
    {
        gameData.PlayerPosotion = gameObject.transform.position;
        gameData.health = _value;
        gameData.RevivePosition = _lastRevivePosition;
    }

    public void LoadFrom(GameData gameData)
    {
        _value = gameData.health;
        _lastRevivePosition = gameData.RevivePosition;
        _characterController.enabled = false;
        gameObject.transform.position = gameData.PlayerPosotion;
        _characterController.enabled = true;
    }

    public void Reboot()
    {
        _value = _maxValue;
        StartDrawBarCorutine();
    }

    public void DealDamage(float damage)
    {
        if (_value > 0)
        {
            _value -= Mathf.Abs(damage);
            _value = Mathf.Clamp(_value, 0, _maxValue);

            if (_value <= 0)
            {
                PlayerIsDead();
            }
            else if (!_inHyperarmor)
            {
                _playerAnimator.SetTrigger("Hit");
            }

            StartDrawBarCorutine();
        }
    }

    private void PlayerIsDead() 
    {
        _playerDeath.Death();
        _invulnerability = false;
    }

    public void AddHealt(float amount)
    {
        _value += Mathf.Abs(amount);
        _value = Mathf.Clamp(_value, 0, _maxValue);
        StartDrawBarCorutine();
    }

    public void GetMaxHealth(float amount)
    {
        _maxValue = Mathf.Abs(amount);
        StartDrawBarCorutine();
    }

    public void Revive()
    {
        _value = _maxValue;
        StartDrawBarCorutine();

        _characterController.enabled = false;
        gameObject.transform.position = _lastRevivePosition;
        _characterController.enabled = true;
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
        while (_mediumHealthValueImage.fillAmount != (_value / _maxValue) + 0.005f)
        {
            _mainHealthValueImage.fillAmount = Mathf.Lerp(_mainHealthValueImage.fillAmount, _value / _maxValue, _mainBarSpeed);

            if (_mainHealthValueImage.fillAmount != (_value / _maxValue) + 0.005f)
            {
                _mediumHealthValueImage.fillAmount = Mathf.Lerp(_mediumHealthValueImage.fillAmount, _mainHealthValueImage.fillAmount, _mediumBarSpeed);
            }
            yield return null;
        }
        _drawHealthBarCorutine = null;
    }

    private void StartHyperArmor() //called by events in animations
    {
        _inHyperarmor = true;
    }

    private void EndHyperArmor() //called by events in animations
    {
        _inHyperarmor = false;
    }

    public bool CheckInvulnerability() { return _invulnerability; }

    public bool CheckAlive() { return _value > 0; }

    private void Invulnerability(int amount) //called by events in animations, frames in roll animation
    {
        _invulnerability = amount == 1? true : false;
    }

    public void SetRevivePosotion(Vector3 pos) { _lastRevivePosition = pos; }
}
