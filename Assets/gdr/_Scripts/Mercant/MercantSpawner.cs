using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercantSpawner : MonoBehaviour
{
    #region Singleton Init
    private static MercantSpawner _instance;

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

    public static MercantSpawner Instance // Init not in order
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
        _instance = FindObjectOfType<MercantSpawner>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    public GameObject mercantPrefab;

    public List<Mercant> mercants;

    void Initialize()
    {
        if (isDebug) DLog.D($"MercantSpawner.DebugEnabled");
        // Init data here
        enabled = true;
    }

    public void SpawnMercants()
    {
        if (isDebug) DLog.D($"SpawnMercants");
        if (Level.Instance == null)
        {
            Debug.LogError($"Cant spawn mercants when level isn't created");
            return;
        }
        mercants = new List<Mercant>();
        int goodMercId = Random.Range(0, 3);
        for (int i = 0; i < 3; i++)
            SpawnMercant(i);
    }

    private void SpawnMercant(int posId)
    {
        if (isDebug) DLog.D($"SpawnMercants {posId}");
        var spawnPoint = transform; // Level.Instance.mercantSpawnPoints[posId];
        GameObject mercGO = Instantiate(mercantPrefab, spawnPoint);
        mercGO.transform.localPosition = Vector3.zero; // Skip rotation next
        var script = mercGO.GetComponent<Mercant>();
        script.RandomizeModel();
        
        int _locationNum = 0;
        for (int i = 0; i < LevelLocationManager.Instance.locationData.Count; i++)
            if (LevelLocationManager.Instance.locationData[i].locationToShow.gameObject.activeSelf)
            {
                _locationNum = i;
                break;
            }
        script.SetMercant(_locationNum);
        mercants.Add(script);
    }
}
