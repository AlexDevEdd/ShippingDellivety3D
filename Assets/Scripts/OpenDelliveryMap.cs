using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDelliveryMap : MonoBehaviour
{
    [SerializeField] private Button _openDelliveryMapButton;
    [SerializeField] private Button _closeDelliveryMapButton;
    [SerializeField] private GameObject _mapPanel;

    public void OpenDelliveryMapClick()
    {
        _mapPanel.SetActive(true);
    }
    public void OnCloseButtonClick()
    {
        _mapPanel.SetActive(false);
    }
}
