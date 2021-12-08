using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cabinet : MonoBehaviour
{
    private const string FIRST_FOR_FREE = "first for free";
    private const string MANAGER_PRICE = "managerPrice";
    private const string MANAGER_LVL = "managerLvl";
    private const string CAR_PRICE = "carPrice";
    private const string CAR_COUNT = "carCount";
    [SerializeField] private CabinetSO _cabinetSO;
    [Space(10)]
    [Header("UI manager window references")]
    [SerializeField] private Button _managerLvlUpButton;
    [SerializeField] private Button _carBuyingButton;
    [SerializeField] private Text _carBuyingPriceText;
    [SerializeField] private Text _managerLvlText;
    [Space(10)]
    [Header("UI top panel  references")]
    [SerializeField] private Text _carValueText;
    [SerializeField] private Text _managerValueText;
    [Space(10)]
    [Header("Content value")]
    [SerializeField] private int _managerLvl;
    [SerializeField] private int _carsBuyed;
    [Space(10)]
    [Header("Manager")]
    [SerializeField] private float _currentManagerPrice;
    [SerializeField] private int _increaseManagerValue;
    [SerializeField] private int _currentManagerLvl;
    [SerializeField] private GameObject _manager;
    [Header("Car")]
    [SerializeField] private float _currentCarPrice;
    [SerializeField] private int _increaseCarValue;
    [SerializeField] private int _currentCarValue;

    [SerializeField] private Text _managerPriceText;
    [SerializeField] private Text _carPriceText;
    [SerializeField] private ManagerWindow _managerWindow;  

    [SerializeField] CabinetState _state;
    public CabinetState State
    {
        get => _state;
        set => _state = value;
    }   
    private bool IsBoughtFirstManager = false;
    private bool IsBoughtFirstCar = false;
      
    public void Init(int managerLevel, int carsBuyed)
    {
        _state = new CabinetState(managerLevel, carsBuyed);

        Debug.Log(_state.carsBuyed.ToString());
        if (_state.managerLevel <= 0)
            _managerPriceText.text = FIRST_FOR_FREE;
        else
        {
            _managerPriceText.text = _currentManagerPrice.ToString();
            _managerLvlText.text = _state.managerLevel.ToString();
            _managerValueText.text = _state.managerLevel.ToString();
            if (_state.managerLevel > 0)
                ManagerEnable();
        }

        if (_state.carsBuyed <= 0)
            _carPriceText.text = FIRST_FOR_FREE;
        else
        {
            _carPriceText.text = _currentCarPrice.ToString();
            _carBuyingPriceText.text = _state.carsBuyed.ToString();
            _carValueText.text = _state.carsBuyed.ToString();
           // _managerWindow.enableCar.EnableCar();
            DelliveryButtonEnable();
        }
    }
    public void OnBuyManagerClick()
    {
        _state.managerLevel++;
        if (_managerPriceText.text.Contains(FIRST_FOR_FREE))
        {
            IsBoughtFirstManager = true;
            _managerLvlText.text = _state.managerLevel.ToString();
            _managerPriceText.text = _currentManagerPrice.ToString();
        }
        else
        {
            _currentManagerPrice = _currentManagerPrice += _increaseManagerValue * Mathf.Pow(2f, _state.managerLevel);
            _managerLvlText.text = _state.managerLevel.ToString();
            _managerValueText.text = _state.managerLevel.ToString();
            _managerPriceText.text = _currentManagerPrice.ToString();
            Debug.Log($"tretie ---{_managerPriceText.text}");
        }
    }

    public void OnBuyCarClick()
    {
        _state.carsBuyed++;
        if (_carPriceText.text.Contains(FIRST_FOR_FREE))
        {
            IsBoughtFirstCar = true;
            _carBuyingPriceText.text = _state.carsBuyed.ToString();
            _carPriceText.text = _currentCarPrice.ToString();
            //_currentCarValue ++;
        }
        else
        {
            IsBoughtFirstCar = false;
            _currentCarPrice = _currentCarPrice += _increaseCarValue * Mathf.Pow(2f, _state.carsBuyed);
            _carBuyingPriceText.text = _state.carsBuyed.ToString();
            _carValueText.text = _state.carsBuyed.ToString();
            _carPriceText.text = _currentCarPrice.ToString();
            Debug.Log($"tretie ---{_managerPriceText.text}");
        }
    }

    public void OnCloseButtonClick()
    {
        if (_state.managerLevel <= 0)
            Invoke(nameof(ManagerEnable), 1f);         

        if (_state.carsBuyed <= 1 && IsBoughtFirstCar)
        {
          //  _managerWindow.enableCar.EnableCar();
            _managerWindow.IsClickBuyButton = true;

            CloseManagerUIPanel();
            Invoke(nameof(DelliveryButtonEnable), 1.5f);
        }
        else
            CloseManagerUIPanel();
        

    }
    private void CloseManagerUIPanel()
    {
        _managerWindow._windowManager.SetActive(false);
        _managerWindow._openManagerButton.gameObject.SetActive(true);
    }
    private void DelliveryButtonEnable()
    {
        _managerWindow._carMovebutton.gameObject.SetActive(true);
    }
    private void ManagerEnable()
    {
        _manager.SetActive(true);
    }



    private void SaveManagerValues(string price, string lvl)
    {
        PlayerPrefs.SetFloat(price, _currentManagerPrice);
        PlayerPrefs.SetInt(lvl, _currentManagerLvl);
    }
    private void SaveCarValues(string price, string count)
    {
        PlayerPrefs.SetFloat(price, _currentCarPrice);
        PlayerPrefs.SetInt(count, _currentCarValue);
    }
    private void LoadManagerValue(string keyPrice, string keyLvl)
    {
        if (PlayerPrefs.HasKey(keyPrice) && PlayerPrefs.HasKey(keyLvl))
        {
            _currentManagerPrice = PlayerPrefs.GetFloat(keyPrice);
            _currentManagerLvl = PlayerPrefs.GetInt(keyLvl);
        }
    }
    private void LoadCarValues(string keyPrice, string keyCount)
    {
        if (PlayerPrefs.HasKey(keyPrice) && PlayerPrefs.HasKey(keyCount))
        {
            _currentCarPrice = PlayerPrefs.GetFloat(keyPrice);
            _currentCarValue = PlayerPrefs.GetInt(keyCount);
        }
    }

}

