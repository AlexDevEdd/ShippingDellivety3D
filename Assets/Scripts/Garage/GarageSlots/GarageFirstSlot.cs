using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GarageFirstSlot : MonoBehaviour, IGarageSlot
{
    private Car[] _carList;
    private GarageButton[] _garageButtons;
    private List<Button> _buttonList;
    private void Start()
    {
        _buttonList = new List<Button>();
        UILevelManager.Instance.OnStartMovingFirstCar += StartMoving;
        UILevelManager.Instance.OnOpenDelliveryInfoFirstCar += OpenDelliveryInfoPanel;
        UILevelManager.Instance.OnCloseFirstDelliveryPanel += CloseDelliveryInfoPanel;
       _carList = FindObjectsOfType<Car>();
       _garageButtons = FindObjectsOfType<GarageButton>();
        foreach (var item in _garageButtons)
        {
            item.gameObject.GetComponent<Button>();
            _buttonList.Add(item.GetComponent<Button>());
        }

    }
    public void CloseDelliveryInfoPanel()
    {
        UILevelManager.Instance.FirstCardeliveryInfoPanel.SetActive(false);
        foreach (var item in _buttonList)
        {
            item.interactable = true;
        }
    }

    public void OpenDelliveryInfoPanel()
    {
        UILevelManager.Instance.FirstCardeliveryInfoPanel.SetActive(true);
        foreach (var item in _buttonList)
        {
            item.interactable = false;
        }
    }

    public void StartMoving()
    {
        UILevelManager.Instance.FirstStartMovebutton.gameObject.SetActive(false);
        UILevelManager.Instance.FirstDelliveryInfoButton.gameObject.SetActive(true);
        foreach (var item in _carList)
        {
            if (item.Id == 1)
            {
                item.StartCarMovement();
            Debug.Log("start ID " + item.Id);

            }
        }
     
    }
}

