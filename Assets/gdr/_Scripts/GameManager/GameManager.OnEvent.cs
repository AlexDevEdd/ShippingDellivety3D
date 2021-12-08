using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager // OnEvent
{
    public void OnEvent_EnemyDied()
    {
        if (isDebug) DLog.D($"OnEvent_EnemyDied");
    }

    public void OnEvent_ObstacleDied()
    {
        if (isDebug) DLog.D($"OnEvent_ObstacleDied");
    }

    public void OnEvent_LevelAnimationEnd()
    {
        if (isDebug) DLog.D($"OnEvent_LevelAnimationEnd");
        MainData.Instance.gameState = MainData.GameState.PlayingGame;
    }

    [NaughtyAttributes.Button("Call LevelCompleted")]
    public void OnEvent_LevelCompleted()
    {
        if (isDebug) DLog.D($"OnEvent_LevelCompleted");
        MainData.Instance.gameState = MainData.GameState.LevelComplete;
        // MyFacebook.Instance.LogEvent(GPGevent.level_complete);
        // PlayerSpawner.Instance.DespawnPlayer();
        UIManager.Instance.SetPlayerStatsPanel(true, true, true, false, false);
        if (isDebug) DLog.D($"ExecuteAction wait 1 sec...");
        CoroutineActions.ExecuteAction(1f, () =>
        {
            CameraMover.Instance.Enable();
            // LevelManager.Instance.DestroyCurrentLevel();
            UIManager.Instance.ShowLevelRewardPanel();
        });
    }

    public void OnEvent_PlayerDied()
    {
        if (isDebug) DLog.D($"OnEvent_PlayerDied");
        CoroutineActions.ExecuteAction(2f, () =>
        {
            OnEvent_GameLose();
        });
    }

    public void OnEvent_GameLose()
    {
        if (isDebug) DLog.D($"OnEvent_GameLose");
        MainData.Instance.gameState = MainData.GameState.LevelFail;
        CameraMover.Instance.Enable();
        LevelManager.Instance.DestroyCurrentLevel();
        UIManager.Instance.ShowGameOverPanel();
        UIManager.Instance.SetPlayerStatsPanel(true, true, true, false, true);
    }

    public void OnEvent_ShowLevelRewardPanel()
    {
        if (isDebug) DLog.D($"OnEvent_ShowLevelRewardPanel");
        UIManager.Instance.SetPlayerStatsPanel(false, false, false, false, true);
        UIManager.Instance.ShowLevelRewardPanel();
    }

    public void OnEvent_OpenOfflineBonusPanel()
    {
        if (isDebug) DLog.D($"OpenOfflinePanel");
        UIManager.Instance.ShowOfflineBonusPanel();
    }

    public void OnEvent_StartNextLevel()
    {
        if (isDebug) DLog.D($"OnEvent_StartNextLevel");
        
        UIManager.Instance.SetPlayerStatsPanel(true, true, true, true, false);

        bool isShowTapToStartHere = true;
        if (isShowTapToStartHere)
            UIManager.Instance.ShowTapToStartPanel();
        else
            OnClick_TapToStart(); // Simulate click
    }

    public void OnEvent_AddEarnings(Vector3 workerPos)
    {
        if (isDebug) DLog.D($"OnEvent_AddEarnings");
        EffectsManager.Instance.CreateEarningEffect(workerPos, MainData.Instance.GetEarningsValue());
        MainData.Instance.AddEarnings();
    }

    public void OnEvent_LevelRewardRecieved()
    {
        if (isDebug) DLog.D($"OnEvent_LevelRewardRecieved + IncreaceLevel");
        // UIManager.Instance.SetPlayerStatsPanel(true, true, true, true, true);
        // LevelLocationManager.Instance.ShowLocationDependingOnLevelId();

        // bool isStartLevelRightAfterRecievingReward = false;
        // if (isStartLevelRightAfterRecievingReward)
        //     StartGame();
    }

    public void OnEvent_AllStageEnemiesKilled()
    {
        if (isDebug) DLog.D($"OnEvent_AllStageEnemiesKilled");
        GoToNextStage();
    }
}
