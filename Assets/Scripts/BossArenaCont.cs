using UnityEngine;

public class BossArenaCont : MonoBehaviour
{
    [Header("BossUI")]
    [SerializeField] private CanvasGroup _bossHealthBar;

    private FogCollider _fogCollider;
    private GameObject _player;
    private UIFader _uiFader;

    private Coroutine _healthBarCoroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _fogCollider = bootStrap.Resolve<FogCollider>();
        _player = bootStrap.Resolve<PlayerController>().gameObject;
        _uiFader = bootStrap.Resolve<UIFader>();
    }

    private void Start()
    {
        _healthBarCoroutine = StartCoroutine(_uiFader.Fading(_bossHealthBar, false));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            _fogCollider.ActivateCollider();

            if (_healthBarCoroutine != null)
                StopCoroutine(_healthBarCoroutine);
            _healthBarCoroutine = StartCoroutine(_uiFader.Fading(_bossHealthBar, true));
        }
    }
}
