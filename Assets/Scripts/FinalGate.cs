using System.Collections;
using UnityEngine;

public class FinalGate : GateOpen
{
    private bool _isBossDead;
    private UIFader _uiFader;
    private TransitionBGCont _bgCont;

    [Header("Game Final Settings")]
    [SerializeField] private CanvasGroup _gamePlayUI;

    public override void Initialize(BootStrap bootStrap)
    {
        base.Initialize(bootStrap);

        _uiFader = bootStrap.Resolve<UIFader>();
        _bgCont = bootStrap.Resolve<TransitionBGCont>();
    }

    public override void OpenGate()
    {
        base.OpenGate();

        StartCoroutine(FinalEffects());
    }

    private IEnumerator FinalEffects()
    {
        _uiFader.Fade(_gamePlayUI, false);
        yield return new WaitForSeconds(1.5f);
        _bgCont.Dissolve(true, 0.015f, 0.0050f);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!_isBossDead)
            return;

        base.OnTriggerEnter(other); 
    }

    public void BossIsDead() { _isBossDead = true; }
}
