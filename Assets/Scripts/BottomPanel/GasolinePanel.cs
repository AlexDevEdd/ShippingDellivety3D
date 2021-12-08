using UnityEngine;
using UnityEngine.UI;

public class GasolinePanel : BottomPanelBase
{
    private const string GASOLINE_PRICE = "gasolinePrice";
    private const string GASOLINE_LVL = "gasolineLvl";

    [Space(10)]
    [Header("Gasoline UI elements")]
    [SerializeField] private Text _gasolinePriceText;
    [SerializeField] private Text _gasolineLvlText;
    [SerializeField] private Button _gasolinePriceUpgradeButton;

    private void Awake()
    {
        _carList = FindObjectsOfType<Car>();
        Load(GASOLINE_PRICE, GASOLINE_LVL);
        SetBottomPanelValues(_gasolinePriceText, _gasolineLvlText);
    }
    private void OnDisable()
    {
        Save(GASOLINE_PRICE, GASOLINE_LVL);
    }
    public void OnUpgradeGasolineClick()
    {
        UpgradePrice(this, _gasolinePriceText);
        UpgradeLvl(this, _gasolineLvlText);

        foreach (var item in _carList)
            item.UpdateCarGasolineData(CurrentLvl);
    }
}

