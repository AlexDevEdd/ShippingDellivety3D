using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// V 1.1.1 Personal debug script, works only for you
#if UNITY_EDITOR
[CustomEditor(typeof(QLog))]
public class QLogEditor : Editor
{
    // Main Settings are here
    private void DrawData(QLog script)
    {
        QLogMethod(ref GameManager.isDebug, nameof(GameManager), script.data);
        QLogMethod(ref MainData.isDebug, nameof(MainData), script.data);
        QLogMethod(ref UIManager.isDebug, nameof(UIManager), script.data);
        QLogMethod(ref LevelManager.isDebug, nameof(LevelManager), script.data);
        QLogMethod(ref SwipeSystem.isDebug, nameof(SwipeSystem), script.data);
        QLogMethod(ref CollectorScript.isDebug, nameof(CollectorScript), script.data);
        QLogMethod(ref CoroutineActions.isDebug, nameof(CoroutineActions), script.data);
        QLogMethod(ref ShowAds.isDebug, nameof(ShowAds), script.data);
        QLogMethod(ref TimeManager.isDebug, nameof(TimeManager), script.data);
        QLogMethod(ref CameraMover.isDebug, nameof(CameraMover), script.data);
        QLogMethod(ref SpritePainter.isDebug, nameof(SpritePainter), script.data);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        QLog script = (QLog)target;

        script.data = new System.Collections.Generic.List<QLog.QLogData>();

        DrawData(script);

        EditorGUILayout.LabelField($"Entries: {script.data.Count}");

        if (GUILayout.Button("Cache colors"))
        {
            script.CacheColors();
        }

        if (GUILayout.Button("Decache colors"))
        {
            script.DecacheColors();
        }

        UnityEditor.EditorUtility.SetDirty(script);

        serializedObject.ApplyModifiedProperties();
    }

    private void QLogMethod(ref bool isDebug, string className, System.Collections.Generic.List<QLog.QLogData> linkToList)
    {
        var isEnabled = PlayerPrefs.GetInt($"{className}", 0) == 1;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"{className}", GUILayout.Width(100));
        var oldValue = isEnabled;
        var value = EditorGUILayout.Toggle(isEnabled, GUILayout.Width(20));
        if (oldValue != value)
        {
            PlayerPrefs.SetInt($"{className}", value ? 1 : 0);
            isDebug = value;
        }
        var loadColor = new Color(
            PlayerPrefs.GetFloat($"{className}.r", 1f),
            PlayerPrefs.GetFloat($"{className}.g", 1f),
            PlayerPrefs.GetFloat($"{className}.b", 1f),
            PlayerPrefs.GetFloat($"{className}.a", 1f));
        loadColor = EditorGUILayout.ColorField(loadColor, GUILayout.Width(40));
        PlayerPrefs.SetFloat($"{className}.r", loadColor.r);
        PlayerPrefs.SetFloat($"{className}.g", loadColor.g);
        PlayerPrefs.SetFloat($"{className}.b", loadColor.b);
        PlayerPrefs.SetFloat($"{className}.a", loadColor.a);
        //if (!isEnabled)
        //{
        //    if (GUILayout.Button("Enable", GUILayout.Width(100)))
        //    {
        //        PlayerPrefs.SetInt($"{className}", 1);
        //        isDebug = true;
        //    }
        //}
        //else
        //{
        //    if (GUILayout.Button("Disable", GUILayout.Width(100)))
        //    {
        //        PlayerPrefs.SetInt($"{className}", 0);
        //        isDebug = false;
        //    }
        //}
        EditorGUILayout.EndHorizontal();
        linkToList.Add(new QLog.QLogData() { className = className });
    }
}
#endif

public class QLog : MonoBehaviour
{
    public static QLog Instance;

    [System.Serializable]
    public class QLogData
    {
        public string className;
    }

    public List<QLogData> data;
    public List<Color> colors;

    private void Awake()
    {
        Instance = this;
#if UNITY_EDITOR
        if (data != null)
        {
            foreach (var d in data)
            {
                if (d != null)
                {
                    Type t = Type.GetType($"{d.className}");
                    try
                    {
                        var field = t.GetField("isDebug");
                        field.SetValue(t, PlayerPrefs.GetInt($"{d.className}") == 1);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"{d.className} e");
                    }
                    try
                    {
                        var field2 = t.GetField("debugColor");
                        var color = new Color(
                        PlayerPrefs.GetFloat($"{d.className}.r", 1f),
                        PlayerPrefs.GetFloat($"{d.className}.g", 1f),
                        PlayerPrefs.GetFloat($"{d.className}.b", 1f),
                        PlayerPrefs.GetFloat($"{d.className}.a", 1f));
                        field2.SetValue(t, color);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"{d.className} e");
                    }
                }
            }
        }
#endif
    }

    public void CacheColors()
    {
#if UNITY_EDITOR
        int id = -1;
        colors = new List<Color>();
        if (data != null)
        {
            foreach (var d in data)
            {
                id++;
                if (d != null)
                {
                    Type t = Type.GetType($"{d.className}");
                    try
                    {
                        var color = new Color(
                        PlayerPrefs.GetFloat($"{d.className}.r", 1f),
                        PlayerPrefs.GetFloat($"{d.className}.g", 1f),
                        PlayerPrefs.GetFloat($"{d.className}.b", 1f),
                        PlayerPrefs.GetFloat($"{d.className}.a", 1f));

                        colors.Add(color);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"{d.className} e");
                    }
                }
            }
        }
#endif
    }

    public void DecacheColors()
    {
#if UNITY_EDITOR
        int id = -1;
        if (data != null)
        {
            foreach (var d in data)
            {
                id++;
                if (d != null)
                {
                    Type t = Type.GetType($"{d.className}");
                    try
                    {
                        var field2 = t.GetField("debugColor");
                        var color = colors[id];
                        field2.SetValue(t, color);
                        PlayerPrefs.SetFloat($"{d.className}.r", color.r);
                        PlayerPrefs.SetFloat($"{d.className}.g", color.g);
                        PlayerPrefs.SetFloat($"{d.className}.b", color.b);
                        PlayerPrefs.SetFloat($"{d.className}.a", color.a);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"{d.className} e");
                    }
                }
            }
        }
#endif
    }
}