using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AnimationsRenamer : EditorWindow
{
    // public GameObject model;
    public List<Object> animations;
    public string prefix = "Base ";
    public string changeTo = "";
    private List<string> listStrings = new List<string>();

    [MenuItem("Tools/AnimationsRenamer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationsRenamer));
    }

    public void OnGUI()
    {
        // GUILayout.Label("MyLabel");
        // model = EditorGUILayout.ObjectField(model, typeof(GameObject), true) as GameObject;
        EditorGUILayout.LabelField("Insert extracted animations here");
        EditorGUILayout.LabelField("Reads all data from file animation");
        EditorGUILayout.LabelField("Replaces prefixes with empty space");

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty prefixSP = so.FindProperty("prefix");
        EditorGUILayout.PropertyField(prefixSP, new GUIContent("Prefix"), true);

        SerializedProperty chaneSP = so.FindProperty("changeTo");
        EditorGUILayout.PropertyField(chaneSP, new GUIContent("ChangeTo"), true);

        SerializedProperty list = so.FindProperty("animations");
        EditorGUILayout.PropertyField(list, new GUIContent("Animations"), true);

        if (GUILayout.Button("TestPathToAnimations"))
        {
            foreach (var animation in animations)
            {
                listStrings.Clear();
                var path = AssetDatabase.GetAssetPath(animation);
                path = path.Substring(6);
                path = Application.dataPath + path;
                Debug.Log(path);
                string msg;
                using (var file = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            msg = streamReader.ReadLine();
                            listStrings.Add(msg);
                        }
                    }
                }
                bool _isPathReaded = false;
                List<string> _readedStrings = new List<string>();
                int _pathID = 0;
                int _id = 0;
                foreach (var item in listStrings.ToArray())
                {
                    if (!_isPathReaded)
                    {
                        if (item.Contains("path:"))
                        {
                            _pathID = _id;
                            _readedStrings.Clear();
                            _readedStrings.Add(item);
                            _isPathReaded = true;
                        }
                    }
                    else
                    {
                        if (item.Contains(":"))
                        {
                            string oneLineString = "";
                            // PARSE
                            foreach (var readedString in _readedStrings)
                                oneLineString += readedString;
                            //oneLineString = oneLineString.Replace("HumanPelvis1", "HumanPelvis");
                            //oneLineString = oneLineString.Replace("HumanPelvis2", "HumanPelvis");
                            //oneLineString = oneLineString.Replace("unknow_character___2", "unknow_character");
                            oneLineString = oneLineString.Replace(prefix, changeTo);       
                            //oneLineString = oneLineString.Replace(" ", "");
                            //oneLineString = oneLineString.Replace("path:", "    path: ");
                            //oneLineString = oneLineString.Replace("001", "");
                            //oneLineString += "\n";
                            listStrings[_pathID] = oneLineString;
                            for (int i = _pathID + 1; i < _id; i++)
                                listStrings[i] = "";
                            _isPathReaded = false;
                        }
                        else
                        {
                            _readedStrings.Add(item);
                        }
                    }
                    _id++;
                }
                listStrings.RemoveAll((x) => { return x == ""; });
                using (var file = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter streamWriter = new StreamWriter(file))
                    {
                        foreach (var _string in listStrings)
                        {
                            streamWriter.WriteLine(_string);
                        }
                    }
                }
            }
        }
        so.ApplyModifiedProperties();
    }
}
