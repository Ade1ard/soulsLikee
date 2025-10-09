using UnityEngine;

public class LootSouls : MonoBehaviour
{
    [SerializeField] private string _TutCluetextString;

    private bool _nearLoot;

    private TutorialClueCont _tutClueCont;
    private LevelUpCont _levelUpCont;
    private PlayerController _playerController;

    public void Initialize(BootStrap bootStrap)
    {
        _tutClueCont = bootStrap.Resolve<TutorialClueCont>();
        _levelUpCont = bootStrap.Resolve<LevelUpCont>();
        _playerController = bootStrap.Resolve<PlayerController>();
    }

    void Update()
    {
        if (_nearLoot)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _levelUpCont.GetCurrienciesSouls(1);
                _tutClueCont.TutorialGetUnvisible();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerController.gameObject == other.gameObject)
        {
            _tutClueCont.TutorialGetVisible(_TutCluetextString);
            _nearLoot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playerController.gameObject == other.gameObject)
        {
            _tutClueCont.TutorialGetUnvisible();
            _nearLoot = false;
        }
    }
}
