// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using static AppsFlyer;

public class MyAppsflyer : MonoBehaviour
{
//     #region Singleton Init
//     private static MyAppsflyer _instance;

//     void Awake() // Init in order
//     {
//         if (_instance == null)
//             Init();
//         else if (_instance != this)
//         {
//             Debug.Log($"Destroying {gameObject.name}, caused by one singleton instance");
//             Destroy(gameObject);
//         }
//     }

//     public static MyAppsflyer Instance // Init not in order
//     {
//         get
//         {
//             if (_instance == null)
//                 Init();
//             return _instance;
//         }
//         private set { _instance = value; }
//     }

//     static void Init() // Init script
//     {
//         _instance = FindObjectOfType<MyAppsflyer>();
//         if (_instance != null)
//             _instance.Initialize();
//     }
//     #endregion

//     const string m_appsflyerDevKey = "6eRCe9pE9pB7QAkSXDptTa"; //"rhm2p8MNtjDThjnnLcR7ph";
//     const string m_androidPackageName = "com.AlexGame.PregnancyIdle";
//     const string m_iosKey = "1500555497"; //1470921988

//     void Initialize()
//     {
//         // Init data here
//         enabled = true;
//     }

//     void Start()
//     {
//         Application.runInBackground = true;
//         // Screen.orientation = ScreenOrientation.Portrait;
//         transform.SetParent(null);
//         DontDestroyOnLoad(this);
//         AppsFlyer.setIsDebug(true);

//         AppsFlyer.setAppsFlyerKey(m_appsflyerDevKey);

// #if UNITY_IOS
//         AppsFlyer.setAppID(m_iosKey);
//         AppsFlyer.trackAppLaunch();
// #elif UNITY_ANDROID
//         AppsFlyer.setAppID(m_androidPackageName);
//         AppsFlyer.init(m_appsflyerDevKey, "AppsFlyerTrackerCallbacks");
//         AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks");
// #endif
//     }
}
