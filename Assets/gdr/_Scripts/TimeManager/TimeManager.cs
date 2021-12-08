using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// 1. Internet time. Call method GetInternetTime(success, fail), and recieve result in "internetTime" field
/// "isInternetTimeExist" should be true
/// </summary>
public class TimeManager : MonoBehaviour
{
    #region Singleton Init
    private static TimeManager _instance;

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

    public static TimeManager Instance // Init not in order
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
        _instance = FindObjectOfType<TimeManager>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    [NaughtyAttributes.ShowNativeProperty]
    string CurrentTime => (DateTimeOffset.Now.ToOffset(TimeSpan.Zero).ToString());
    [NaughtyAttributes.ShowNativeProperty]
    string CurrentInternetTime => (internetTime == null ? "" : internetTime.Value.ToOffset(TimeSpan.Zero).ToString());
    [NaughtyAttributes.ShowNativeProperty]
    string FreeSpinTime => (internetTime == null && PlayerPrefs.HasKey("FreeSpin") ? "" : Instance.DTOLoad("FreeSpin").ToOffset(TimeSpan.Zero).ToString());
    [NaughtyAttributes.ShowNativeProperty]
    string Hours24UTC => (internetTime == null && PlayerPrefs.HasKey("hours24key") ? "" : Instance.DTOLoad("hours24key").ToOffset(TimeSpan.Zero).ToString());

    public DateTimeOffset? internetTime;

    public bool isInternetTimeExist;
    private bool isHaveInternet;

    public TimeSpan currentTimeDif = new TimeSpan(0);

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    private void Initialize()
    {
        isInternetTimeExist = false;
        enabled = true;

        GetInternetTime(null, null);
    }

    private void Update()
    {
        if (isInternetTimeExist)
        {
            sw.Stop();
            internetTime = internetTime.Value.AddMilliseconds(sw.ElapsedMilliseconds);
            sw.Restart();
        }
    }

    public class Example
    {
        public const string ExampleKey = "ExampleKeyTime";
    }

    #region Ok, Now its easy to use

