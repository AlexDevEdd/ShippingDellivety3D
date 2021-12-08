using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
// using LionStudios;
// using LionStudios.Ads;

public class ShowAds : MonoBehaviour // LionStudios
{
    #region Singleton Init
    private static ShowAds _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static ShowAds Instance // Init not in order
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
        _instance = FindObjectOfType<ShowAds>();
        _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    private UnityAction successCallback, failureCallback;
    // private ShowAdRequest _ShowRewardedAdRequest;
    // private ShowAdRequest _ShowInterAdRequest;

    private void Initialize()
    {
        if (isDebug) DLog.D($"ShowAds.DebugEnabled");
        //// Create show ad request when initializing
        //_ShowRewardedAdRequest = new ShowAdRequest();

        //// Ad event callbacks
        //_ShowRewardedAdRequest.OnDisplayed += adUnitId => Debug.Log("Displayed Rewarded Ad :: Ad Unit ID = " + adUnitId);
        //_ShowRewardedAdRequest.OnClicked += adUnitId => Debug.Log("Clicked Rewarded Ad :: Ad Unit ID = " + adUnitId);
        //_ShowRewardedAdRequest.OnHidden += adUnitId => Debug.Log("Closed Rewarded Ad :: Ad Unit ID = " + adUnitId);
        //_ShowRewardedAdRequest.OnFailedToDisplay += (adUnitId, error) => Debug.LogError("Failed To Display Rewarded Ad :: Error = " + error + " :: Ad Unit ID = " + adUnitId);
        //_ShowRewardedAdRequest.OnReceivedReward += (adUnitId, reward) => OnReveivedReward(adUnitId, reward);

        //// Analytics settings and data
        //_ShowRewardedAdRequest.sendAnalyticsEvents = true; // Defaults to true
        /////////////////////////////////////////////////////////////////////
        //_ShowInterAdRequest = new ShowAdRequest();
        //// Analytics settings and data
        //_ShowInterAdRequest.sendAnalyticsEvents = true; // Defaults to true
    }

    public void ShowRewardAd(UnityAction _successCallback, string _placementName, UnityAction _failureCallback = null)
    {
        if (isDebug) DLog.D($"ShowRewardAd {_placementName}");
#if UNITY_EDITOR
        _successCallback();
        return;
#endif
        successCallback += _successCallback;
        if (_failureCallback != null)
            failureCallback += _failureCallback;

        //_ShowRewardedAdRequest.SetPlacement($"rewarded_video_{_placementName}");
        //_ShowRewardedAdRequest.SetLevel(MainData.Instance.currentLevel);
        //_ShowRewardedAdRequest.SetEventParam(_placementName, "custom_param_value");
        //RewardedAd.Show(_ShowRewardedAdRequest);
        //AdsTimer.Instance.RewardAdShowed();
    }

    //private void OnReveivedReward(string _adUnitId, MaxSdkBase.Reward _reward)
    //{
    //    successCallback?.Invoke();
    //    Debug.Log("Received Reward :: Reward = " + _reward + " :: Ad Unit ID = " + _adUnitId);
    //}

    public bool IsRewardAdsReady()
    {
        if (isDebug) DLog.D($"IsRewardAdsReady false");
        return false;
        //return RewardedAd.IsAdReady;
    }

    public void ShowInterstitialAd()
    {
        if (isDebug) DLog.D($"ShowInterstitialAd");
        //Interstitial.Show(_ShowInterAdRequest);
        //AdsTimer.Instance.InterstitialAdShowed();
    }

    public bool InterstitialAdsIsReady()
    {
        if (isDebug) DLog.D($"InterstitialAdsIsReady");
        return false;
        //return Interstitial.IsAdReady;
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.Advertisements;
////using GameAnalyticsSDK;

//public class ShowAds : MonoBehaviour
//{
//    #region Singleton Init
//    private static ShowAds _instance;

//    void Awake() // Init in order
//    {
//        if (_instance == null)
//            Init();
//        else if (_instance != this)
//            Destroy(gameObject);
//    }

//    public static ShowAds Instance // Init not in order
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
//        _instance = FindObjectOfType<ShowAds>();
//        _instance.Initialize();
//    }
//    #endregion  

//    public string androidGameId = "3411822";
//    public string iosGameId = "3411823";
//    public bool isTestMode;
//    public static string video_ads = "video";

//    private void Initialize()
//    {
////#if UNITY_ANDROID
////        Advertisement.Initialize(androidGameId, isTestMode);
////#elif UNITY_IOS
////        Advertisement.Initialize(iosGameId, isTestMode);
////#endif
//    }

//    public void ShowAd()
//    {
//        //Advertisement.Show(video_ads);
//        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"interstitial_shown");
//        //FBInit.Instance.LogEvent(GPGevent.interstitial_shown);

//        //LionStudios.Ads.Banner.Show(ShowAdRequest showAdRequest = null)
//        //LionStudios.Ads.Interstitial.Show(ShowAdRequest showAdRequest = null)
//        //LionStudios.Ads.RewardedAd.Show(ShowAdRequest showAdRequest = null)

//        //PlayerConfig playerConfig = LionStudios.AppLovin.LoadRemoteData<PlayerConfig>();
//    }

//    public bool AdsIsReady()
//    {
//        return true;
//        //return Advertisement.IsReady(video_ads);
//    }

//    //public class PlayerConfig
//    //{
//    //    public int startingHitPoints = 137;
//    //    public float damage = 23.0f;
//    //    public float movementSpeed = 4.7f;
//    //}
//}
