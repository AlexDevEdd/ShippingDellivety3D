using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager // OnClick
{
    public void OnClick_TapToStart()
    {
        if (isDebug) DLog.D($"OnClick_TapToStart");
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        UIManager.Instance.HideTapToStartPanel(isInstant: true);
        // MyFacebook.Instance.LogEvent(GPGevent.level_start);
        StartGame();
    }

    public void OnClick_LevelTakeReward(bool isX2)
    {
        if (isDebug) DLog.D($"OnClick_LevelTakeRewardX2");

        // Minigame.Current.OnEvent_RewardTake(isX2);

        MainData.Instance.TryAddLevelReward(isX2);

        MainData.Instance.IncreaceLevel();

        MMVibrationManager.Haptic(HapticTypes.MediumImpact);

        UIManager.Instance.HideLevelRewardPanel(true);

        OnEvent_StartNextLevel();

        LevelManager.Instance.DestroyCurrentLevel();

        if (LevelCompleteExplosion.Instance != null)
            LevelCompleteExplosion.Instance.PlayEffect();
    }

    public void OnEvent_UpgradeBuyed()
    {
        if (isDebug) DLog.D($"OnEvent_UpgradeBuyed");
        // if (isDebug) DLog.DPrintAutoColor($"Last upgrade: {UpgradeSystem.Instance.lastBuyedUpgrade}");
        // MyFacebook.Instance.LogEvent(GPGevent.upgrade, UpgradeSystem.Instance.lastBuyedUpgrade.ToString());
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }

    public void OnClick_ChangeVibrationState()
    {
        if (isDebug) DLog.D($"OnClick_ChangeVibrationState");
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        MainData.Instance.isVibration = !MainData.Instance.isVibration;
        CacheUtil.Set("isVibration", MainData.Instance.isVibration);
        UIManager.Instance.ActivateVibrationSprite(MainData.Instance.isVibration);
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }

    public void OnClick_LevelComplete(bool isX3) // two methods in one acused this bool to appear
    {
        if (isDebug) DLog.D($"OnClick_LevelComplete {isX3}");
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        MainData.Instance.TryAddLevelReward(isX3);
        UIManager.Instance.HideLevelRewardPanel(isInstant: true);
    }

    public void OnClick_Respawn()
    {
        if (isDebug) DLog.D($"OnClick_Respawn");
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        //ShowAds.Instance.ShowRewardAd(() =>
        //{
        //    //MainData.Instance.gameState = MainData.GameState.PlayingGame;
        //    //UIManager.Instance.HideGameOverPanel();
        //    //CameraMover.Instance.Disable();
        //    //Player.Instance.SetCameraActive(true);
        //    //Player.Instance.Respawn();
        //    //UIManager.Instance.SetPlayerStatsPanel(true, true, true, true);


        //    LevelManager.Instance.DestroyCurrentLevel();
        //    UIManager.Instance.HideGameOverPanel();
        //}, "respawn_ad", () => { });
    }

    private float onClick_RestartDelay = 0f;
    public void OnClick_Restart()
    {
        if (isDebug) DLog.D($"OnClick_Restart");
        if (onClick_RestartDelay > Time.time)
            return;
        onClick_RestartDelay = Time.time + 1f;
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        // LevelManager.Instance.DestroyCurrentLevel();
        UIManager.Instance.HideGameOverPanel();
        // MainData.Instance.currentLevel = 0;
        RestartCurrentLevel();
    }
}
