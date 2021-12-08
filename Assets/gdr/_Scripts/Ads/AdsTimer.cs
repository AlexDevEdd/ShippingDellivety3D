using UnityEngine;

public class AdsTimer : MonoBehaviour
{
    #region Singleton Init
    private static AdsTimer _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static AdsTimer Instance // Init not in order
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
        _instance = FindObjectOfType<AdsTimer>();
        _instance.Initialize();
    }
    #endregion
   
    private void Initialize() { }
        
    public float timer;
    public float rewardAdsTimer;

    public bool isFirstAdRequsted = false;
    public bool isRequestedInterstitialAd = false;
    public bool isRequestedRewardAd = false;
    public bool isAfterReward;

    private void Update()
    {
        if (!isRequestedInterstitialAd)
        {
            timer += Time.deltaTime;

            if (timer > MainData.Instance.interStartDelay)
            {
                RequestInterstitialAd();
                isFirstAdRequsted = true;
                timer = 0.0f;
                return;
            }
            if (!isAfterReward)
            {
                if (timer > MainData.Instance.interIntervalDelay && isFirstAdRequsted)
                {
                    RequestInterstitialAd();
                    timer = 0.0f;
                }
            }
            else
            {
                if (timer > MainData.Instance.afterRewardDelay && isFirstAdRequsted)
                {
                    isAfterReward = false;
                    RequestInterstitialAd();
                    timer = 0.0f;
                }
            }
        }
    }

    public void RequestInterstitialAd()
    {
        Debug.Log($"isRequestedInterstitialAd = {isRequestedInterstitialAd} !!!!!!!!!!!!!!");
        isRequestedInterstitialAd = true;
        timer = 0.0f;
    }

    public void InterstitialAdShowed()
    {
        Debug.Log($"isRequestedInterstitialAd = {isRequestedInterstitialAd} !!!!!!!!!!!!!!");
        isRequestedInterstitialAd = false;
        timer = 0.0f;
    }

    public void RewardAdShowed()
    {
        Debug.Log($"isRequestedInterstitialAd = {isRequestedInterstitialAd} !!!!!!!!!!!!!!");
        isAfterReward = true;
        isRequestedInterstitialAd = false;
        isFirstAdRequsted = false;
        timer = 0.0f;
    }
}