    public bool IsDateExist(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public bool IsNetDateReached(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return IsNetTimeMoreThanSavedValue(key);
        return false;
    }

    public bool IsLocalDateReached(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return IsCurrentTimeMoreThanSavedValue(key);
        return false;
    }

    public void SetAvailableExactTime(string key, DateTimeOffset dt)
    {
        PlayerPrefs.SetString(key, dt.ToUnixTimeMilliseconds().ToString());
    }

    public void SetAvailableIn(string key, TimeSpan time, bool sinceCurrent) // Set to 2:00:00 - will set after 2 hours
    {
        Debug.Log($"SetAvailableIn {time}");
        if (!IsDateExist(key))
            SaveWithCurrentTime(key, time);
        else if (sinceCurrent)
            SaveWithCurrentTime(key, time);
        else
            AddToExist(key, time);
    }

    public TimeSpan GetDifference(string key, bool isCurrentMore)
    {
        var ts = GetTimeDifference(key, isCurrentMore) + currentTimeDif;
        ts = new TimeSpan(ts.Days < 0 ? 0 : ts.Days, ts.Hours < 0 ? 0 : ts.Hours, ts.Minutes < 0 ? 0 : ts.Minutes, ts.Seconds < 0 ? 0 : ts.Seconds, 0);
        return ts;
    }

    public DateTimeOffset GetExact(string key)
    {
        var dtoDiff = Instance.DTOLoad(key);
        return dtoDiff;
    }

    public DateTimeOffset? GetCurrentTime()
    {
        return internetTime;
    }

    #endregion

    #region Use it! (High level api)

    /// <summary>
    /// Saved = Current
    /// </summary>
    private void SaveWithCurrentTime(string key)
    {
        if (isInternetTimeExist)
            PlayerPrefs.SetString(key, ToStrInternetTime(TimeSpan.Zero));
        else
            Debug.LogError($"You are trying to use net time while it's not available yet! Or network is not reachable!");
    }

    private void SaveWithCurrentTime(string key, TimeSpan add)
    {
        Debug.Log($"SaveWithCurrentTime {key} {add}");
        var result = ToStrInternetTime(add);
        if (result == null)
            Debug.LogError($"Your value {key} cant be saved until you get net time!");
        PlayerPrefs.SetString(key, result);
    }

    private void SaveWithNetTime(string key, TimeSpan add) // Same as above
    {
        var result = ToStrInternetTime(add);
        if (result == null)
            Debug.LogError($"Your value cant be saved until you get net time!");
        PlayerPrefs.SetString(key, result);
    }

    private void AddToExist(string key, TimeSpan add)
    {
        SaveWithSpecifiedValue(key, DTOLoad(key).Add(add));
    }

    private bool IsNetTimeMoreThanSavedValue(string key)
    {
        var current = internetTime;
        if (internetTime != null)
        {
            var saved = DTOLoad(key);
            bool isMore = (current - saved).Value.TotalHours >= 0f;
            return isMore;
        }
        else
            return false;
    }

    private bool IsCurrentTimeMoreThanSavedValue(string key)
    {
        var current = DateTimeOffset.Now;
        var saved = DTOLoad(key);
        return (current - saved).TotalHours >= 0f;
    }

    private TimeSpan GetTimeDifference(string key, bool isCurrentMore = false)
    {
        if (isCurrentMore)
        {
            var dtoDiff = Instance.ToDTONetTimeAsUnixTime() - Instance.DTOLoad(key);
            return new TimeSpan(dtoDiff.Hours, dtoDiff.Minutes, dtoDiff.Seconds);
        }
        else
        {
            var dtoDiff = Instance.DTOLoad(key) - Instance.ToDTONetTimeAsUnixTime();
            return new TimeSpan(dtoDiff.Hours, dtoDiff.Minutes, dtoDiff.Seconds);
        }
    }

    #endregion

    public bool IsReachableCache()
    {
        return isHaveInternet;
    }

    public bool IsReachable()
    {
        UpdateState();
        return isHaveInternet;
    }

    private void OnApplicationPause(bool _isPause)
    {
        if (!_isPause)
            UpdateState();
    }

    private void UpdateState()
    {
        isHaveInternet = (
            Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork);
    }

    public void GetInternetTime(UnityAction success, UnityAction fail)
    {
        StartCoroutine(GetInternetTimeCoroutine(success, fail));
    }

    private IEnumerator GetInternetTimeCoroutine(UnityAction success, UnityAction fail)
    {
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("https://www.google.com");
        yield return myHttpWebRequest.SendWebRequest();

        if (myHttpWebRequest.isNetworkError)
        {
            if (isDebug) Debug.Log("NETWORK ERROR");
            internetTime = null;
            if (fail != null)
                fail.Invoke();
            yield break;
        }
        string netTimeString = myHttpWebRequest.GetResponseHeader("date");
        if (netTimeString != "")
            internetTime = DateTimeOffset.ParseExact(netTimeString,
                                           "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                           CultureInfo.InvariantCulture.DateTimeFormat,
                                           DateTimeStyles.AssumeUniversal);
        if (isDebug) Debug.Log("Global UTC time: " + internetTime);
        isInternetTimeExist = true;
        currentTimeDif = DateTimeOffset.Now - internetTime.Value;
        currentTimeDif = new TimeSpan(currentTimeDif.Days, currentTimeDif.Hours, currentTimeDif.Minutes, currentTimeDif.Seconds,0);
        sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        if (success != null)
            success.Invoke();
    }

    public class DailyReward
    {
        public const string LastRewardDateTime = "Last Reward Date Time";
        public static void Time_TestDebugSetActive()
        {
            //if (HackerKiller.Instance.HaveNetTimeAndInternet())
                Instance.SaveWithNetTime(LastRewardDateTime, new TimeSpan(0, 0, 5));
        }

        public static void Time_BackTo_ForFirstTime()
        {
           //if (HackerKiller.Instance.HaveNetTimeAndInternet())
                Instance.SaveWithNetTime(LastRewardDateTime, new TimeSpan(-24, 0, 0));
        }

        public static void Time_DailyTaken()
        {
           // if (HackerKiller.Instance.HaveNetTimeAndInternet())
                Instance.SaveWithNetTime(LastRewardDateTime, new TimeSpan(24, 0, 0));
        }

        public static DateTimeOffset Time_GetBackTo()
        {
            return Instance.DTOLoad(LastRewardDateTime);
        }

        public static bool Is24HoursLeft()
        {
            return Instance.IsNetTimeMoreThanSavedValue(LastRewardDateTime);
        }

        public static TimeSpan GetCurrentTimeDifference()
        {
            var diff = Instance.GetTimeDifference(LastRewardDateTime);
            if (diff.TotalSeconds < 0f)
                return new TimeSpan(0, 0, 0);
            return diff;
        }
    }

    #region Dont use - it's internal (Low level api)

    private string StrLoad(string key)
    {
        return PlayerPrefs.GetString(key, "");
    }

    private DateTimeOffset DTOLoad(string key)
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(key, "")))
            return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(PlayerPrefs.GetString(key)));
        //throw new System.NotSupportedException();
        return DateTimeOffset.MinValue;
    }

    private void SaveWithSpecifiedValue(string key, DateTimeOffset dto)
    {
        PlayerPrefs.SetString(key, dto.ToUnixTimeMilliseconds().ToString());
    }

    private string ToStrInternetTime(TimeSpan add)
    {
        //return DateTimeOffset.Now.Add(add).ToUnixTimeMilliseconds().ToString();
        var str = internetTime?.Add(add).ToUnixTimeMilliseconds().ToString();
        if (str == null)
            Debug.Log($"null time");
        return str;
    }

    private DateTimeOffset ToDTOCurrentTimeAsUnixTime()
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()));
    }

    private DateTimeOffset ToDTONetTimeAsUnixTime()
    {
        //return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()));
#if UNITY_EDITOR
        return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()));
#else
        return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(internetTime?.ToUnixTimeMilliseconds().ToString()));
#endif
    }

    #endregion
}
