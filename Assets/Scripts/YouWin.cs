using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YouWin : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public TextMeshProUGUI timer;

    private float _time;
    private float _timeeee;
    // Start is called before the first frame update
    void Start()
    {
        _time = 180;
    }

    // Update is called once per frame
    void Update()
    {
        _timeeee += 1 * Time.deltaTime;
        if(_timeeee > 1)
        {
            _time -= 1;
            _timeeee = 0;
        }
        timer.text = _time.ToString();

        if(_time == 0)
        {
            Destroy(timer);
            text1.SetActive(true);
            Invoke("Text2Active", 1);
            Invoke("DestroyYouWin", 3);
        }
    }

    private void Text2Active()
    {
        text2.SetActive(true);
    }

    private void DestroyYouWin()
    {
        text2.SetActive(false);
        text1.SetActive(false);
    }
}
