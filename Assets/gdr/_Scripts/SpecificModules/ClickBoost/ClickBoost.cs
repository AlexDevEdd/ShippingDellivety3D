using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickBoost : MonoBehaviour
{
    #region Singleton Init
    private static ClickBoost _instance;

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

    public static ClickBoost Instance // Init not in order
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
        _instance = FindObjectOfType<ClickBoost>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    public bool isActivated;
    public float activateDuration;

    private float deactivateTime;

    void Initialize()
    {
        if (isDebug) DLog.D($"ClickBoost.DebugEnabled");
        // Init data here
        enabled = true;
    }

    public void Activate()
    {
        if (isDebug) DLog.D($"Activate"); 
        isActivated = true;
    }

    public void Deactivate()
    {
        if (isDebug) DLog.D($"Deactivate");
        isActivated = false;
    }

    private void Update()
    {
        if (IsClicked())
        {
            deactivateTime = Time.time + activateDuration;
            Activate();
        }

        if (isActivated && deactivateTime < Time.time)
            Deactivate();
    }

    private bool IsClicked()
    {
        if ((MainData.Instance.gameState == MainData.GameState.PlayingGame || MainData.Instance.gameState == MainData.GameState.PlayingAnimation) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                return true;
            }
        }
        return false;
    }
}
