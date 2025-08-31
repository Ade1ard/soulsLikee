using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialClueCont : MonoBehaviour
{
    [SerializeField] private Image _tutorialClueFrame;
    [SerializeField] private TextMeshProUGUI _tutorialClueText;

    [SerializeField] private float _tutorialFadeSpeed = 5;

    private Coroutine _tutCoroutine;

    void Start()
    {
        StartTutCorutine(false);
    }

    void Update()
    {
        
    }

    public void TutorialGetVisible(String _string)
    {
        _tutorialClueText.text = _string;
        StartTutCorutine(true);
    }

    public void TutorialGetUnvisible()
    {
        StartTutCorutine(false);
    }

    private void StartTutCorutine(bool _bool)
    {
        if (_tutCoroutine != null)
        {
            StopCoroutine(TutGetVisible(0));
        }
        _tutCoroutine = StartCoroutine(TutGetVisible(_bool ? 1:0));
    }

    private  IEnumerator TutGetVisible(float amount)
    {
        while (_tutorialClueFrame.color.a != amount)
        {
            float newAlpha = Mathf.MoveTowards(_tutorialClueFrame.color.a, amount, _tutorialFadeSpeed * Time.deltaTime);
            _tutorialClueFrame.color = new Color(_tutorialClueFrame.color.r, _tutorialClueFrame.color.g, _tutorialClueFrame.color.b, newAlpha);
            _tutorialClueText.color = new Color(_tutorialClueText.color.r, _tutorialClueText.color.g, _tutorialClueText.color.b, newAlpha);
            yield return null;
        }
        _tutCoroutine = null;
    }
}
