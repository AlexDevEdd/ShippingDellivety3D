using System;
using UnityEngine;

[System.Serializable]
public class CabinetData
{
   [SerializeField] private int managerLevelMax;
   [SerializeField] int carsBuyedMax;
    public int ManagerLevelMax => managerLevelMax;
    
    public int CarsBuyedMax => carsBuyedMax;
    
}

