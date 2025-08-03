using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaPlayerController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _staminaValueBar;

    [Header("Values")]
    [SerializeField] private float _maxStaminaValue = 100;
    [SerializeField] private float _barSpeed = 0.05f;
    [SerializeField] private float _staminaRecoveryPerSecond = 5;
    [SerializeField] private float _staminaStartRecoveryDelay;
    private float _staminaValue;

    [Header("Coasts")]
    [SerializeField] private float _runCoastPerSecond;
    [SerializeField] private float _rollCoast;
    [SerializeField] private float _atttackCoast;
    [SerializeField] private float _heavyAttackCoast;

    private Dictionary<string, float> MoveCoasts = new Dictionary<string, float>();

    private float _timeLastSpent;

    void Start()
    {
        _staminaValue = _maxStaminaValue;

        MoveCoasts["run"] = _runCoastPerSecond;
        MoveCoasts["roll"] = _rollCoast;
        MoveCoasts["attack"] = _atttackCoast;
        MoveCoasts["heavyAttack"] = _heavyAttackCoast;
    }

    void Update()
    {
        _staminaValueBar.fillAmount = Mathf.Lerp(_staminaValueBar.fillAmount, _staminaValue / _maxStaminaValue, _barSpeed);

        if (Time.time - _timeLastSpent > _staminaStartRecoveryDelay && _staminaValue < _maxStaminaValue)
        {
            _staminaValue += _staminaRecoveryPerSecond * Time.deltaTime;
            _staminaValue = Mathf.Clamp(_staminaValue, 0, _maxStaminaValue);
        }
    }

    public float GetCoast(string coastName)
    {
        return MoveCoasts[coastName];
    }

    public float CheckStamina()
    {
        return _staminaValue;
    }

    public void SpentStamina(float amount)
    {
        _staminaValue -= Mathf.Abs(amount);
        _staminaValue = Mathf.Clamp(_staminaValue, 0, _maxStaminaValue);
        _timeLastSpent = Time.time;
    }

    public void AddMaxStamina(float amount)
    {
        _maxStaminaValue += Mathf.Abs(amount);
    }
}
