using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyEditorSelector : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    public System.Func<int, int> SomeInt => i => i; // Delegate with return type
    public System.Func<int, int> SomeInt2 = k => k; // Delegate with return type
    public GameObject m_objectWithScriptComponentsToInspect;

    string m_nameOfItem;
    string m_anyParentName;
    bool m_toggleParentType;
    Object m_parentObj;
    List<GameObject> foundGameObjects;

    [MenuItem("Window/EditorSelector")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MyEditorSelector));
        System.Func<int, int> SomeFunc = (x) => { return x; };
    }

    void OnGUI()
    {
        if (CollectorScript.Instance == null)
        {
            EditorGUILayout.HelpBox("You must use CollectorScript", MessageType.Error);
        }
        else
        {
            /*
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            myString = EditorGUILayout.TextField("Text Field", myString);

            groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            myBool = EditorGUILayout.Toggle("Toggle", myBool);
            myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            EditorGUILayout.EndToggleGroup();

            if (GUILayout.Button("Debug"))
                Debug.Log("Debug is pressed!");
            */

            m_nameOfItem = EditorGUILayout.TextField("Name of item (or part of it)", m_nameOfItem);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use name or gameObject of parent?", GUILayout.Width(240f));
            m_toggleParentType = EditorGUILayout.Toggle(m_toggleParentType);
            EditorGUILayout.EndHorizontal();
            if (m_toggleParentType)
                m_anyParentName = EditorGUILayout.TextField("Name of parent (or part of it)", m_anyParentName);
            else
            {
                m_parentObj = EditorGUILayout.ObjectField(m_parentObj, typeof(Object), true) as GameObject;
                if (m_parentObj != null)
                {
                    m_anyParentName = m_parentObj.name;
                    EditorGUILayout.LabelField($"Current parent name: {m_anyParentName}");
                }
            }

            if (GUILayout.Button("Select them all"))
            {
                if (!string.IsNullOrEmpty(m_nameOfItem))
                {
                    CollectorScript.Instance.Reinitialize();
                    foundGameObjects = CollectorScript.Instance.FindAllWhereAnyParentIs(m_nameOfItem, m_anyParentName);
                    Debug.Log($"Found: {foundGameObjects.Count} objects");
                    Selection.objects = foundGameObjects.ToArray();
                }
                else
                    Debug.Log("You must enter name of object you want to find");
            }
        }
    }
}
