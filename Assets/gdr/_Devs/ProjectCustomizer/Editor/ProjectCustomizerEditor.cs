using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Callbacks;
using System.Collections.Generic;

// [CreateAssetMenu(fileName = "Temp", menuName = "M/Temp")]
public class ProjectCustomizerEditor : ScriptableObject
{
    private static ProjectCustomizerEditor Instance;

    public bool isEnabled = false;

    private static bool isInited = false;

    [System.Serializable]
    public class HashsetToIcon
    {
        public Texture2D tex;
        public List<UnityEngine.Object> files;
        public HashSet<string> guids;
    }
    public List<HashsetToIcon> hashsetToIcons;

    private void OnEnable()
    {
        Init();
    }

    [NaughtyAttributes.Button]
    void Init()
    {
        Instance = this;

        foreach (var item in Instance.hashsetToIcons)
        {
            item.guids = new HashSet<string>();
            foreach (var file in item.files)
            {
                var uid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(file));
                item.guids.Add(uid);
            }
        }
        isInited = true;
    }

    [DidReloadScripts]
    static void GizmoIconUtility()
    {
        isInited = false;
        EditorApplication.projectWindowItemOnGUI = ItemOnGUI;
    }

    static void ItemOnGUI(string guid, Rect rect)
    {
        if (Instance == null)
            return;

        if (!Instance.isEnabled)
            return;

        if (!isInited && Instance != null && Instance.hashsetToIcons != null)
        {
            foreach (var item in Instance.hashsetToIcons)
            {
                item.guids = new HashSet<string>();
                foreach (var file in item.files)
                {
                    var uid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(file));
                    item.guids.Add(uid);
                }
            }
            isInited = true;
        }

        if (isInited)
        {
            foreach (var hash in Instance.hashsetToIcons)
            {
                if (hash.guids != null)
                {
                    if (hash.guids.Contains(guid))
                    {
                        if (hash.tex != null)
                        {
                            rect.width = rect.height;
                            GUI.DrawTexture(rect, (Texture2D)hash.tex);
                            break;
                        }
                    }
                }
            }
        }
    }
}