using UnityEngine;

[CreateAssetMenu(fileName = "BottomPanelDataInfo", menuName = "Gameplay/New BottomPanelDataInfo")]
public class BottomPanelDataInfo : ScriptableObject
{
    [SerializeField] string _id;
    [SerializeField] private int _newPrice;
    [SerializeField] private int _currentPrice;
    [SerializeField] private int _increaseValue;
    [SerializeField] private int _currentLvl;

    public string ID => _id;
    public int IncreaseValue => _increaseValue;
   
    public int CurrentPrice
    {
        get => _currentPrice;
        set => _currentPrice = value;
    }
    
    public int CurrentLvl
    {
        get => _currentLvl;
        set => _currentLvl = value;
    }
      
    


}

