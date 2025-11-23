using System.Linq;
using UnityEngine;

public class LootSouls : MonoBehaviour, ISaveable
{
    [SerializeField] private LootType _lootType;

    [SerializeField]
    [Tooltip("Money count")]
    private int _moneyCount = 10000;

    public bool ShowSoulAmount => _lootType == LootType.Soul;
    public bool ShowMoneyAmount => _lootType == LootType.Money;

    [SerializeField] private string _TutCluetextString;

    private bool _nearLoot;
    private bool _isCollected = false;

    private TutorialClueCont _tutClueCont;
    private MoneyCont _moneyCont;
    private PlayerController _playerController;

    public void Initialize(BootStrap bootStrap)
    {
        _tutClueCont = bootStrap.Resolve<TutorialClueCont>();
        _moneyCont = bootStrap.Resolve<MoneyCont>();
        _playerController = bootStrap.Resolve<PlayerController>();
    }

    public void SaveTo(GameData gameData)
    {
        var lootData = gameData.lootSouls.FirstOrDefault(e => e.lootID == gameObject.name);

        if (lootData == null)
        {
            lootData = new LootData();
            gameData.lootSouls.Add(lootData);
        }

        lootData.lootID = gameObject.name;
        lootData.isCollected = _isCollected;
    }

    public void LoadFrom(GameData gameData)
    {
        foreach (LootData lootdata in gameData.lootSouls)
        {
            if (lootdata.lootID == gameObject.name)
            {
                if (lootdata.isCollected)
                {
                    _isCollected = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        if (_nearLoot)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (_lootType)
                {
                    case LootType.Soul:
                        _moneyCont.GetSouls(1);
                        break;

                    case LootType.Money:
                        _moneyCont.GetMoney(_moneyCount);
                        break;
                }
                _tutClueCont.TutorialGetUnvisible();
                _isCollected = true;
                gameObject.SetActive(false);
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

public enum LootType
{
    Soul,
    Money
}
