using System;
using TMPro;
using UnityEngine;

public class TutorialClueCont : MonoBehaviour
{
    [SerializeField] private CanvasGroup _tutorialClue;
    [SerializeField] private TextMeshProUGUI _tutorialClueText;

    private UIFader _uiFader;
    private Coroutine _coroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
    }

    void Start()
    {
        _uiFader.Fade(_tutorialClue, false);
    }

    public void TutorialGetVisible(String _string)
    {
        _tutorialClueText.text = _string;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(_uiFader.Fading(_tutorialClue, true));
    }

    public void TutorialGetUnvisible()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(_uiFader.Fading(_tutorialClue, false));
    }
}
