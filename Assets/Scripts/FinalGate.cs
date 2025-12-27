using System.Collections;
using UnityEngine;

public class FinalGate : GateOpen
{
    private bool _isBossDead;
    private UIFader _uiFader;
    private TransitionBGCont _bgCont;

    [Header("Game Final Settings")]
    [SerializeField] private CanvasGroup _gamePlayUI;
    [SerializeField] private RectTransform _finalFlash;
    [SerializeField] private CanvasGroup _finalText;

    public override void Initialize(BootStrap bootStrap)
    {
        base.Initialize(bootStrap);

        _uiFader = bootStrap.Resolve<UIFader>();
        _bgCont = bootStrap.Resolve<TransitionBGCont>();
    }

    private void Start()
    {
        _uiFader.Fade(_finalText, false, 50);
    }

    public override void OpenGate()
    {
        base.OpenGate();

        StartCoroutine(FinalEffects());
    }

    private IEnumerator FinalEffects()
    {
        _uiFader.Fade(_gamePlayUI, false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(FinalFlash());

        yield return new WaitForSeconds(1.5f);
        _bgCont.Dissolve(true, 0.015f, 0.0050f);

        yield return new WaitForSeconds(3.2f);
        _uiFader.Fade(_finalText, true, 2);
    }

    private IEnumerator FinalFlash()
    {
        while (_finalFlash.lossyScale.x <= 30)
        {
            _finalFlash.localScale = new Vector3(_finalFlash.lossyScale.x + 0.07f, _finalFlash.lossyScale.y + 0.07f, _finalFlash.lossyScale.z + 0.07f);
            yield return null;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!_isBossDead)
            return;

        base.OnTriggerEnter(other); 
    }

    public void BossIsDead() { _isBossDead = true; }
}
