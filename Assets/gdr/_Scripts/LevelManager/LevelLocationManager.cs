using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLocationManager : MonoBehaviour
{
    #region Singleton Init
    private static LevelLocationManager _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
        {
            Debug.Log($"Destroying {gameObject.name}, caused by one singleton instance");
            Destroy(gameObject);
        }
    }

    public static LevelLocationManager Instance // Init not in order
    {
        get
        {
            if (_instance == null)
                Init();
            return _instance;
        }
        private set { _instance = value; }
    }

    static void Init() // Init script
    {
        _instance = FindObjectOfType<LevelLocationManager>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    [System.Serializable]
    public class LocationData
    {
        public Vector2 fromToPrice;
        public LevelLocation locationToShow;
    }
    public List<LocationData> locationData;

    void Initialize()
    {
        DisableAllLocations();
        // Init data here
        enabled = true;
    }

    public void ShowLocationDependingOnPlayerItemPrice()
    {
        DisableAllLocations();
        //var price = ItemSystem.Instance.PlayerItem.itemPrice;
        //foreach(var item in locationData)
        //{
        //    if (item.fromToPrice.x <= price && item.fromToPrice.y >= price)
        //    {
        //        item.locationToShow.Activate();
        //        return;
        //    }    
        //}
    }

    public void ShowFirstLocation()
    {
        DisableAllLocations();
        locationData[0].locationToShow.Activate();
    }

    private void DisableAllLocations()
    {
        foreach(var item in locationData)
        {
            item.locationToShow.gameObject.SetActive(false);
        }
    }
}
