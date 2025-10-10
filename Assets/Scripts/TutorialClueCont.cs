using System;
using TMPro;
using UnityEngine;

public class TutorialClueCont : MonoBehaviour
{
    [SerializeField] private CanvasGroup _tutorialClue;
    [SerializeField] private TextMeshProUGUI _tutorialClueText;

    private UIFader _uiFader;

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
        _uiFader.Fade(_tutorialClue, true);
    }

    public void TutorialGetUnvisible()
    {
        _uiFader.Fade(_tutorialClue, false);
    }
}
