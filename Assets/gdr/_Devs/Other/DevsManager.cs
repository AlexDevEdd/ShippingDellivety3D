using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevsManager : MonoBehaviour
{
    #region Singleton Init
    private static DevsManager _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static DevsManager Instance // Init not in order
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
        _instance = FindObjectOfType<DevsManager>();
    }
    #endregion
}
