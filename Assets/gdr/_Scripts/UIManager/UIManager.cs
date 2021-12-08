using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : GeneratedUI
{
    #region Singleton Init
    private static UIManager _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static UIManager Instance // Init not in order
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
        _instance = FindObjectOfType<UIManager>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    private void Initialize()
    {
        if (isDebug) DLog.D($"UIManager.DebugEnabled");

        tapToStartPanel.tapToPlayButton.onClick.AddListener(() => { GameManager.Instance.OnClick_TapToStart(); });

        levelRewardPanel.levelRewardButton.gameObject.SetActive(false);
        levelRewardPanel.levelRewardButton.onClick.AddListener(() =>
        {
            GameManager.Instance.OnClick_LevelTakeReward(false);
        });
        levelRewardPanel.levelRewardX3Button.onClick.AddListener(() =>
        {
            GameManager.Instance.OnClick_LevelTakeReward(true);
        });

        //levelLosePanel.levelLoseRespawnButton.onClick.AddListener(() => GameManager.Instance.OnClick_Respawn());
        levelLosePanel.levelLoseRestartButton.onClick.AddListener(() => GameManager.Instance.OnClick_Restart());

        enabled = true;
    }

    public void ShowTutorial()
    {
        if (isDebug) DLog.D($"Show Tutorial Panel");
        //GenericUI.Show(tutrPanel.currentAnimator);
    }

    public void HideTutorial()
    {
        if (isDebug) DLog.D($"Hide Tutorial Panel");
        //GenericUI.Hide(tutrPanel.currentAnimator);
        //GameManager.Instance.isTutorialShowing = false;
    }

    public void UpdateCurrentLevel(int _level)
    {
        if (isDebug) DLog.D($"UpdateCurrentLevel");
        /*GenericUI.ChangeText(currentLevelText.Value, $"{_level}");*/
    }

    public void ActivateVibrationSprite(bool _value)
    {
        if (isDebug) DLog.D($"ActivateVibrationSprite {_value}");
        /* GenericUI.SetSprite(vibrationSettingsImage.Value, _value ? vibOnSprite : vibOffSprite); */
    }

    public void ShowOfflineBonusPanel()
    {
        if (isDebug) DLog.D($"ShowOfflineBonusPanel");
        // offlineRewardPanel.ShowPanel();
    }

    public void HideOfflineBonusPanel()
    {
        if (isDebug) DLog.D($"HideOfflineBonusPanel");
        // offlineRewardPanel.HidePanel();
    }

    public void ShowTapToStartPanel()
    {
        if (isDebug) DLog.D($"ShowTapToStartPanel");
        tapToStartPanel.SetPanelActive(true);
        // GenericUI.PlayAnim(tapToStartPanelAnimator.Value, "IsOpen");
    }

    public void HideTapToStartPanel(bool isInstant = false)
    {
        if (isDebug) DLog.D($"HideTapToStartPanel");
        tapToStartPanel.SetPanelActive(false, isInstant);
        // GenericUI.StopAnim(tapToStartPanelAnimator.Value, "IsOpen");
    }


    public void ShowUpgradePanel()
    {
        if (isDebug) DLog.D($"ShowUpgradePanel");
        upgradesPanel.SetPanelActive(true);
        // GenericUI.PlayAnim(tapToStartPanelAnimator.Value, "IsOpen");
    }

    public void HideUpgradePanel(bool isInstant = false)
    {
        if (isDebug) DLog.D($"HideUpgradePanel");
        upgradesPanel.SetPanelActive(false, isInstant);
        // GenericUI.PlayAnim(tapToStartPanelAnimator.Value, "IsOpen");
    }

    public void ShowGameOverPanel()
    {
        if (isDebug) DLog.D($"ShowGameOverPanel");
        levelLosePanel.SetPanelActive(true);
        HideUpgradePanel();
        // gameOverPanel.OpenPanel(_isTemperatureFail);
    }

    public void HideGameOverPanel()
    {
        if (isDebug) DLog.D($"HideGameOverPanel");
        levelLosePanel.SetPanelActive(false);
        // gameOverPanel.OpenPanel(_isTemperatureFail);
    }

    public void ShowLevelRewardPanel(float showNoThanksDelay = 3f)
    {
        if (isDebug) DLog.D($"ShowLevelRewardPanel");
        levelRewardPanel.SetPanelActive(true);
        levelRewardPanel.levelRewardTitleText.text = $"LEVEL {MainData.Instance.currentLevel + 1}";
        levelRewardPanel.levelRewardMoneyText.text = $"{Utility.FormatK(MainData.Instance.GetLevelRewardValue())}";
        levelRewardPanel.levelRewardButton.gameObject.SetActive(false);
        HideUpgradePanel(true);
        CoroutineActions.ExecuteAction(showNoThanksDelay, () =>
        {
            if (levelRewardPanel.current.activeInHierarchy)
            {
                levelRewardPanel.levelRewardButton.gameObject.SetActive(true);
            }
        });
        // levelUpRewardPanel.OpenPanel();
    }

    public void HideLevelRewardPanel(bool isInstant = false)
    {
        if (isDebug) DLog.D($"HideLevelRewardPanel");
        levelRewardPanel.SetPanelActive(false, isInstant);
        // levelUpRewardPanel.OpenPanel();
    }

    public void SetPlayerStatsPanel(bool isEnabled, bool isShowSettings, bool isShowMoney, bool isShowHealth, bool isShowProgress)
    {
        if (isDebug) DLog.D($"SetPlayerStatsPanel {isEnabled},{isShowSettings},{isShowMoney},{isShowHealth},{isShowProgress}");
        playerStatusPanel.SetPanelActive(isEnabled, true);
        playerStatusPanel.settingsArea.SetActive(isShowSettings);
        playerStatusPanel.moneyArea.SetActive(isShowMoney);
        playerStatusPanel.healthArea.SetActive(isShowHealth);
        playerStatusPanel.progressArea.SetActive(isShowProgress);
        if (isShowProgress)
            UpdateProgress();
    }

    public void UpdateProgress()
    {
        bool isFilling = true;
        float targetProgress = 1f; // Place here your target progress
        CoroutineActions.DoActionUntilConditionIsTrue(0, () => isFilling,
            () =>
            {
                playerStatusPanel.progressFill.fillAmount = Mathf.Lerp(playerStatusPanel.progressFill.fillAmount, targetProgress, Time.deltaTime);
                if (Mathf.Abs(targetProgress - playerStatusPanel.progressFill.fillAmount) < 0.01f)
                    isFilling = false;
            });
    }

    public void BlinkDamage()
    {
        if (isDebug) DLog.D($"BlinkDamage");
        playerStatusPanel.playerDamageImage.GetComponent<Animator>().Play("Blink", 0, 0f);
    }

    public void FlipToggle(GameObject toggle, bool _value = true)
    {
        Toggle toggleComponent = toggle.GetComponent<Toggle>();
        if (toggleComponent) toggleComponent.isOn = _value;
    }
}
