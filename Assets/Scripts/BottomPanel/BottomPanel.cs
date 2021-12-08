using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
   /* [SerializeField] private BottomPanelDataBase _bottomPanelDataBase;
    [SerializeField] private CarDataBase _carDataBase;

    [Space(10)]
    [Header("Speed UI elements")]
    [SerializeField] private Text _speedPriceText;
    [SerializeField] private Text _speedLvlText;
    [SerializeField] private Button _speedUpgradeButton;

    [Space(10)]
    [Header("Capacity UI elements")]
    [SerializeField] private Text _gasolinePriceText;
    [SerializeField] private Text _gasolinePriceTextLvlText;
    [SerializeField] private Button _gasolinePriceUpgradeButton;

    [Space(10)]
    [Header("Capacity UI elements")]
    [SerializeField] private Text _capacityPriceText;
    [SerializeField] private Text _capacityLvlText;
    [SerializeField] private Button _capacityUpgradeButton;

    private List<BottomPanelDataInfo> _bottomPanelList;
    private List<CarDataSO> _carListData;

    private void Awake()
    {
        _bottomPanelList = new List<BottomPanelDataInfo>();
        _bottomPanelList = _bottomPanelDataBase.bottomPanelList;
      
        _carListData = new List<CarDataSO>();
        _carListData = _carDataBase.cars;

        SetBottomPanelValues();
    }
    private void OnEnable()
    {
        Load();
    }

    private static void Load()
    {
        PlayerPrefs.GetInt("currentPrice");
        PlayerPrefs.GetInt("currentLevel");
    }

    private void OnDisable()
    {
        Save();
    }
    private void Save()
    {
        foreach (var item in _bottomPanelList)
        {
            PlayerPrefs.SetInt("currentPrice", item.CurrentPrice);
            PlayerPrefs.SetInt("currentLevel", item.CurrentLvl);
        }
    }
    public void OnUpgradeSpeedClick()
    {
        foreach (var item in _bottomPanelList)
        {
            if (item.ID.ToString().Contains("speedSO"))
            {
                UpgradePrice(item, _speedPriceText);
                UpgradeLvl(item, _speedLvlText);
                UpdateCarSpeedData();
            }
        };
    }

    public void OnUpgradeGasolineClick()
    {
        foreach (var item in _bottomPanelList)
        {
            if (item.ID.ToString().Contains("gasolineSO"))
            {
                UpgradePrice(item, _gasolinePriceText);
                UpgradeLvl(item, _gasolinePriceTextLvlText);
                UpdateCarGasolineData();
            }
        };
    }
    public void OnUpgradeCapacityClick()
    {
        foreach (var item in _bottomPanelList)
        {
            if (item.ID.ToString().Contains("capacitySO"))
            {
                UpgradePrice(item, _capacityPriceText);
                UpgradeLvl(item, _capacityLvlText);
                UpdateCarCapacityData();
            }
        };
    }
    private void SetBottomPanelValues()
    {
        Debug.Log("Method Set : ");
        foreach (var item in _bottomPanelList)
        {
            if (item.ID.ToString().Contains("speedSO"))
            {
                _speedPriceText.text = item.CurrentPrice.ToString();
                _speedLvlText.text = item.CurrentLvl.ToString();
            }
            else if (item.ID.ToString().Contains("gasolineSO"))
            {
                _gasolinePriceText.text = item.CurrentPrice.ToString();
                _gasolinePriceTextLvlText.text = item.CurrentLvl.ToString();
            }
            else if (item.ID.ToString().Contains("capacitySO"))
            {
                _capacityPriceText.text = item.CurrentPrice.ToString();
                _capacityLvlText.text = item.CurrentLvl.ToString();
            }
        }
    }

    private void UpdateCarSpeedData()
    {
        foreach (var speed in _carListData)
        {
            speed.Speed += 5;
            PlayerPrefs.SetFloat("currentSpeed", speed.Speed);
        }
    }
    private void UpdateCarGasolineData()
    {
        foreach (var gasoline in _carListData)
        {
            gasoline.Gasoline += 5;
            PlayerPrefs.SetInt("currentGasoline", gasoline.Gasoline);
        }
    }
    private void UpdateCarCapacityData()
    {
        foreach (var capacity in _carListData)
        {
            capacity.Capacity += 5;
            PlayerPrefs.SetInt("currentCapacity", capacity.Capacity);
        }
    }

    private void UpgradePrice(BottomPanelDataInfo item, Text text)
    {
        item.CurrentPrice = item.CurrentPrice += item.IncreaseValue;
       // Tween t = DOTween.To(() => text.text, x => text.text = x, item.CurrentPrice.ToString(), 0.5f);
        text.text = item.CurrentPrice.ToString();
        PlayerPrefs.SetInt("currentPrice", item.CurrentPrice);
    }
    private void UpgradeLvl(BottomPanelDataInfo item, Text text)
    {
        item.CurrentLvl = item.CurrentLvl += item.NextLvl;
        text.text = item.CurrentLvl.ToString();
        PlayerPrefs.SetInt("currentLevel", item.CurrentLvl);
    }*/

  /*  private void OnApplicationQuit()
    {
        Save();
    }*/
}

