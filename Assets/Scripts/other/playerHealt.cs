using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealt : MonoBehaviour
{
    public float value = 100;
    public RectTransform valueRectTransform;

    public GameObject gameplayUI;
    public GameObject gameoverscreen;

    public float _maxValue;
    // Start is called before the first frame update
    void Start()
    {
        _maxValue = value;


        DrawHealtBar();
    }

    public void DealDamage(float damage)
    {
        value -= damage;

        if (value <= 0)
        {
            PlayerisDead();
        }

        DrawHealtBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawHealtBar()
    {
        valueRectTransform.anchorMax = new Vector2(value / _maxValue, 1);
    }

    private void PlayerisDead()
    {
        gameplayUI.gameObject.SetActive(false);
        gameoverscreen.gameObject.SetActive(true);
        GetComponent<PlayerController>().enabled = false;
        //GetComponent<fireballcaster>().enabled = false;
        GetComponent<CameraRatation>().enabled = false;
    }

    public void addHealt(float amount)
    {
        value += amount;
        value = Mathf.Clamp(value, 0, _maxValue);
        DrawHealtBar();
    }
 
}
