using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSystem : MonoBehaviour
{
    #region Singleton Init
    private static SwipeSystem _instance;

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

    public static SwipeSystem Instance // Init not in order
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
        _instance = FindObjectOfType<SwipeSystem>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    public bool isListeningSwipes = false;

    void Initialize()
    {
        SwipeManager.OnSwipeDetected += (Swipe swipeDirection, Vector2 swipeDelta) =>
        {
            if (isListeningSwipes)
            {
                if (isDebug) DLog.D($"SwipeSystem.DebugEnabled dir:{swipeDirection} vel:{swipeDelta}");

                //if (swipeDirection == Swipe.Right)
                //    CameraMover.Instance.OnClick_PrevInterrogationFace();
                //else if (swipeDirection == Swipe.Left)
                //    CameraMover.Instance.OnClick_NextInterrogationFace();
            }
        };

        // Init data here
        enabled = true;
    }
}
