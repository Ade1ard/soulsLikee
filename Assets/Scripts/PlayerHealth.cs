using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _value = 100;
    private float _maxValue;
    [SerializeField] private Image _mainHealthValueImage;
    [SerializeField] private Image _mediumHealthValueImage;
    [SerializeField] private float _mainBarSpeed;
    [SerializeField] private float _mediumBarSpeed;

    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameoverscreen;

    void Start()
    {
        _maxValue = _value;

        StartCoroutine(DrawHealtBar());
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemySword"))
        {
            DealDamage(Random.Range(30, 40));
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

        StartCoroutine(DrawHealtBar());
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
        StartCoroutine(DrawHealtBar());
    }

    public void AddMaxHealth(float amount)
    {
        _maxValue += Mathf.Abs(amount);
        StartCoroutine(DrawHealtBar());
    }
}
