using System.Collections.Generic;
using UnityEngine;

public class GaragePlace : MonoBehaviour
{
    [SerializeField] public CarDataBase _carDataBase;
    [SerializeField] private Transform[] _placePositions;
  
    private List<CarDataSO> _carListData;
    private int index = 0;
 
    private void Awake()
    {
        _carListData = new List<CarDataSO>();
        _carListData = _carDataBase.cars;

        foreach (var item in _carListData)
        {
            var go = Instantiate(item.CarPrefab);
            go.Init(item.Speed, item.Gasoline, item.Capacity, item.ID);
            go.transform.position = _placePositions[index].position;
            index++;
        }    
    }
   
 
}



