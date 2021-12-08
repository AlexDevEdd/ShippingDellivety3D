using UnityEngine;

public class CabinetSystem : MonoBehaviour
{
    private const string IS_LOADED_KEY = "IS_LOADED_KEY";
    private const string MANAGER_LVL_KEY = "MANAGER_LVL_KEY";
    private const string CARS_LVL_KEY = "CARS_LVL_KEY";

    private static Cabinet _currentCabinet;
    public static Cabinet CurrentCabinet => _currentCabinet;

    private bool IsLoaded;

    private void Awake()
    {
        _currentCabinet = FindObjectOfType<Cabinet>();
    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        IsLoaded = PlayerPrefs.HasKey(IS_LOADED_KEY);
        if (!IsLoaded)
        _currentCabinet.Init(0,0);            
        else
            _currentCabinet.Init(PlayerPrefs.GetInt(MANAGER_LVL_KEY), PlayerPrefs.GetInt(CARS_LVL_KEY));
    }
    private void OnDisable()
    {
        PlayerPrefs.SetInt(IS_LOADED_KEY, 0);
        PlayerPrefs.SetInt(MANAGER_LVL_KEY, _currentCabinet.State.managerLevel);
        PlayerPrefs.SetInt(CARS_LVL_KEY, _currentCabinet.State.carsBuyed);
    }
}

public static class CabinetSettings
{
  
}