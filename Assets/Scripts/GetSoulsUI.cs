using System.Collections;
using UnityEngine;

public class GetSoulsUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _getSoulsUI;

    private Coroutine _corutine;
    private UIFader _uiFader;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
    }

    void Start()
    {
        _uiFader.Fade(_getSoulsUI, false);
    }

    public void GetSoulsVisualisation()
    {
        if (_corutine == null)
        {
            _corutine = StartCoroutine(GetSoulsVisible());
        }
    }

    private IEnumerator GetSoulsVisible()
    {
        yield return StartCoroutine(_uiFader.Fading(_getSoulsUI, true));
        yield return new WaitForSeconds(2);
        StartCoroutine(_uiFader.Fading(_getSoulsUI, false));

        _corutine = null;
    }
}
