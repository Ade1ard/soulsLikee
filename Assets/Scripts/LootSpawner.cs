using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [Header("LootPrefab")]
    [SerializeField] LootSouls _lootSouls;

    private MoneyCont _moneyCont;
    private BootStrap _bootStrap;

    public void Initialize(BootStrap bootStrap)
    {
        _bootStrap = bootStrap;
        _moneyCont = bootStrap.Resolve<MoneyCont>();
    }

    public void DropLoot(Vector3 DropPosition, int minMoneyCount, int maxMoneyCount, float dropChanse)
    {
        _moneyCont.GetMoney(Random.Range(minMoneyCount, maxMoneyCount));
        if (Random.value <= dropChanse)
        {
            DropPosition.y = 0;
            Instantiate(_lootSouls, DropPosition, Quaternion.identity).Initialize(_bootStrap);
        }
    }
}
