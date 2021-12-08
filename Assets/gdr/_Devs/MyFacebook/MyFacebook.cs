//using Facebook.Unity;
//using System.Collections;
//using UnityEngine;

//public class MyFacebook : MonoBehaviour
//{
//    #region Singleton Init
//    private static MyFacebook _instance;

//    void Awake() // Init in order
//    {
//        if (_instance == null)
//            Init();
//        else if (_instance != this)
//            Destroy(gameObject);
//    }

//    public static MyFacebook Instance // Init not in order
//    {
//        get
//        {
//            if (_instance == null)
//                Init();
//            return _instance;
//        }
//        private set { _instance = value; }
//    }

//    static void Init() // Init script
//    {
//        _instance = FindObjectOfType<MyFacebook>();
//        _instance.Initialize();
//    }
//    #endregion

//    private void Initialize()
//    {
//#if UNITY_EDITOR
//        return; // Dont actually works in editor anyway
//#endif

//        if (FB.IsInitialized)
//        {
//            FB.ActivateApp();
//        }
//        else
//        {
//            FB.Init(() =>
//            {
//                FB.ActivateApp();
//            });
//        }

//        FBStartGame();
//    }

//    IEnumerator StartGameIE()
//    {
//        yield return new WaitForSeconds(5.0f);
//        LogEvent(GPGevent.game_start);
//    }

//    public void FBStartGame()
//    {
//        StartCoroutine(StartGameIE());
//    }

//    public void FBEndGame()
//    {
//        LogEvent(GPGevent.game_end);
//    }

//    public void LogEvent(GPGevent _event)
//    {
//#if UNITY_EDITOR
//        return; // Dont actually works in editor anyway
//#endif
//        if (FB.IsInitialized)
//        {
//            if (_event == GPGevent.level_complete)
//                FB.LogAppEvent($"{_event} ({MainData.Instance.currentLevel})");
//            else
//                FB.LogAppEvent(_event.ToString());
//        }
//    }

//    void OnApplicationPause(bool pauseStatus)
//    {
//#if UNITY_EDITOR
//        return; // Dont actually works in editor anyway
//#endif
//        if (!pauseStatus)
//        {
//            if (FB.IsInitialized)
//            {
//                FB.ActivateApp();
//            }
//            else
//            {
//                //Handle FB.Init
//                FB.Init(() =>
//                {
//                    FB.ActivateApp();
//                });
//            }
//        }
//        else
//        {
//            FBEndGame();
//        }
//    }

//}

///*
// * - level_achieved : the player clicks on level up
// * - unlocked_unit : the player unlocked a new unit (new line, new production unit..)
// * - ad_shown_interstitial : an interstitial is shown
// * - rewarded_shown : a rewarded video is shown
// * - banner_shown : an ad banner is shown
// */

//public enum GPGevent
//{
//    game_start, game_end, game_end_endless, game_end_levels,
//    level_complete, interstitial_shown, rewarded_shown, banner_shown,
//    skin_bought, player_died
//};

