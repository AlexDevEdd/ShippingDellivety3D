using UnityEngine;
using UnityEngine.UI;

public class BottomPanelBase : MonoBehaviour
{
    [Space(10)]
    [Header("Base values")]
    [SerializeField] string _id;
    [SerializeField] private float _currentPrice =100;
    [SerializeField] private int _baseValue = 20;
    [SerializeField] private int _currentLvl = 0;

    protected Car[] _carList;
    
    public string ID => _id;
    public int BaseValue => _baseValue;
    public float CurrentPrice
    {
        get => _currentPrice;
        set => _currentPrice = value;
    }

    public int CurrentLvl
    {
        get => _currentLvl;
        set => _currentLvl = value;
    }

    protected void SetBottomPanelValues(Text price, Text lvl)
    {      
        if (price == null && lvl == null)
            return;
        else
        {
            price.text = CurrentPrice.ToString();
            lvl.text = CurrentLvl.ToString();
        }
    }

    protected void UpgradePrice(BottomPanelBase item, Text text)
    {
        var lvlMul = Mathf.Pow(2f, item.CurrentLvl);
        // item.CurrentPrice = 50;
        item.CurrentPrice += item.BaseValue * lvlMul;      
        // Tween t = DOTween.To(() => text.text, x => text.text = x, item.CurrentPrice.ToString(), 0.5f);
        text.text = item.CurrentPrice.ToString();
        Debug.Log($"прайс - {item.CurrentPrice} ///имя - {item.name}");
    }
    protected void UpgradeLvl(BottomPanelBase item, Text text)
    {
        //item.CurrentLvl = 1;
        item.CurrentLvl ++;
        text.text = item.CurrentLvl.ToString();
        Debug.Log($"текущий лвл - {item.CurrentLvl} ///имя - {item.name}");

    }
    protected void Save(string keyPrice, string keyLvl)
    {
        PlayerPrefs.SetFloat(keyPrice, CurrentPrice);
        PlayerPrefs.SetInt(keyLvl, CurrentLvl);
    }

    protected void Load(string keyPrice, string keyLvl)
    {
        if (PlayerPrefs.HasKey(keyPrice) && PlayerPrefs.HasKey(keyLvl))
        {
            CurrentPrice = PlayerPrefs.GetFloat(keyPrice);
            CurrentLvl = PlayerPrefs.GetInt(keyLvl);
        }
    }

}


