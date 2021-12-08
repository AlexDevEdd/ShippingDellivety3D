using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedPanel : BottomPanelBase
{
    private const string SPEED_PRICE = "speedPrice";
    private const string SPEED_LVL = "speedLvl";

    [Space(10)]
    [Header("Speed UI elements")]
    [SerializeField] private Text _speedPriceText;
    [SerializeField] private Text _speedLvlText;
    [SerializeField] private Button _speedUpgradeButton;

    private void Awake()
    {
        _carList = FindObjectsOfType<Car>();
        Load(SPEED_PRICE, SPEED_LVL);
        SetBottomPanelValues(_speedPriceText, _speedLvlText);
    }
  
    public void OnUpgradeSpeedClick()
    {
        UpgradePrice(this, _speedPriceText);
        UpgradeLvl(this, _speedLvlText);

        foreach (var item in _carList)
            item.UpdateCarSpeedData(CurrentLvl);
    }
    private void OnDisable()
    {
        Save(SPEED_PRICE, SPEED_LVL);
    }
}

