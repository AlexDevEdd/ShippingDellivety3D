using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MercantMover : MonoBehaviour
{
    #region Singleton Init
    private static MercantMover _instance;

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

    public static MercantMover Instance // Init not in order
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
        _instance = FindObjectOfType<MercantMover>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    private int selectedMercantId = 0;
    public int SelectedMercantId => selectedMercantId;

    public List<List<UnityAction>> moveActions;

    void Initialize()
    {
        if (isDebug) DLog.D($"MercantMover.DebugEnabled");
        moveActions = new List<List<UnityAction>>();
        moveActions.Add(new List<UnityAction>());
        moveActions.Add(new List<UnityAction>());
        moveActions.Add(new List<UnityAction>());
        // Init data here
        enabled = true;
    }

    public void MoveMercantIn(int index)
    {
        if (isDebug) DLog.D($"MoveMercantIn {index}");
        var mercant = MercantSpawner.Instance.mercants[index];
        mercant.transform.position = LevelLocation.Current.enterMercantPoints[0].position;
        mercant.transform.rotation = LevelLocation.Current.enterMercantPoints[0].rotation;

        List<Transform> path = new List<Transform>();
        path.AddRange(LevelLocation.Current.enterMercantPoints);
        path.Add(LevelLocation.Current.targetMercantPoint);
        // mercant.itemInstance.HideSmall();
        // Player.Instance.PlayerItem.HideSmall();
        mercant.PlayWalk();
        MainData.Instance.gameState = MainData.GameState.HoldInput;
        //IndependedMoveSystem.Instance.GoPath(mercant.transform, path, 1f, 7f, () =>
        //{
        //    mercant.PlayExchange();
        //    Player.Instance.PlayExchangeAnimation();
        //    CoroutineActions.ExecuteAction(mercant.TakeOutTime, () =>
        //    {
        //        mercant.itemInstance.ShowSmall();
        //        Player.Instance.PlayerItem.ShowSmall();
        //    });
        //    CoroutineActions.ExecuteAction(mercant.DropTime, () =>
        //    {
        //        mercant.itemInstance.EquipSmall(LevelLocation.Current.botDropZone, true, true);
        //        Player.Instance.PlayerItem.EquipSmall(LevelLocation.Current.playerDropZone, true, true);

        //        if (LevelLocation.Current.bigTr != null)
        //            mercant.itemInstance.EquipBig(LevelLocation.Current.bigTr, true);
        //    });
        //    CoroutineActions.ExecuteAction(mercant.ExchangeTime, () =>
        //    {
        //        MainData.Instance.gameState = MainData.GameState.PlayingGame;
        //        UIManager.Instance.ShowExchangePanel(mercant.itemInstance.render, Player.Instance.PlayerItem.render);
        //        UIManager.Instance.ShowOurObjectPanel(Player.Instance.PlayerItem.render);

        //        if (!mercant.itemInstance.IsShowBig())
        //            CameraMover.Instance.MoveToExchange();

        //        if (GameManager.Instance.isAcceptDeclinePanelUsed)
        //            UIManager.Instance.acceptDeclinePanel.SetPanelActive(true);

        //        // Tutorial
        //        if (GameManager.Instance.IsFirstLaunch)
        //        {
        //            GameManager.Instance.ShowTutorial();
        //        }
        //    });
        //});
    }

    public void MoveMercantOut(int index)
    {
        if (isDebug) DLog.D($"MoveMercantOut {index}");
        var mercant = MercantSpawner.Instance.mercants[index];
        //mercant.transform.position = LevelLocation.Current.exitMercantPoints[0].position;
        //mercant.transform.rotation = LevelLocation.Current.exitMercantPoints[0].rotation;

        mercant.PlayTakeSelf();
        Player.Instance.PlayTakeSelfAnimation();
        //path.Add(LevelLocation.Current.targetMercantPoint);
        //CoroutineActions.ExecuteAction(mercant.TakeInTime, () =>
        //{
        //    mercant.itemInstance.EquipSmall(mercant.handTr, false, true);
        //    Player.Instance.PlayerItem.EquipSmall(Player.Instance.handTr, false, true);
        //    mercant.itemInstance.HideBig(false);
        //});
        //CoroutineActions.ExecuteAction(mercant.TakeHideTime, () =>
        //{
        //    mercant.itemInstance.HideSmall();
        //    Player.Instance.PlayerItem.HideSmall();
        //});
        //CoroutineActions.ExecuteAction(mercant.TakeTime, () =>
        //{
        //    mercant.PlaySad();
        //});
        //CoroutineActions.ExecuteAction(mercant.TakeTime + mercant.SadTime, () =>
        //{
        //    List<Transform> path = new List<Transform>();
        //    path.AddRange(LevelLocation.Current.exitMercantPoints);
        //    mercant.PlaySadWalk();

        //    IndependedMoveSystem.Instance.GoPath(mercant.transform, path, .5f, 1f, () => mercant.PlayIdle());
        //});
    }

    public void InitMercantPositions()
    {
        if (isDebug) DLog.D($"InitMercantPositions");
        selectedMercantId = 0;
        //var posOriginal = Level.Instance.mercantSpawnPoints[1].position;
        //var posHide = posOriginal + Vector3.up * 10;
        //for (int i = 0; i < 3; i++)
        //{
        //    var mercant = MercantSpawner.Instance.mercants[i].transform;
        //    //if (i == 0)
        //    //{
        //    //    mercant.position = posHide;
        //    //    IndependedMoveSystem.Instance.Move(mercant, posOriginal, 0.4f, true);
        //    //}
        //    //else
        //    mercant.position = posHide;
        //}
    }

    public void SelectNextMercant()
    {
        if (MainData.Instance.IsPlaying)
        {
            if (isDebug) DLog.D($"SelectNextMercant");

            //CameraMover.Instance.MoveToDefault();

            int prevMercant = selectedMercantId;
            selectedMercantId++;
            if (selectedMercantId > 2)
                selectedMercantId = 0;
            SelectMercant(selectedMercantId, prevMercant);
        }
    }

    private void SelectMercant(int id, int prevMercant)
    {
        if (isDebug) DLog.D($"SelectMercant {id}/{prevMercant}");
        //var posOriginal = Level.Instance.mercantSpawnPoints[1].position;
        //var posHide = posOriginal + Vector3.up * 10;
        //for (int i = 0; i < 3; i++)
        //{
        //    var mercant = MercantSpawner.Instance.mercants[i].transform;
        //    if (prevMercant == i)
        //    {
        //        MoveMercantOut(prevMercant);
        //    }
        //    else if (i == id)
        //    {
        //        CoroutineActions.ExecuteAction(5f, () =>
        //        {
        //            MoveMercantIn(id);
        //        });
        //        //IndependedMoveSystem.Instance.Move(mercant, posHide, 0.4f, true, () =>
        //        //{
        //        //    IndependedMoveSystem.Instance.Move(mercant, posOriginal, 0.4f, true);
        //        //});
        //    }
        //    else
        //    {
        //        // IndependedMoveSystem.Instance.Move(mercant, posHide, 0.4f, true);
        //    }
        //}
    }
}
