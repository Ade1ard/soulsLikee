using UnityEngine;

public class BossArenaCont : MonoBehaviour, IRebootable
{
    [Header("BossUI")]
    [SerializeField] private CanvasGroup _bossHealthBar;

    private FogCollider _fogCollider;
    private GameObject _player;
    private UIFader _uiFader;
    private FinalGate _gate;
    private MusicCont _musicCont;

    private Coroutine _healthBarCoroutine;

    private bool _bossIsDead;

    public void Initialize(BootStrap bootStrap)
    {
        _fogCollider = bootStrap.Resolve<FogCollider>();
        _player = bootStrap.Resolve<PlayerController>().gameObject;
        _uiFader = bootStrap.Resolve<UIFader>();
        _gate = bootStrap.Resolve<FinalGate>();
        _musicCont = bootStrap.Resolve<MusicCont>();
    }

    private void Start()
    {
        _healthBarCoroutine = StartCoroutine(_uiFader.Fading(_bossHealthBar, false));
    }

    public void BossIsDead()
    {
        _gate.BossIsDead();
        _bossIsDead = true;
        _musicCont.ChangeCurrentSoundtrec(_musicCont._standartSoundtrec);
    }

    public void Reboot()
    {
        if (_healthBarCoroutine != null)
            StopCoroutine(_healthBarCoroutine);
        _healthBarCoroutine = StartCoroutine(_uiFader.Fading(_bossHealthBar, false));

        _fogCollider.ActivateCollider(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player && !_bossIsDead)
        {
            _fogCollider.ActivateCollider(true);

            if (_healthBarCoroutine != null)
                StopCoroutine(_healthBarCoroutine);
            _healthBarCoroutine = StartCoroutine(_uiFader.Fading(_bossHealthBar, true));

            _musicCont.ChangeCurrentSoundtrec(_musicCont._bossFigthSoundtrec);
        }
    }
}
