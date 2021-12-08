using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

public partial class MainData : MonoBehaviour
{
    #region Singleton Init

    private static MainData _instance;
    private static bool isInitialized = false; // A bit faster singleton

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static MainData Instance // Init not in order
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
        _instance = FindObjectOfType<MainData>();
        if (_instance != null)
        {
            _instance.Initialize();
            isInitialized = true;
        }
    }
    #endregion

    public enum RewardType { Level, MiniGame, Offline }

    public static bool isDebug;
    public static Color debugColor;

    [Header("Dev")]
    public bool clearPlayerPrefsOnStart = false;
    public bool isDebugPP;

    [Header("Money")]
    public double money;
    private bool isMoneyChanged = true;
    #region Event MoneyChange
    private bool _isOnMoneyChangeEventInited = false;
    private UnityEvent _onMoneyChangeEvent;
    public UnityEvent OnMoneyChangeEvent
    {
        get
        {
            if (!_isOnMoneyChangeEventInited)
            {
                _onMoneyChangeEvent = new UnityEvent();
                _isOnMoneyChangeEventInited = true;
            }
            return _onMoneyChangeEvent;
        }
    }
    #endregion

    [Header("Level")]
    public int currentLevel;

    [Header("Balance")]
    public float baseLevelReward = 600f;
    public float nextLevelRewardPercent = 0.1f;
    public float baseMiniGameReward = 600f;
    public float nextMiniGameRewardPercent = 0.1f;

    [Header("ADS")]
    public float interStartDelay;
    public float interIntervalDelay;
    public float afterRewardDelay;

    [Header("Offline Bonus")]
    public int closeTime;
    public int pastCloseTime;
    public int nowTime;
    public int timeToReward;
    public double ips; // Income per second
    public double reward;
    public float offlineIncomeMul;

    [Header("Game Manager")]
    public bool isVibration;
    public bool panelIsOpen;

    public enum GameState
    {
        TapToStartWindow,
        PlayingAnimation,
        PlayingGame,
        LevelComplete,
        LevelFail,
        RewardRecieved,
        HoldInput
    }
    public GameState gameState;
    public bool IsPlaying => gameState == GameState.PlayingAnimation || gameState == GameState.PlayingGame;

    [Header("Dev & Cheats")]
    public bool startWithMoney;
    [Header("Set -1 to disable")]
    public int cheatStartStageId = -1;

    [Header("Upgrade")]
    public int netUpgradeLevel;
    public int earningUpgradeLevel;
    public int speedUpgradeLevel;

    public int maxNetUpgradeLevel;
    public int maxEarningUpgradeLevel;
    public int maxSpeedUpgradeLevel;

    public float netEffect { get { return 0.5f + 0.5f * netUpgradeLevel; } }
    public float moveEffect { get { return 1.6f + 0.1f * speedUpgradeLevel; } }
    public float earningEffect { get { return 30f * (earningUpgradeLevel + 1); } }

    private void Initialize()
    {
        if (isDebug) DLog.D($"MainData.DebugEnabled");

        if (clearPlayerPrefsOnStart)
        {
            Debug.LogError($"Caution! Player Prefs Cleared!");
            ClearGamePP();
        }
        // Load all here
        if (PlayerPrefs.IsShowPlayerPrefs) DLog.D($"PlayerPrefs.DebugEnabled", Color.red);

        if (startWithMoney)
            money = CacheUtil.Get("money", 8000d);
        else
            money = CacheUtil.Get("money", 200d);
        currentLevel = CacheUtil.Get("currentLevel", 0);

        netUpgradeLevel = CacheUtil.Get("amountUpgradeLevel", 0);
        earningUpgradeLevel = CacheUtil.Get("moveUpgradeLevel", 0);
        speedUpgradeLevel = CacheUtil.Get("workUpgradeLevel", 0);


        // if (!PlayerPrefs.HasKey("currentLevel"))
        //    LionStudios.Analytics.Events.LevelStarted(MainData.GetStandartParameters());
    }

    private void Update()
    {
        if (isMoneyChanged)
        {
            OnMoneyChangeEvent.Invoke();
            isMoneyChanged = false;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
            CheatMoney();
#endif
    }

    public void AddMoney(double _value)
    {
        money += _value;
        CacheUtil.Set("money", money);
        isMoneyChanged = true;
    }

    public void AddEarnings()
    {
        var eargnings = earningEffect;
        AddMoney(eargnings);
    }

    public float GetEarningsValue()
    {
        return earningEffect;
    }

    public void OpenOfflinePanel()
    {
        if (isDebug) DLog.D($"OpenOfflinePanel");
        StartCoroutine(OpenOfflinePanelCoroutine());
    }

    private IEnumerator OpenOfflinePanelCoroutine()
    {
        if (PlayerPrefs.HasKey("CloseTime"))
            pastCloseTime = PlayerPrefs.GetInt("CloseTime", 0);
        else
        {
            CacheUtil.Set("CloseTime", 0);
            pastCloseTime = 0;
        }
        nowTime = (int)Utility.DateToTimeUnix();

        yield return new WaitForSeconds(0.4f);

        ips = GetCurrentIPS();
        if (pastCloseTime != 0 && ips != 0)
        {
            timeToReward = nowTime - pastCloseTime;
            if (timeToReward > 120)
                GameManager.Instance.OnEvent_OpenOfflineBonusPanel();
        }
    }

    private double GetCurrentIPS()
    {
        // Station count * passive income per second
        //int stationCount = GetCapturedStationsCount();
        //double stationIncome = MainData.Instance.stationMoney; // Calculated for one
        //float stationFreq = MainData.Instance.stationFreq; // Already with time multi
        //float oneStationIPS = (float)(stationIncome / stationFreq);
        //float allStationIPS = oneStationIPS * stationCount;

        // Train income calculation
        //int trainCount = stationCount; // actually station count
        //double trainMoney = MainData.Instance.trainMoney;
        //float trainFreq = MainData.Instance.trainFreq; // Already with time multi
        //float oneTrainIPS = (float)(trainMoney / trainFreq);
        //float allTrainIPS = oneTrainIPS * trainCount;

        // Time income already taken into account

        // Drill? Cooler? Dont care about them

        // return allStationIPS + allTrainIPS;

        return 0;
    }

    public void CalculateIdleIncome()
    {
    }

    public static Dictionary<string, object> GetStandartParameters()
    {
        Dictionary<string, object> standartParameters = new Dictionary<string, object>();
        //standartParameters.Add("event_timestamp", Utility.DateToTimeUnix());
        standartParameters.Add("Level Number", Instance.currentLevel + 1); // Use +1 for better visualization
        return standartParameters;
    }

    public void TryAddMinigameReward(bool isX3)
    {
        if (isDebug) DLog.D($"TryAddMinigameReward {isX3}");
        if (isX3)
        {
            ShowAds.Instance.ShowRewardAd(() =>
            {
                if (isDebug) DLog.D($"RewardRecieved {isX3}");
                Instance.AddReward(RewardType.MiniGame, 3);
                //GameManager.Instance.OnEvent_LevelRewardRecieved();
            }, "level_reward_x3");
        }
        else
        {
            if (isDebug) DLog.D($"RewardRecieved {isX3}");
            Instance.AddReward(RewardType.MiniGame, 1);
            //GameManager.Instance.OnEvent_LevelRewardRecieved();
        }
    }

    public void TryAddLevelReward(bool isX3)
    {
        if (isDebug) DLog.D($"TryAddLevelReward {isX3}");
        if (isX3)
        {
            ShowAds.Instance.ShowRewardAd(() =>
            {
                if (isDebug) DLog.D($"RewardRecieved {isX3}");
                Instance.AddReward(RewardType.Level, 3);
                GameManager.Instance.OnEvent_LevelRewardRecieved();
            }, "level_reward_x3");
        }
        else
        {
            if (isDebug) DLog.D($"RewardRecieved {isX3}");
            Instance.AddReward(RewardType.Level, 1);
            GameManager.Instance.OnEvent_LevelRewardRecieved();
        }
    }

    public void IncreaceLevel()
    {
        currentLevel++;
        CacheUtil.Set("currentLevel", currentLevel);
        UIManager.Instance.UpdateCurrentLevel(currentLevel);
    }

    private void AddReward(RewardType rewardType, int _multi)
    {
        if (isDebug) DLog.D($"MainData.GetReward({rewardType},{_multi},money:{(rewardType == RewardType.Offline ? GetOfflineRewardValue() * _multi : GetLevelRewardValue() * _multi)}");
        if (rewardType == RewardType.Level)
        {
            AddMoney(GetLevelRewardValue() * _multi);
            // LionStudios.Analytics.Events.LevelComplete(MainData.GetStandartParameters());
            // LionStudios.Analytics.Events.LevelStarted(MainData.GetStandartParameters());
            //FBInit.Instance.LogEvent(GPGevent.level_complete);
        }
        else if (rewardType == RewardType.MiniGame)
        {
            AddMoney(GetMiniGameRewardValue() * _multi);
        }
        else if (rewardType == RewardType.Offline)
        {
            AddMoney(GetOfflineRewardValue() * _multi);
            UIManager.Instance.HideOfflineBonusPanel();
        }
    }

    public double GetRewardText(RewardType _rewardType)
    {
        if (_rewardType == RewardType.Level)
            return GetLevelRewardValue();
        if (_rewardType == RewardType.MiniGame)
            return GetMiniGameRewardValue();
        if (_rewardType == RewardType.Offline)
            return GetOfflineRewardValue();
        return 0;
    }

    public double GetLevelRewardValue()
    {
        return baseLevelReward * (1.0f + currentLevel * nextLevelRewardPercent);
    }

    public double GetMiniGameRewardValue()
    {
        return baseMiniGameReward * (1.0f + currentLevel * nextMiniGameRewardPercent);
    }

    public double GetOfflineRewardValue()
    {
        if (isDebug) DLog.D($"GetOfflineRewardValue");
        pastCloseTime = 0;
        CacheUtil.Set("CloseTime", pastCloseTime);
        if (timeToReward > 18000)
            timeToReward = 18000;
        reward = (double)(ips * timeToReward) * offlineIncomeMul;
        return reward;
    }

    [NaughtyAttributes.Button]
    void CheatMoney()
    {
        AddMoney(100000d);
    }

    [NaughtyAttributes.Button]
#if UNITY_EDITOR
    [MenuItem("Edit/Clear Game PP")]
#endif
    public static void ClearGamePP()
    {
        Debug.Log($"Game PP Cleared");
        PlayerPrefs.DeleteKey($"CloseTime");
        PlayerPrefs.DeleteKey($"currentLevel");
        PlayerPrefs.DeleteKey($"money");
        PlayerPrefs.DeleteKey($"lastExitTime");
        PlayerPrefs.DeleteKey($"level");
        PlayerPrefs.DeleteKey($"freeSpinsCount");
        PlayerPrefs.DeleteKey($"currentGiftId");
        // if (BonusController.Instance != null)
        //     BonusController.Instance.DeletePP();
    }
}

