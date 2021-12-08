using System;
using UnityEngine;
using UnityEngine.UI;

public class GarageThirdSlot : MonoBehaviour, IGarageSlot
{
    private Car[] _carList;
    private GarageButton[] _garageButtons;
    private void Start()
    {
        UILevelManager.Instance.OnStartMovingThirdCar += StartMoving;
        _garageButtons = FindObjectsOfType<GarageButton>();
        _carList = FindObjectsOfType<Car>();

    }
    public void CloseDelliveryInfoPanel()
    {
        UILevelManager.Instance.ThirdCardeliveryInfoPanel.SetActive(false);
    }

    public void OpenDelliveryInfoPanel()
    {       
        UILevelManager.Instance.ThirdCardeliveryInfoPanel.SetActive(true);
      
    }

    public void StartMoving()
    {
        UILevelManager.Instance.ThirdStartMovebutton.gameObject.SetActive(false);
        UILevelManager.Instance.ThirdDelliveryInfoButton.gameObject.SetActive(true);
        foreach (var item in _carList)
        {
            if (item.Id == 3)
            {
                item.StartCarMovement();
                Debug.Log("start ID " + item.Id);
            }
        }
    }
}
    

