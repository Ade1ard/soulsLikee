using UnityEngine;
using System.Collections;
    
public class UIFader : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 5;

    public void Fade(CanvasGroup ui, bool _bool)
    {
        var target = _bool ? 1 : 0;
        StartCoroutine(Fading(ui, target));
    }
    private IEnumerator Fading(CanvasGroup ui, float target)
    {
        while (ui.alpha != target)
        {
            ui.alpha = Mathf.MoveTowards(ui.alpha, target, _fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
