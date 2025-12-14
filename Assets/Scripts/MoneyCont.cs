using System.Collections;
using UnityEngine;
using TMPro;

public class MoneyCont : MonoBehaviour, ISaveable
{
    [SerializeField] private TextMeshProUGUI _MoneyText;
    [SerializeField] private TextMeshProUGUI _getMoneyCountText;

    [SerializeField] private float _getMoneyFadeSpeed = 5;

    private GetSoulsUI _getSoulsUI;

    private int _currentMoneyCount;
    public int _currentSoulsCount { get; private set; }
    public int _targetMoneyCount { get; private set; }
    private int _getMoneyCount;

    private Coroutine _DrawMoneyCorutine;

    public void Initialize(BootStrap bootStrap)
    {
        _getSoulsUI = bootStrap.Resolve<GetSoulsUI>();
    }

    void Start()
    {
        _getMoneyCountText.color = new Color(_getMoneyCountText.color.r, _getMoneyCountText.color.g, _getMoneyCountText.color.b, 0);
    }

    public void SaveTo(GameData gameData)
    {
        gameData.money = _targetMoneyCount;
        gameData.souls = _currentSoulsCount;
    }

    public void LoadFrom(GameData gameData)
    {
        _currentMoneyCount = gameData.money;
        _targetMoneyCount = gameData.money;
        _currentSoulsCount = gameData.souls;
        _MoneyText.text = _currentMoneyCount.ToString();
    }

    public void GetMoney(int amount)
    {
        _targetMoneyCount += Mathf.Abs(amount);
        _getMoneyCount += Mathf.Abs(amount);

        if (_DrawMoneyCorutine != null)
        {
            StopCoroutine(_DrawMoneyCorutine);
        }
        _DrawMoneyCorutine = StartCoroutine(DrawMoneyCount());
    }

    public void GetSouls(int amount)
    {
        _currentSoulsCount += Mathf.Abs(amount);
        _getSoulsUI.GetSoulsVisualisation();
    }

    private IEnumerator DrawMoneyCount()
    {
        _getMoneyCountText.text = "+" + _getMoneyCount.ToString();

        while (_getMoneyCountText.color.a != 1)
        {
            float newAlpha = Mathf.MoveTowards(_getMoneyCountText.color.a, 1, _getMoneyFadeSpeed * Time.deltaTime);
            _getMoneyCountText.color = new Color(_getMoneyCountText.color.r, _getMoneyCountText.color.g, _getMoneyCountText.color.b, newAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(2);

        int offset = 1;
        int offsetCounter = 0;

        while (_MoneyText.text != _targetMoneyCount.ToString())
        {
            if (offset > _getMoneyCount)
                offset = _getMoneyCount;

            _MoneyText.text = (_currentMoneyCount += offset).ToString();
            _getMoneyCountText.text = "+" + (_getMoneyCount -= offset).ToString();
            offsetCounter++;

            if (offsetCounter >= 20)
            {
                offset++;
                offsetCounter = 0;
            }
            yield return null;
        }

        while (_getMoneyCountText.color.a != 0)
        {
            float newAlpha = Mathf.MoveTowards(_getMoneyCountText.color.a, 0, _getMoneyFadeSpeed * Time.deltaTime);
            _getMoneyCountText.color = new Color(_getMoneyCountText.color.r, _getMoneyCountText.color.g, _getMoneyCountText.color.b, newAlpha);
            yield return null;
        }
    }

    public void SpentMoney(float amount)
    {
        _currentMoneyCount -= Mathf.Abs(Mathf.FloorToInt(amount));
        _targetMoneyCount -= Mathf.Abs(Mathf.FloorToInt(amount));
        _MoneyText.text = _currentMoneyCount.ToString();
    }

    public void SpentSouls(float amount)
    {
        _currentSoulsCount -= Mathf.Abs(Mathf.FloorToInt(amount));
    }
}
