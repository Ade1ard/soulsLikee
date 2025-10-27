using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, ISaveable
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
    private PlayerController _playerController;

    private Coroutine _drawHealthBarCorutine;

    private bool _inHyperarmor = false;
    private bool _invulnerability = false;

    public void Initialize(BootStrap bootStrap)
    {
        _playerController = bootStrap.Resolve<PlayerController>();
        _playerAnimator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == gameObject.name);
    }

    private void Start()
    {
        StartDrawBarCorutine();
    }

    public void SaveTo(GameData gameData)
    {
        gameData.playerPosition = gameObject.transform.position;
        gameData.health = _value;
    }

    public void LoadFrom(GameData gameData)
    {
        _value = gameData.health;
        gameObject.transform.position = gameData.playerPosition;
    }

    public void DealDamage(float damage)
    {
        _value -= Mathf.Abs(damage);
        _value = Mathf.Clamp(_value, 0, _maxValue);

        if (!_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (_inHyperarmor)
            {

            }
            else
            {
                _playerAnimator.SetTrigger("Hit");
            }
        }

        if (_value <= 0 && !_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            PlayerIsDead();
        }

        StartDrawBarCorutine();
    }

    private void PlayerIsDead() 
    {
        _playerAnimator.SetTrigger("Death");
        _playerController.enabled = false;
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

    public bool CheckInvulnerability()
    {
        return _invulnerability;
    }

    private void Invulnerability(int amount) //called by events in animations, frames in roll animation
    {
        _invulnerability = amount == 1? true : false;
    }
}
