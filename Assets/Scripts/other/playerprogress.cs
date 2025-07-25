using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerprogress : MonoBehaviour
{
    public TextMeshProUGUI levelValue;
    public RectTransform expValueRectTransform;

    private int _levelValue = 1;
    private float _expCurrentValue = 0;
    public float _expTargetValue = 100;
    void Start()
    {
        DrawUI();
    }

    void Update()
    {
        
    }

    public void AddEXP(float value)
    {
        _expCurrentValue += value;
        if (_expCurrentValue >= _expTargetValue)
        {
            _levelValue += 1;
            _expCurrentValue = 0;
            _expTargetValue = _expTargetValue * 1.5f;
            //GetComponent<PlayerHealth>().value += 25;
            //GetComponent<PlayerHealth>()._maxValue += 25;
            
        }
        DrawUI();
    }

    private void DrawUI()
    {
        expValueRectTransform.anchorMax = new Vector2(_expCurrentValue / _expTargetValue, 1);
        levelValue.text = _levelValue.ToString();
    }

}
