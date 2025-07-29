using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("BarsUI")]
    [SerializeField] private Image _mainHealthValueImage;
    [SerializeField] private Image _mediumHealthValueImage;
    [SerializeField] private float _value = 100;
    private float _maxValue;

    [Header("BarsSpeeds")]
    [SerializeField] private float _mainBarSpeed;
    [SerializeField] private float _mediumBarSpeed;

    [Header("Animators")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _enemyAnimator;

    [Header("GlobalUI")]
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameoverscreen;

    private Coroutine _drawHealthBarCorutine;

    void Start()
    {
        _maxValue = _value;

        StartDrawBarCorutine();
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Great Sword Walk") && !_enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Great Sword Idle"))
        {
            if (other.gameObject.CompareTag("EnemySword"))
            {
                DealDamage(Random.Range(20, 40));
            }
        }
    }

    public void DealDamage(float damage)
    {
        _value -= Mathf.Abs(damage);
        _value = Mathf.Clamp(_value, 0, _maxValue);
        _playerAnimator.SetTrigger("Hit");

        if (_value <= 0)
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
        _gameplayUI.gameObject.SetActive(false);
        _gameoverscreen.gameObject.SetActive(true);
        GetComponent<PlayerController>().enabled = false;
    }

    public void addHealt(float amount)
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
