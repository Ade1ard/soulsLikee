using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GetSoulsUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _getSoulsText;

    private Coroutine _corutine;

    void Start()
    {
        _getSoulsText.color = new Color(_getSoulsText.color.r, _getSoulsText.color.g, _getSoulsText.color.b, 0);
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, 0);
    }

    void Update()
    {
        
    }

    public void GetSoulsVisualisation()
    {
        if (_corutine == null)
        {
            _corutine = StartCoroutine(GetSoulsVisible());
        }
    }

    IEnumerator GetSoulsVisible()
    {
        while (_getSoulsText.color.a != 1)
        {
            float newAlpha = Mathf.MoveTowards(_getSoulsText.color.a, 1, 5 * Time.deltaTime);
            _getSoulsText.color = new Color(_getSoulsText.color.r, _getSoulsText.color.g, _getSoulsText.color.b, newAlpha);
            _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, newAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(2);

        while (_getSoulsText.color.a != 0)
        {
            float newAlpha = Mathf.MoveTowards(_getSoulsText.color.a, 0, 5 * Time.deltaTime);
            _getSoulsText.color = new Color(_getSoulsText.color.r, _getSoulsText.color.g, _getSoulsText.color.b, newAlpha);
            _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, newAlpha);
            yield return null;
        }
        _corutine = null;
    }
}
