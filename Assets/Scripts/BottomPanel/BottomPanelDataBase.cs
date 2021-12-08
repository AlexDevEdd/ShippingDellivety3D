using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BottomPanelDataBase", menuName = "Gameplay/New BottomPanelDataBase")]
public class BottomPanelDataBase : ScriptableObject
{
    public List<BottomPanelDataInfo> bottomPanelList;
}

