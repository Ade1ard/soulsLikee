using System;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    [SerializeField] CanvasGroup _messageUI;
    [SerializeField] TextMeshProUGUI _messageText;

    private UIFader _uiFader;
    private Coroutine _coroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
    }

    void Start()
    {
        _uiFader.Fade(_messageUI, false);
    }

    public void MessageGetVisible(String _string)
    {
        _messageText.text = _string;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(_uiFader.Fading(_messageUI, true));
    }

    public void MessageGetUnvisible()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(_uiFader.Fading(_messageUI, false));
    }
}
