using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTool : MonoBehaviour
{
    #region Singleton Init
    private static DebugTool _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static DebugTool Instance // Init not in order
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
        _instance = GameObject.Find("PermanentGO").GetComponent<DebugTool>();
        _instance.Initialize();
    }
    #endregion

    void Initialize()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        // Init data here
        enabled = true;
    }

    [ContextMenu("Load scene 0")]
    public void LoadScene0()
    {
        SceneManager.LoadScene(0);
    }

    [ContextMenu("Load scene 1")]
    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }
}
