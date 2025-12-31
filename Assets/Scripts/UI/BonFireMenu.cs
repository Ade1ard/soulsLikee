using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonFireMenu : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup BonFireMenuUI;

    private UIFader _uiFader;
    private MenuesController _menuesController;
    private JsonSaveSystem _saveSystem;
    private TransitionBGCont _transitionBGCont;
    private MusicCont _musicCont;

    private Coroutine _SetActiveCoroutine;
    private Coroutine _MainMenuLoadingCoroutine;

    public void Initialize(BootStrap bootStrap)
    {
        _uiFader = bootStrap.Resolve<UIFader>();
        _menuesController = bootStrap.Resolve<MenuesController>();
        _saveSystem = bootStrap.Resolve<JsonSaveSystem>();
        _transitionBGCont = bootStrap.Resolve<TransitionBGCont>();
        _musicCont = bootStrap.Resolve<MusicCont>();
    }

    public void SetActive(bool _bool)
    {
        if (_SetActiveCoroutine != null)
            StopCoroutine(_SetActiveCoroutine);
        _SetActiveCoroutine = StartCoroutine(_uiFader.Fading(BonFireMenuUI, _bool));

        _uiFader.Fade(BonFireMenuUI, _bool);
        if (_bool)
            _menuesController.SetCurrnetMenu(this);
    }

    private BonFireCont _bonFireCont;
    public void GetCurrentBonFire(BonFireCont bonFireCont) => _bonFireCont = bonFireCont;

    public void QuitButton()
    {
        _bonFireCont.QuitBonfire();
        _menuesController.CloseMenu(true);
    }

    public void MainMenuLoad()
    {
        if (_MainMenuLoadingCoroutine == null)
            _MainMenuLoadingCoroutine = StartCoroutine(LoadingMainMenu());
    }

    private IEnumerator LoadingMainMenu()
    {
        Coroutine coroutine1 = _transitionBGCont.Dissolve(true);
        Coroutine coroutine2 = StartCoroutine(_musicCont.FadeCurrentSoundtrec());

        yield return coroutine1;
        yield return coroutine2;

        _menuesController.CloseMenu(true);
        _menuesController.CloseMenu(true);
        _saveSystem.SaveGame();
        SceneManager.LoadScene(0);

        _MainMenuLoadingCoroutine = null;
    }
}
