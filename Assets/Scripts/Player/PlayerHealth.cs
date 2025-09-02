using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("BarsUI")]
    [SerializeField] private Image _mainHealthValueImage;
    [SerializeField] private Image _mediumHealthValueImage;
    private float _maxValue;
    private float _value;

    [Header("BarsSpeeds")]
    [SerializeField] private float _mainBarSpeed;
    [SerializeField] private float _mediumBarSpeed;

    [Header("Animators")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _enemyAnimator;

    private PlayerController _playerController;

    private Coroutine _drawHealthBarCorutine;

    private bool _inHyperarmor = false;
    private bool _invulnerability = false;

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
            //PlayerIsDead();
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
        _value = _maxValue;
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

    private void StartInvulnerability() //called by events in animations, frames in roll animation
    {
        _invulnerability = true;
    }
    
    private void EndInvulnerability() //called by events in animations
    {
        _invulnerability = false;
    }
}
