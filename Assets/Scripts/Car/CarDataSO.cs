using UnityEngine;

[CreateAssetMenu(fileName = "CarDataInfo", menuName = "Gameplay/New CarDataInfo")]
public class CarDataSO : ScriptableObject
{
    [SerializeField] Car _carPrefab;
    [SerializeField] int _id;
    [SerializeField] float _speed;
    [SerializeField] int _gasoline;
    [SerializeField] int _capacity;

    public Car CarPrefab
    {
        get => _carPrefab;
        set => _carPrefab = value;
    }
    public int ID => _id;
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    public int Gasoline
    {
        get => _gasoline;
        set => _gasoline = value;
    }
    public int Capacity
    {
        get => _capacity;
        set => _capacity = value;
    }  
}