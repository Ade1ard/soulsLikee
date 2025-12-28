using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalGate : GateOpen
{
    private bool _isBossDead;
    private bool _canLoadMainMenu;
    private UIFader _uiFader;
    private TransitionBGCont _bgCont;
    private SavesManager _savesManager;
    private MenuesController _menuesController;

    private Coroutine _AnyKeyCoroutine;

    [Header("Game Final Settings")]
    [SerializeField] private CanvasGroup _gamePlayUI;
    [SerializeField] private RectTransform _finalFlash;
    [SerializeField] private CanvasGroup _finalText;
    [SerializeField] private CanvasGroup _pressAnyKey;

    public override void Initialize(BootStrap bootStrap)
    {
        base.Initialize(bootStrap);

        _uiFader = bootStrap.Resolve<UIFader>();
        _bgCont = bootStrap.Resolve<TransitionBGCont>();
        _savesManager = bootStrap.Resolve<SavesManager>();
        _menuesController = bootStrap.Resolve<MenuesController>();
    }

    private void Start()
    {
        _uiFader.Fade(_finalText, false, 50);
        AnyKeyActive(false);
    }

    protected override void OpenGate()
    {
        base.OpenGate();

        StartCoroutine(FinalEffects());
    }

    protected override void Update()
    {
        base.Update();

        if (_canLoadMainMenu)
        {
            if (Input.anyKeyDown)
            {
                MainMenuLoad();
            }
        }
    }

    private void MainMenuLoad()
    {
        _menuesController.CloseMenu(true);
        _menuesController.CloseMenu(true);
        _savesManager.DeleteAllSaves();
        SceneManager.LoadScene(0);
    }

    private IEnumerator FinalEffects()
    {
        _uiFader.Fade(_gamePlayUI, false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(FinalFlash());

        yield return new WaitForSeconds(1.3f);
        _bgCont.Dissolve(true, 0.015f, 0.0050f);

        yield return new WaitForSeconds(3.2f);
        _uiFader.Fade(_finalText, true, 2);

        yield return new WaitForSeconds(7f);
        AnyKeyActive(true);
        _canLoadMainMenu = true;
    }

    private IEnumerator FinalFlash()
    {
        while (_finalFlash.lossyScale.x <= 40)
        {
            _finalFlash.localScale = new Vector3(_finalFlash.lossyScale.x + 0.07f, _finalFlash.lossyScale.y + 0.07f, _finalFlash.lossyScale.z + 0.07f);
            yield return null;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!_isBossDead)
            return;

        base.OnTriggerEnter(other); 
    }

    private void AnyKeyActive(bool active)
    {
        if (_AnyKeyCoroutine != null)
            StopCoroutine(_AnyKeyCoroutine);
        _AnyKeyCoroutine = StartCoroutine(_uiFader.Fading(_pressAnyKey, active));
    }

    public void BossIsDead() { _isBossDead = true; }
}
