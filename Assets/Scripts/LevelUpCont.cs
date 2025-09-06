using System.Collections;
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

    private Coroutine _changeColorCorutine;

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
            _OneUpgrateCost = Mathf.Floor(_OneUpgrateCost *= 1.5f);
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
        
        if (_currentMoneyCount >= SumCost && _currentSoulsCount >= _levelWillUpCount + 1)
        {
            _moneyCost = SumCost;
            _moneyCostText.gameObject.SetActive(true);
            _moneyCostText.text = "~" + SumCost.ToString();
            return true;
        }
        else
        {
            if (_currentMoneyCount < SumCost)
            {
                StartChangeColorCorutine(_currentMoneyCountText);
            }
            if (_currentSoulsCount < _levelWillUpCount + 1)
            {
                StartChangeColorCorutine(_currentSoulsCountText);
            }
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

        _OneUpgrateCost = Mathf.Floor(_OneUpgrateCost /= 1.5f);
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

    public void CanselChanges()
    {
        _willBeDamage = _currentDamage;
        _willBeDamageText.text = _currentDamageText.text;

        _willBeHealth = _currentMaxHealth;
        _willBeHealthText.text = _currentHealthText.text;

        _willBeFlaskEfficiency = _currentFlaskEfficiency;
        _willBeFlaskEfficiencyText.text = _currentFlaskEfficiencyText.text;

        while (_levelWillUpCount != 0)
        {
            _OneUpgrateCost /= 1.5f;
            _levelWillUpCount -= 1;
        }
        _moneyCost = 0;

        _soulsCostText.gameObject.SetActive(false);
        _moneyCostText.gameObject.SetActive(false);

    }

    private void UpdateAllValues()
    {
        _playerHealth.GetMaxHealth(_currentMaxHealth);
        _playerSword.GetDamageValue(_currentDamage);
        _Flask.GetFlaskEfficiency(_currentFlaskEfficiency);
    }

    private void StartChangeColorCorutine(TextMeshProUGUI Curriens)
    {
        if (_changeColorCorutine == null)
        {
            _changeColorCorutine = StartCoroutine(NotEnoughtCurriencies(Curriens));
        }
    }

    private IEnumerator NotEnoughtCurriencies(TextMeshProUGUI Curriens)
    {
        while (Curriens.color.g != 0.3f)
        {
            float newAlpha = Mathf.MoveTowards(Curriens.color.g, 0.3f, 5 * Time.deltaTime);
            Curriens.color = new Color(Curriens.color.r, newAlpha, newAlpha, Curriens.color.a);
            yield return null;
        }
        yield return new WaitForSeconds(1);

        while (Curriens.color.g != 1)
        {
            float newAlpha = Mathf.MoveTowards(Curriens.color.g, 1, 5 * Time.deltaTime);
            Curriens.color = new Color(Curriens.color.r, newAlpha, newAlpha, Curriens.color.a);
            yield return null;
        }
        _changeColorCorutine = null;
    }

    public void GetCurrienciesMoney()
    {
        _currentMoneyCountText.text = _moneyCountGamePlayUI.text;
        _currentMoneyCount = int.Parse(_moneyCountGamePlayUI.text);
    }

    public void GetCurrienciesSouls()
    {
        _currentSoulsCount += 1;
        _currentSoulsCountText.text = _currentSoulsCount.ToString();
    }
}
