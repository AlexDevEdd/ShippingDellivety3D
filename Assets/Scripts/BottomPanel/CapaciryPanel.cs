using UnityEngine;
using UnityEngine.UI;

public class CapaciryPanel : BottomPanelBase
{
    private const string CAPACITY_PRICE = "capacityPrice";
    private const string CAPACITY_LVL = "capacityLvl";

    [Space(10)]
    [Header("Capacity UI elements")]
    [SerializeField] private Text _capacityPriceText;
    [SerializeField] private Text _capacityLvlText;
    [SerializeField] private Button _capacityUpgradeButton;

    private void Awake()
    {
        _carList = FindObjectsOfType<Car>();
        Load(CAPACITY_PRICE, CAPACITY_LVL);
        SetBottomPanelValues(_capacityPriceText, _capacityLvlText);
    }
    private void OnDisable()
    {
        Save(CAPACITY_PRICE, CAPACITY_LVL);      
    }
    public void OnUpgradeCapacityClick()
    {
        UpgradePrice(this, _capacityPriceText);
        UpgradeLvl(this, _capacityLvlText);

        foreach (var item in _carList)
            item.UpdateCarCapacityData(CurrentLvl);
    }
}

