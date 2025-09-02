using UnityEngine;
using TMPro;

public class LevelUpCont : MonoBehaviour
{
    [Header("StartStatsFloats")]
    [SerializeField] private float _currentMaxHealth = 100;
    [SerializeField] private float _currentDamage = 30;
    [SerializeField] private float _currentFlaskEfficiency = 50;

    [SerializeField] private float _OneUpgrateCost = 1200;

    private float _willBeHealth;
    private float _willBeDamage;
    private float _willBeFlaskEfficiency;

    private float _currentMoneyCount;
    private float _currentSoulsCount;

    private float _moneyCost;

    [Header("UILevelUpMenu")]
    [SerializeField] private TextMeshProUGUI _currentSoulsCountText;
    [SerializeField] private TextMeshProUGUI _currentMoneyCountText;

    [SerializeField] private TextMeshProUGUI _moneyCostText;
    [SerializeField] private TextMeshProUGUI _soulsCostText;

    [SerializeField] private TextMeshProUGUI _costValueMoney;

    [SerializeField] private TextMeshProUGUI _currentHealthText;
    [SerializeField] private TextMeshProUGUI _currentDamageText;
    [SerializeField] private TextMeshProUGUI _currentFlaskEfficiencyText;

    [SerializeField] private TextMeshProUGUI _willBeHealthText;
    [SerializeField] private TextMeshProUGUI _willBeDamageText;
    [SerializeField] private TextMeshProUGUI _willBeFlaskEfficiencyText;

    [Header("CurrienciesUI")]
    [SerializeField] private TextMeshProUGUI _moneyCountGamePlayUI;

    private PlayerHealth _playerHealth;
    private PlayerSword _playerSword;
    private Healing _Flask;
    private MoneyCont _moneyCont;

    private int _levelWillUpCount = 0;

    void Start()
    {
        _moneyCont = FindObjectOfType<MoneyCont>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _playerSword = FindObjectOfType<PlayerSword>();
        _Flask = FindObjectOfType<Healing>();

        UpdateAllValues();

        _willBeHealth = _currentMaxHealth;
        _willBeDamage = _currentDamage;
        _willBeFlaskEfficiency = _currentFlaskEfficiency;
    }

    void Update()
    {
        
    }

    public void StatPlus(string statName)
    {
        if (CheckEnoughMoney())
        {
            if (statName == "Health")
            {
                _willBeHealthText.text = (_willBeHealth += 20).ToString();
            }
            else if (statName == "Damage")
            {
                _willBeDamageText.text = (_willBeDamage += 10).ToString();
            }
            else if (statName == "Flask")
            {
                _willBeFlaskEfficiencyText.text = (_willBeFlaskEfficiency += 25).ToString();
            }

            _levelWillUpCount += 1;
            _soulsCostText.gameObject.SetActive(true);
            _soulsCostText.text = "~" + _levelWillUpCount.ToString();
            _OneUpgrateCost *= 1.5f;
        }
    }

    public void StatMinus(string statName)
    {
        if (statName == "Health")
        {
            if (_currentMaxHealth <= _willBeHealth - 20)
            {
                _willBeHealthText.text = (_willBeHealth -= 20).ToString();
                ValuesReset();
            }
        }
        else if (statName == "Damage")
        {
            if (_currentDamage <= _willBeDamage - 10)
            {
                _willBeDamageText.text = (_willBeDamage -= 10).ToString();
                ValuesReset();
            }
        }
        else if (statName == "Flask")
        {
            if (_currentFlaskEfficiency <= _willBeFlaskEfficiency - 25)
            {
                _willBeFlaskEfficiencyText.text = (_willBeFlaskEfficiency -= 25).ToString();
                ValuesReset();
            }
        }
    }

    private bool CheckEnoughMoney()
    {
        float SumCost = _moneyCost + _OneUpgrateCost;

        if (_currentMoneyCount >= SumCost)
        {
            _moneyCost = SumCost;
            _moneyCostText.gameObject.SetActive(true);
            _moneyCostText.text = "~" + SumCost.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ValuesReset()
    {
        _levelWillUpCount -= 1;
        if (_levelWillUpCount == 0)
        {
            _soulsCostText.gameObject.SetActive(false);
        }
        else
        {
            _soulsCostText.text = "~" + _levelWillUpCount.ToString();
        }

        _OneUpgrateCost /= 1.5f;
        if ((_moneyCost -= _OneUpgrateCost) == 0)
        {
            _moneyCostText.gameObject.SetActive(false);
        }
        else
        {
            _moneyCostText.text = _moneyCost.ToString();
        }
    }

    public void ConfirmChanges()
    {
        _currentDamage = _willBeDamage;
        _currentDamageText.text = _willBeDamageText.text;

        _currentMaxHealth = _willBeHealth;
        _currentHealthText.text = _willBeHealthText.text;

        _currentFlaskEfficiency = _willBeFlaskEfficiency;
        _currentFlaskEfficiencyText.text = _willBeFlaskEfficiencyText.text;

        _currentSoulsCount -= _levelWillUpCount;
        _levelWillUpCount = 0;
        _soulsCostText.gameObject.SetActive(false);
        _currentSoulsCountText.text = _currentSoulsCount.ToString();

        _currentMoneyCount -= _moneyCost;
        _moneyCont.SpentMoney(_moneyCost);
        _moneyCost = 0;
        _moneyCostText.gameObject.SetActive(false);
        _currentMoneyCountText.text = _currentMoneyCount.ToString();
        _costValueMoney.text = _OneUpgrateCost.ToString();

        UpdateAllValues();
    }

    private void UpdateAllValues()
    {
        _playerHealth.GetMaxHealth(_currentMaxHealth);
        _playerSword.GetDamageValue(_currentDamage);
        _Flask.GetFlaskEfficiency(_currentFlaskEfficiency);
    }

    public void GetCurrienciesMoney()
    {
        _currentMoneyCountText.text = _moneyCountGamePlayUI.text;
        _currentMoneyCount = int.Parse(_moneyCountGamePlayUI.text);
    }

    public void GetCurrienciesSouls(float amount)
    {
        _currentSoulsCount += Mathf.Abs(amount);
        _currentSoulsCountText.text = _currentSoulsCount.ToString();
    }
}
