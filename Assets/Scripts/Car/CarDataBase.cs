using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarDataBase", menuName = "Gameplay/New CarDataBse")]
public class CarDataBase : ScriptableObject
{
    public List<CarDataSO> cars;
}


