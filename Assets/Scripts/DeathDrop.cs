using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    [SerializeField] private string _TutCluetextString;

    private int _dropedMoneyAmount;

    private bool _nearLoot;

    private TutorialClueCont _tutClueCont;
    private MoneyCont _moneyCont;
    private PlayerController _playerController;
    private PlayerDeath _playerDeath;

    public void Initialize(BootStrap bootStrap)
    {
        _tutClueCont = bootStrap.Resolve<TutorialClueCont>();
        _moneyCont = bootStrap.Resolve<MoneyCont>();
        _playerController = bootStrap.Resolve<PlayerController>();
        _playerDeath = bootStrap.Resolve<PlayerDeath>();
    }

    private void Start()
    {
        _dropedMoneyAmount = _moneyCont._targetMoneyCount;
        _moneyCont.SpentMoney(_dropedMoneyAmount);
    }

    void Update()
    {
        if (_nearLoot)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _moneyCont.GetMoney(_dropedMoneyAmount);
                _tutClueCont.TutorialGetUnvisible();
                _playerDeath.ClearDeathDrop();
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
