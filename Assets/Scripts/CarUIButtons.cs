using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUIButtons : MonoBehaviour
{
    [SerializeField] private Button _moveCarbutton;
    [SerializeField] private Button _delliveryInfoButton;
    [SerializeField] private Slider _serviceSlider;

    private Vector3 screenPos;
    private Vector3 offset = new Vector3(-55, 0, 0);
    private void Start()
    {
        _serviceSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        _moveCarbutton.transform.position = screenPos;    
        _delliveryInfoButton.transform.position = screenPos;
        if(_serviceSlider.IsActive())
        _serviceSlider.transform.position = screenPos + offset;
    }
}
