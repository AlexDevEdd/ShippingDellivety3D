using SWS;
using System;
using UnityEngine;
using UnityEngine.UI;


public class GarageSecondSlot : MonoBehaviour, IGarageSlot
{
    private Car[] _carList;
    private void Start()
    {
        UILevelManager.Instance.OnStartMovingSecondCar += StartMoving;
        _carList = FindObjectsOfType<Car>();

    }
    public void CloseDelliveryInfoPanel()
    {
        UILevelManager.Instance.SecondCardeliveryInfoPanel.SetActive(false);
    }

    public void OpenDelliveryInfoPanel()
    {
        UILevelManager.Instance.SecondCardeliveryInfoPanel.SetActive(true);
    }

    public void StartMoving()
    {
        UILevelManager.Instance.SecondStartMovebutton.gameObject.SetActive(false);
        UILevelManager.Instance.SecondDelliveryInfoButton.gameObject.SetActive(true);
        foreach (var item in _carList)
        {
            if (item.Id == 2)
            {
                item.StartCarMovement();
                Debug.Log("start ID " + item.Id);
            }
        }
    }
}