public class PlayerPrefs
{
    public static bool IsShowPlayerPrefs = false;
    public static void DeleteAll()
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.DeleteAll", Color.red);
    }
    public static void SetFloat(string key, float f)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.SetFloat({key},{f})", Color.red);
        UnityEngine.PlayerPrefs.SetFloat(key, f);
    }
    public static void SetInt(string key, int i)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.SetInt({key},{i})", Color.red);
        UnityEngine.PlayerPrefs.SetInt(key, i);
    }
    public static void SetString(string key, string s)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.SetString({key},{s})", Color.red);
        UnityEngine.PlayerPrefs.SetString(key, s);
    }
    public static bool HasKey(string key)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.HasKey({key})", Color.red);
        return UnityEngine.PlayerPrefs.HasKey(key);
    }
    public static int GetInt(string key, int value)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.GetInt({key},{value})", Color.red);
        return UnityEngine.PlayerPrefs.GetInt(key, value);
    }
    public static int GetInt(string key)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.GetInt({key})", Color.red);
        return UnityEngine.PlayerPrefs.GetInt(key);
    }
    public static string GetString(string key, string value)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.GetString({key},{value})", Color.red);
        return UnityEngine.PlayerPrefs.GetString(key, value);
    }
    public static string GetString(string key)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.GetString({key})", Color.red);
        return UnityEngine.PlayerPrefs.GetString(key);
    }
    public static float GetFloat(string key, float value)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.GetFloat({key},{value})", Color.red);
        return UnityEngine.PlayerPrefs.GetFloat(key, value);
    }
    public static float GetFloat(string key)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.GetFloat({key})", Color.red);
        return UnityEngine.PlayerPrefs.GetFloat(key);
    }
    public static void Save()
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.Save()", Color.red);
        UnityEngine.PlayerPrefs.Save();
    }
    public static void DeleteKey(string key)
    {
        if (IsShowPlayerPrefs) DLog.D($"PP.DeleteKey()", Color.red);
        UnityEngine.PlayerPrefs.DeleteKey(key);
    }
}
