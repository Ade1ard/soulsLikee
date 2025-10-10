using UnityEngine;
using System.Collections;
    
public class UIFader : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 5;

    public void Fade(CanvasGroup ui, bool _bool)
    {
        StartCoroutine(Fading(ui, _bool));
    }

    public IEnumerator Fading(CanvasGroup ui, bool _bool)
    {
        if (_bool)
        {
            ui.gameObject.SetActive(true);
        }

        var target = _bool ? 1 : 0;
        while (ui.alpha != target)
        {
            ui.alpha = Mathf.MoveTowards(ui.alpha, target, _fadeSpeed * Time.deltaTime);
            yield return null;
        }
        
        if (!_bool)
        {
            ui.gameObject.SetActive(false);
        }
    }
}
