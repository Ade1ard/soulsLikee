using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _value = 100;
    private float _maxValue;
    [SerializeField] private RectTransform _healthValueAnchor_Max_X;

    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameoverscreen;

    void Start()
    {
        _maxValue = _value;

        DrawHealtBar();
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

        if (_value <= 0)
        {
            //PlayerIsDead();
        }

        DrawHealtBar();
    }

    private void DrawHealtBar()
    {
        _healthValueAnchor_Max_X.anchorMax = new Vector2(_value / _maxValue, 1);
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
        DrawHealtBar();
    }

    public void AddMaxHealth(float amount)
    {
        _maxValue += Mathf.Abs(amount);
        DrawHealtBar();
    }
}
