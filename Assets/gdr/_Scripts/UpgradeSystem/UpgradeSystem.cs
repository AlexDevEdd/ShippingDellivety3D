using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    #region Singleton Init
    private static UpgradeSystem _instance;

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

    public static UpgradeSystem Instance // Init not in order
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
        _instance = FindObjectOfType<UpgradeSystem>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public bool isDebug;
    public Color debugColor;

    [Header("Auto")]
    public Button netUpgradeButton;
    public Button speedUpgradeButton;
    public Button earningUpgradeButton;

    public bool isPanelOpened = false;

    public enum UpgradeType
    {
        Net,
        Speed,
        Earning
    }
    [Header("State")]
    public UpgradeType lastBuyedUpgrade;

    [Header("Auto")]
    public bool isButtonOutlooksInitialized;
    public List<ButtonOutlook> buttonOutlooks;

    #region Event SetActive
    private bool _isOnSetActiveEventInited = false;
    private UnityEvent<bool> _onSetActiveEvent;
    public UnityEvent<bool> OnSetActiveEvent
    {
        get
        {
            if (!_isOnSetActiveEventInited)
            {
                _onSetActiveEvent = new UnityEvent<bool>();
                _isOnSetActiveEventInited = true;
            }
            else
                if (isDebug) DLog.D($"Event {nameof(OnSetActiveEvent)} is called");
            return _onSetActiveEvent;
        }
    }
    #endregion

    void Initialize()
    {
        if (isDebug) DLog.D($"UpgradeSystem.DebugEnabled");
        InitUI();
        // Init data here
        enabled = true;
    }

    private void InitUI()
    {
        if (isDebug) DLog.D($"InitUI");

        // TODO: Init UI HERE
        // Assign from UIManager.Instance
        netUpgradeButton = UIManager.Instance.upgradesPanel.amountUpgradeButton;
        speedUpgradeButton = UIManager.Instance.upgradesPanel.moveUpgradeButton;
        earningUpgradeButton = UIManager.Instance.upgradesPanel.earningsUpgradeButton;

        OnSetActiveEvent.AddListener((x) => UIManager.Instance.upgradesPanel.SetPanelActive(x));

        if (netUpgradeButton != null)
        {
            netUpgradeButton.onClick.AddListener(() =>
            {
                OnClick_UpgradeButton(UpgradeType.Net);
            });
        }
        if (speedUpgradeButton != null)
        {
            speedUpgradeButton.onClick.AddListener(() =>
            {
                OnClick_UpgradeButton(UpgradeType.Speed);
            });
        }
        if (earningUpgradeButton != null)
        {
            earningUpgradeButton.onClick.AddListener(() =>
            {
                OnClick_UpgradeButton(UpgradeType.Earning);
            });
        }
    }

    public void ShowPanel()
    {
        if (isDebug) DLog.D($"Show panel");
        isPanelOpened = true;
        OnSetActiveEvent.Invoke(true);
    }

    public void HidePanel()
    {
        if (isDebug) DLog.D($"Hide panel");
        isPanelOpened = false;
        OnSetActiveEvent.Invoke(false);
    }

    private void Update()
    {
        if (isPanelOpened)
        {
            UpdateButtonOutlooks();
        }
    }

    private void UpdateButtonOutlooks()
    {
        TryInitOutlooks();
        UpdateButtonOutlook(UpgradeType.Net);
        UpdateButtonOutlook(UpgradeType.Speed);
        UpdateButtonOutlook(UpgradeType.Earning);
    }

    private void UpdateButtonOutlook(UpgradeType upgradeType)
    {
        int id = (int)upgradeType;
        var outlook = buttonOutlooks[id];

        int outlookId = 0;
        if (IsMaxLevel(upgradeType))
        {
            outlookId = 2;
            outlook.ChangeCurrentOutlookText(1, $"MAX {GetMaxLevel(upgradeType)}"); ///{GetMaxLevel(upgradeType)}
        }
        else if (!IsEnoughMoney(upgradeType))
        {
            outlookId = 1;
            outlook.ChangeCurrentOutlookText(0, $"{Utility.FormatK(GetUpgradeCost(upgradeType))}");
            outlook.ChangeCurrentOutlookText(1, $"LEVEL {GetLevel(upgradeType) + 1}"); ///{GetMaxLevel(upgradeType)}
        }
        else
        {
            outlook.ChangeCurrentOutlookText(0, $"{Utility.FormatK(GetUpgradeCost(upgradeType))}");
            outlook.ChangeCurrentOutlookText(1, $"LEVEL {GetLevel(upgradeType) + 1}"); ///{GetMaxLevel(upgradeType)}
        }

        outlook.SetOutlook(outlookId);
    }

    private void TryInitOutlooks()
    {
        if (!isButtonOutlooksInitialized)
        {
            buttonOutlooks = new List<ButtonOutlook>();
            buttonOutlooks.Add(netUpgradeButton.GetComponent<ButtonOutlook>());
            buttonOutlooks.Add(speedUpgradeButton.GetComponent<ButtonOutlook>());
            buttonOutlooks.Add(earningUpgradeButton.GetComponent<ButtonOutlook>());
            isButtonOutlooksInitialized = true;
        }
    }

    private void OnClick_UpgradeButton(UpgradeType upgradeType)
    {
        if (isDebug) DLog.D($"OnClick_UpgradeButton {upgradeType}");
        TryBuyUpgrade(upgradeType);
    }

    private void TryBuyUpgrade(UpgradeType upgradeType)
    {
        if (IsCanBuy(upgradeType))
        {
            TakeMoney(upgradeType);
            Upgrade(upgradeType);
            lastBuyedUpgrade = upgradeType;
            GameManager.Instance.OnEvent_UpgradeBuyed();
        }
    }

    private void TakeMoney(UpgradeType upgradeType)
    {
        var upgradeCost = GetUpgradeCost(upgradeType);
        MainData.Instance.AddMoney(-upgradeCost);
    }

    private void Upgrade(UpgradeType upgradeType)
    {
        AddLevel(upgradeType, 1);
    }

    private bool IsCanBuy(UpgradeType upgradeType)
    {
        return IsEnoughMoney(upgradeType) && !IsMaxLevel(upgradeType);
    }

    private bool IsEnoughMoney(UpgradeType upgradeType)
    {
        var upgradeCost = GetUpgradeCost(upgradeType);
        return MainData.Instance.money >= upgradeCost;
    }

    private double GetUpgradeCost(UpgradeType upgradeType)
    {
        return (GetLevel(upgradeType) + 1) * 100 * (1f + GetLevel(upgradeType) * 0.1f);
    }

    private bool IsMaxLevel(UpgradeType upgradeType)
    {
        return GetLevel(upgradeType) + 1 >= GetMaxLevel(upgradeType);
    }

    private void AddLevel(UpgradeType upgradeType, int count)
    {
        switch (upgradeType)
        {
            case UpgradeType.Net:
                MainData.Instance.netUpgradeLevel += count;
                // Also should affect current minigame count
                //MiniGameManager.Instance.AddWorker();
                CacheUtil.Set("netUpgradeLevel", MainData.Instance.netUpgradeLevel);
                break;
            case UpgradeType.Speed:
                MainData.Instance.speedUpgradeLevel += count;
                CacheUtil.Set("speedUpgradeLevel", MainData.Instance.speedUpgradeLevel);
                break;
            case UpgradeType.Earning:
                MainData.Instance.earningUpgradeLevel += count;
                CacheUtil.Set("earningUpgradeLevel", MainData.Instance.earningUpgradeLevel);
                break;
        }
    }

    private int GetLevel(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Net:
                return MainData.Instance.netUpgradeLevel;
            case UpgradeType.Speed:
                return MainData.Instance.speedUpgradeLevel;
            case UpgradeType.Earning:
                return MainData.Instance.earningUpgradeLevel;
            default:
                return 0;
        }
    }

    private int GetMaxLevel(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Net:
                return MainData.Instance.maxNetUpgradeLevel;
            case UpgradeType.Speed:
                return MainData.Instance.maxSpeedUpgradeLevel;
            case UpgradeType.Earning:
                return MainData.Instance.maxEarningUpgradeLevel;
            default:
                return 0;
        }
    }
}
