using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.Events;

public partial class GameManager : MonoBehaviour
{
    #region Singleton Init
    private static GameManager _instance;
    private static bool isInitialized = false; // A bit faster singleton

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static GameManager Instance // Init not in order
    {
        get
        {
            if (!isInitialized)
                Init();
            return _instance;
        }
        private set { _instance = value; }
    }

    static void Init() // Init script
    {
        _instance = FindObjectOfType<GameManager>();
        if (_instance != null)
        {
            _instance.Initialize();
            isInitialized = true;
        }
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    [Header("Tweaks")]
    public OnChangeBool<bool> isBigChange;
    public bool isBig;
    [NaughtyAttributes.DisableIf("isBig")]
    public float guySize = 1.5f;
    [NaughtyAttributes.DisableIf("isBig")]
    public float rollerSize = 0.66f;
    public static float GuySize = 1f;
    public static float RollerSize = 1f;
    public bool isWithoutUI;
    public OnChangeBool<bool> isWithoutUIChange;

    [System.Serializable]
    public class OnChangeBool<T> // Update should be called in update to register change of value
    {
        [HideInInspector] public UnityAction<T> onChangeEvent;
        private T cachedValue;
        public System.Func<T> Value;

        public void Update()
        {
            if (!cachedValue.Equals(Value()))
            {
                cachedValue = Value();
                onChangeEvent.Invoke(cachedValue);
            }
        }
    }

    private void Initialize()
    {
        if (isDebug) DLog.D();

        // Load game settings here
        MainData.Instance.isVibration = CacheUtil.Get("isVibration", true);

        isBigChange.Value = new System.Func<bool>(() => { return isBig; });
        isBigChange.onChangeEvent = (x) => 
        {  
            if (x)
            {
                GuySize = guySize;
                RollerSize = rollerSize;
            }
            else
            {
                GuySize = 1f;
                RollerSize = 1f;
            }
        };

        isWithoutUIChange.Value = new System.Func<bool>(() => { return isWithoutUI; });
        isWithoutUIChange.onChangeEvent = (x) => 
        {
            if (x)
                UIManager.Instance.transform.GetChild(0).transform.localScale = Vector3.zero;
            else
                UIManager.Instance.transform.GetChild(0).transform.localScale = Vector3.one;
        };

        enabled = true;
    }

    public void Start() { ShowTapToStart(); }
    private void ShowTapToStart()
    {
        if (isDebug) DLog.D($"InitializeGame");
        UIManager.Instance.ShowTapToStartPanel();
        UIManager.Instance.SetPlayerStatsPanel(false, false, false, false, false);
    }

    private void Update()
    {
        if (MainData.Instance.gameState == MainData.GameState.TapToStartWindow)
        {
            // Do things
        }

        isBigChange.Update();
#if UNITY_EDITOR
        isWithoutUIChange.Update();
#endif
    }

    private void StartGame() // Same as load game btw
    {
        if (isDebug) DLog.D($"StartGame");
        MainData.Instance.gameState = MainData.GameState.PlayingAnimation;
        if (Level.Instance == null) LevelManager.Instance.CreateCurrentLevel();
        UIManager.Instance.SetPlayerStatsPanel(true, true, true, true, false);
        // Minigame.Current.Run();
        // LevelLocationManager.Instance.ShowFirstLocation();
    }

    //private void LoadGame(int index)
    //{
    //    if (isDebug) DLog.DPrintAutoColor($"LoadGame");
    //    MainData.Instance.gameState = MainData.GameState.PlayingAnimation;
    //    if (Level.Instance == null) LevelManager.Instance.CreateLevel(index);
    //    UIManager.Instance.SetPlayerStatsPanel(true, true, true, true, false);
    //    // LevelLocationManager.Instance.ShowFirstLocation();
    //}

    public void RestartCurrentLevel()
    {
        if (isDebug) DLog.D($"RestartCurrentLevel");
        MainData.Instance.gameState = MainData.GameState.PlayingAnimation;
        // if (Level.Instance == null) LevelManager.Instance.CreateCurrentLevel();
        UIManager.Instance.SetPlayerStatsPanel(true, true, true, true, false);
        // LevelLocationManager.Instance.ShowFirstLocation();
        // Level.Instance.minigame.Restart();
    }

    public void StartNextLevel()
    {
        if (isDebug) DLog.D($"StartNextLevel");
        MainData.Instance.gameState = MainData.GameState.PlayingAnimation;
        if (Level.Instance == null) LevelManager.Instance.CreateLevel(MainData.Instance.currentLevel + 1);
        UIManager.Instance.SetPlayerStatsPanel(true, true, true, true, false);
        // LevelLocationManager.Instance.ShowFirstLocation();
    }


    private void GoToNextStage()
    {
        MainData.Instance.gameState = MainData.GameState.PlayingAnimation;
    }
}
