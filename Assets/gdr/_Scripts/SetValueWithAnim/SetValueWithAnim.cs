using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetValueWithAnim : MonoBehaviour
{
    public string valueName;
    public Text valueText;
    public float animSpeed = 0.1f;
    private double currentValue;
    private double curValue;

    private void Start()
    {
        //(valueText.text)
        curValue = 0;
    }

    private void Update()
    {
        //Debug.Log($"CacheUtil.Get = {(CacheUtil.Get(valueName, ""))}, valueText = {(valueText.text)}");
        if (CacheUtil.Get(valueName, 0d) != curValue)
        {
            //Debug.Log($"CacheUtil.Get = {(CacheUtil.Get(valueName, ""))}, valueText = {curValue}");
            if (CacheUtil.Get(valueName, 0d) < curValue)
            {
                if (CacheUtil.Get(valueName, 0d) <= curValue)
                {
                    currentValue = curValue;
                    currentValue -= (curValue - CacheUtil.Get(valueName, 0d)) * (double)((animSpeed * Time.deltaTime));
                }
            }
            else
            {
                if (CacheUtil.Get(valueName, 0d) >= curValue)
                {
                    currentValue = curValue;
                    currentValue += (CacheUtil.Get(valueName, 0d) - curValue) * (double)((animSpeed * Time.deltaTime));
                }
            }

            if (valueText != null)
                valueText.text = Utility.FormatK(currentValue);
            curValue = currentValue;
        }
    }
}
