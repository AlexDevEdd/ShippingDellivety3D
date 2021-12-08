using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfficeButtonUpgrades : MonoBehaviour
{
    [SerializeField] private Button _button;
    private Vector3 screenPos;

    private void LateUpdate()
    {     
        screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        _button.transform.position = screenPos;
    }
}
