using System.Collections;
using UnityEngine;
using TMPro;

public class MoneyCont : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _MoneyText;
    [SerializeField] private TextMeshProUGUI _getMoneyCountText;

    [SerializeField] private float _getMoneyFadeSpeed = 5;

    private int _currentMoneyCount;
    public int _targetMoneyCount { get; private set; }
    private int _getMoneyCount;

    private Coroutine _DrawMoneyCorutine;

    void Start()
    {
        _getMoneyCountText.color = new Color(_getMoneyCountText.color.r, _getMoneyCountText.color.g, _getMoneyCountText.color.b, 0);
    }

    void Update()
    {

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

        while (_MoneyText.text != _targetMoneyCount.ToString())
        {
            _MoneyText.text = (_currentMoneyCount += 1).ToString();
            _getMoneyCountText.text = "+" + (_getMoneyCount -= 1).ToString();
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
}
