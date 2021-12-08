using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region Singleton Init
    private static LevelManager _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static LevelManager Instance // Init not in order
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
        _instance = FindObjectOfType<LevelManager>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    public Level currentLevel;
    public int currentIndex;
    [Header("Dev")]
    public int alwaysLoadLevelId = -1;

    void Initialize()
    {
        if (isDebug) DLog.D($"LevelManager.DebugEnabled");
        // Init data here
        enabled = true;
    }

    public void CreateCurrentLevel()
    {
        CreateLevel(MainData.Instance.currentLevel);
    }

    [NaughtyAttributes.ButtonInGame] void CreateLevel1() => CreateLevel(1);

    public void CreateLevel(int index, bool isRewriteMainDataLevelId = true) // MainData
    {
        if (isDebug) DLog.D($"CreateLevel {index}/{isRewriteMainDataLevelId}");
        currentIndex = index;
        if (currentIndex > EditorDatabase.Instance.levelDatas.Count - 1)
            currentIndex = Random.Range(0, EditorDatabase.Instance.levelDatas.Count);
        if (alwaysLoadLevelId != -1)
        {
            Debug.LogWarning($"<color=yellow>Caution, alwaysLoadLevelId is not -1, so it will load always the same level! levelId: {alwaysLoadLevelId}</color>");
            currentIndex = alwaysLoadLevelId;
        }
        if (currentLevel != null)
            DestroyCurrentLevel();

        if (EditorDatabase.Instance.levelDatas.Count == 0)
            Debug.LogError("You forgot fill level data?");

        if (isDebug) DLog.D($"CreateLevel try with id {index} selected id : {currentIndex} max lvl: { EditorDatabase.Instance.levelDatas.Count - 1}");

        if (EditorDatabase.Instance.levelDatas == null || EditorDatabase.Instance.levelDatas.Count < 1)
        {
            Debug.LogError("levelDatas is empty!");
            return;
        }
        else if (EditorDatabase.Instance.levelDatas.Count <= currentIndex)
        {
            Debug.LogError($"levelDatas is out of range db:{EditorDatabase.Instance.levelDatas.Count}, id:{currentIndex}");
            return;
        }
        else if (EditorDatabase.Instance.levelDatas[currentIndex] == null)
        {
            Debug.LogError($"levelDatas at index {currentIndex} is null");
            return;
        }

        currentLevel = Instantiate(EditorDatabase.Instance.levelDatas[currentIndex], null).GetComponent<Level>();
        if (BackgroundScript.Instance != null)
        {
            if (EditorDatabase.Instance.backgroundColors.Count > currentIndex)
                BackgroundScript.Instance.SetMaterial(EditorDatabase.Instance.backgroundColors[currentIndex]);
            else
                Debug.Log($"Cant create bg for level with index {currentIndex}");
        }
        Level.Instance = currentLevel;

        if (isRewriteMainDataLevelId)
        {
            MainData.Instance.currentLevel = index;
            CacheUtil.Set("currentLevel", index);
        }
    }

    public int GetLevelIdInList()
    {
        if (currentLevel != null)
        {
            int id = 0;
            foreach (var data in EditorDatabase.Instance.levelDatas)
            {
                if ($"{data.name}(Clone)" == currentLevel.name)
                    return id;
                id++;
            }
        }
        return -1; // not in list or empty level
    }

    private bool IsLevelValidLogs(int levelId)
    {
        if (isDebug) DLog.D($"IsLevelValidLogs {levelId}");

        if (EditorDatabase.Instance.levelDatas[levelId] == null)
        {
            Debug.LogError($"levelDatas at index {currentIndex} is null");
            return false;
        }
        return true;
    }

    [NaughtyAttributes.ButtonInGame]
    public void DestroyCurrentLevel()
    {
        if (isDebug) DLog.D($"DestroyCurrentLevel {currentLevel}");
        if (currentLevel != null)
        {
            currentLevel.DestroySelf();
            currentLevel = null;
        }
    }

    [NaughtyAttributes.Button]
    private void CheckAllLevels()
    {
        if (EditorDatabase.Instance.levelDatas == null || EditorDatabase.Instance.levelDatas.Count < 1)
        {
            Debug.LogError("levelDatas is empty!");
            return;
        }
        else
        {
            for (int i = 0; i < EditorDatabase.Instance.levelDatas.Count; i++)
            {
                IsLevelValidLogs(i);
            }
        }
    }
}
