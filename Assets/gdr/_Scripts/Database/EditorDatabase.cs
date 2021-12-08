using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorDatabase : MonoBehaviour
{
    #region Singleton Init
    private static EditorDatabase _instance;

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

    public static EditorDatabase Instance // Init not in order
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
        _instance = FindObjectOfType<EditorDatabase>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool IsInited = false;

#if UNITY_EDITOR
    //[NaughtyAttributes.Required]
    //public Object textFolder;
    [NaughtyAttributes.Required]
    public Object levelsFolder;
    public Object backgroundMaterialFolder;
#endif

    [Header("Data")]
    public List<TextData> textDatas;
    public List<GameObject> levelDatas;
    public List<Material> backgroundColors;

    void Initialize()
    {
        // Init data here
        enabled = true;
    }

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    void FillData()
    {
        //textDatas = AssetLoader<TextData>.GetItems(textFolder);
        levelDatas = AssetLoader<GameObject>.GetItems(levelsFolder);
        backgroundColors = AssetLoader<Material>.GetItems(backgroundMaterialFolder);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif

    }
#endif
}
