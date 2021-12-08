using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Unity.Jobs;
using UnityEditor.Animations;

public class EasyLevelMaker : EditorWindow
{
    public int week = 1;
    public int part = 1;
    public Object animation;
    public AnimationClip oldAnim;

    public AnimationClip womanMain, womanLoop;
    public AnimatorController animator;

    [MenuItem("Tools/EasyLevelMaker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EasyLevelMaker));
    }

    public void OnGUI()
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);

        SerializedProperty week = so.FindProperty("week");
        SerializedProperty part = so.FindProperty("part");

        var level = week.intValue * 3 + part.intValue - 3;
        EditorGUILayout.LabelField($"Level {level}");

        EditorGUILayout.PropertyField(week, new GUIContent("Week"), true);

        EditorGUILayout.PropertyField(part, new GUIContent("Part"), true);

        //string womanAnimatorPath = $"{Application.dataPath}/_Animations/LevelsPart2/Woman/Animators";
        //EditorGUILayout.LabelField($"WomanAnimatorPath {womanAnimatorPath}");

        //string womanAnimationsPath = $"{Application.dataPath}/_Animations/LevelsPart2/Woman/Animations";
        //EditorGUILayout.LabelField($"WomanAnimationsPath {womanAnimatorPath}");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Main");
        EditorGUILayout.TextField($"_sc{level}_{week.intValue}-{part.intValue}_Main");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Loop");
        EditorGUILayout.TextField($"_sc{level}_{week.intValue}-{part.intValue}_Loop");
        EditorGUILayout.EndHorizontal();

        SerializedProperty model = so.FindProperty("animation");
        EditorGUILayout.PropertyField(model, new GUIContent("WorkAnimation"), true);

        if (model.objectReferenceValue != oldAnim)
        {
            if (oldAnim != null)
            {
                week.intValue = 0;
                part.intValue = 0;
            }
            oldAnim = model.objectReferenceValue as AnimationClip;
        }

        if (model.objectReferenceValue != null)
        {
            if (model.objectReferenceValue as AnimationClip != null && part.intValue != 0 && week.intValue != 0)
            {
                if (GUILayout.Button("Test & Fix Animation"))
                {
                    if (model.objectReferenceValue != null)
                    {
                        if (model.objectReferenceValue as AnimationClip != null)
                        {
                            var animation = model.objectReferenceValue;
                            List<string> listStrings = new List<string>();

                            List<string> searchPrefixes = new List<string>
                            {
                                "Base ",
                                "main_woman_NewR:",
                                "baby_rig:",
                                "girl_teen:",
                                "hans:",
                                "loretta:",
                                "unknown_character:",
                                "conrad:"
                            };
                            var path = AssetDatabase.GetAssetPath(animation);
                            path = path.Substring(6);
                            path = Application.dataPath + path;
                            string msg;
                            foreach (var brokeString in searchPrefixes)
                            {
                                string prefix = brokeString;
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
                                    //if (item.Contains("2.5"))
                                    //    listStrings[_id] = item.Replace("2.5", "1.0");

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
                                            oneLineString = oneLineString.Replace("HumanPelvis1", "HumanPelvis");
                                            oneLineString = oneLineString.Replace("HumanPelvis2", "HumanPelvis");
                                            oneLineString = oneLineString.Replace(prefix, "");
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
                                AssetDatabase.Refresh();
                            }
                        }
                    }
                    else
                        Debug.LogWarning($"Set animation first!");
                }

                if (GUILayout.Button("Rename as Main"))
                {
                    string newName = $"woman_sc{level}_{week.intValue}-{part.intValue}_Main";
                    string oldPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(model.objectReferenceValue);
                    string newPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(model.objectReferenceValue).Replace(model.objectReferenceValue.name, newName);
                    System.IO.File.Move(oldPath, newPath);
                    AssetDatabase.Refresh();
                    model.objectReferenceValue = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(Object));
                    oldAnim = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(AnimationClip)) as AnimationClip;
                }

                if (GUILayout.Button("Rename as Loop"))
                {
                    string newName = $"woman_sc{level}_{week.intValue}-{part.intValue}_Loop";
                    string oldPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(model.objectReferenceValue);
                    string newPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(model.objectReferenceValue).Replace(model.objectReferenceValue.name, newName);
                    System.IO.File.Move(oldPath, newPath);
                    AssetDatabase.Refresh();
                    model.objectReferenceValue = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(Object));
                    oldAnim = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(AnimationClip)) as AnimationClip;
                }

                if (GUILayout.Button("Rename as MainLoop"))
                {
                    string newName = $"woman_sc{level}_{week.intValue}-{part.intValue}_MainLoop";
                    string oldPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(model.objectReferenceValue);
                    string newPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(model.objectReferenceValue).Replace(model.objectReferenceValue.name, newName);
                    System.IO.File.Move(oldPath, newPath);
                    AssetDatabase.Refresh();
                    model.objectReferenceValue = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(Object));
                    oldAnim = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(AnimationClip)) as AnimationClip;
                }

                //if (GUILayout.Button("Send to folder woman Animations"))
                //{
                //    if (model.objectReferenceValue != null)
                //    {
                //        string newName = model.objectReferenceValue.name;
                //        string tPath = $"{Application.dataPath}/_Animations/LevelsPart2/Woman/Animations";
                //        string mainPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(model.objectReferenceValue);
                //        string newPath = $"{tPath}/{newName}.anim";
                //        Debug.Log($"Move from {mainPath} to {newPath}");
                //        System.IO.File.Move(mainPath, $"{newPath}");
                //        AssetDatabase.Refresh();
                //        model.objectReferenceValue = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(Object));
                //        oldAnim = AssetDatabase.LoadAssetAtPath($"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}", typeof(Object)) as AnimationClip;
                //    }

                //    // System.IO.File.Move();
                //}
            }
            else if (model.objectReferenceValue as GameObject != null)
            {
                if (GUILayout.Button("Test Model Animation Bones"))
                {
                    List<string> searchPrefixes = new List<string>
                {
                    "Base ",
                    "main_woman_NewR:",
                    "baby_rig:",
                    "girl_teen:",
                     "hans:",
                     "loretta:",
                     "unknown_character:",
                     "conrad:"
                };
                    var go = (GameObject)model.objectReferenceValue;
                    var trs = go.GetComponentsInChildren<Transform>();
                    bool isBroken = false;
                    string logString = "";
                    string brokeString = "";
                    foreach (var exc in searchPrefixes)
                    {
                        foreach (Transform tr in trs)
                        {
                            if (tr.gameObject.name.Contains(exc))
                            {
                                logString += $"{tr.gameObject.name} is invalid\n";
                                isBroken = true;
                                brokeString = exc;
                            }
                        }
                    }
                    if (isBroken)
                        Debug.LogWarning($"Model : {go.name}, this model animation require AnimationRenamer pass\n{logString}");
                    else
                        Debug.Log($"Model : {go.name} is fine");
                }
            }
        }
        EditorGUILayout.HelpBox("Next part is for fine models only", MessageType.Info);

        SerializedProperty womanMain = so.FindProperty("womanMain");
        EditorGUILayout.PropertyField(womanMain, new GUIContent("WomanMainAnimation"), true);

        SerializedProperty womanLoop = so.FindProperty("womanLoop");
        EditorGUILayout.PropertyField(womanLoop, new GUIContent("WomanLoopAnimation"), true);

        string womanAnimationsPath = $"{Application.dataPath}/_Animations/LevelsPart2/Woman/Animations";

        SerializedProperty animator = so.FindProperty("animator");
        EditorGUILayout.PropertyField(animator, new GUIContent("Animator"), true);

        if (womanMain.objectReferenceValue != null && week.intValue != 0 && part.intValue != 0)
        {
            if (GUILayout.Button("Send to folder woman Animations"))
            {
                if (animator.objectReferenceValue != null && animator.objectReferenceValue as AnimatorController != null)
                {
                    if (womanMain.objectReferenceValue != null)
                    {
                        string newName;
                        if (womanLoop.objectReferenceValue != null)
                            newName = $"woman_sc{level}_{week.intValue}-{part.intValue}_Main.anim";
                        else
                            newName = $"woman_sc{level}_{week.intValue}-{part.intValue}_MainLoop.anim";
                        string mainPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(womanMain.objectReferenceValue);
                        string newPath = $"{womanAnimationsPath}/{newName}";
                        Debug.Log($"Move from {mainPath} to {newPath}");
                        System.IO.File.Move(mainPath, newPath);
                        string assetPath = $"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}";
                        AssetDatabase.Refresh();
                        womanMain.objectReferenceValue = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip));
                    }

                    if (womanLoop.objectReferenceValue != null)
                    {
                        string newName = $"woman_sc{level}_{week.intValue}-{part.intValue}_Loop.anim";
                        string loopPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(womanLoop.objectReferenceValue);
                        string newPath = $"{womanAnimationsPath}/{newName}";
                        System.IO.File.Move(loopPath, newPath);
                        string assetPath = $"Assets/{newPath.Replace("Assets/", "\n").Split('\n')[1]}";
                        AssetDatabase.Refresh();
                        womanLoop.objectReferenceValue = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip));
                    }


                    string aNewName = $"woman_sc{level}_{week.intValue}-{part.intValue}";
                    string aOldPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(animator.objectReferenceValue);
                    string aNewPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(animator.objectReferenceValue).Replace(animator.objectReferenceValue.name, aNewName);
                    System.IO.File.Move(aOldPath, aNewPath);
                    AssetDatabase.Refresh();
                    string aAssetPath = $"Assets/{aNewPath.Replace("Assets/", "\n").Split('\n')[1]}";
                    AssetDatabase.Refresh();
                    animator.objectReferenceValue = AssetDatabase.LoadAssetAtPath(aAssetPath, typeof(AnimatorController));

                    var anim = animator.objectReferenceValue as AnimatorController;
                    var root = anim.layers[0].stateMachine;
                    var states = root.states;
                    foreach (var cas in states)
                    {
                        // Debug.Log(cas.state);
                        if (cas.state.name.Contains("Level"))
                        {
                            cas.state.name = $"Level {level}";
                            cas.state.motion = womanMain.objectReferenceValue as AnimationClip;
                        }
                        else
                        {
                            if (womanLoop.objectReferenceValue != null)
                                cas.state.motion = womanLoop.objectReferenceValue as AnimationClip;
                            else
                                cas.state.motion = womanMain.objectReferenceValue as AnimationClip;
                        }
                    }

                    AssetDatabase.Refresh();
                }
                // System.IO.File.Move();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"Send to {womanAnimationsPath}");

        if (womanMain.objectReferenceValue != null)
        {
            string mainPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(womanMain.objectReferenceValue);
            EditorGUILayout.LabelField($"WomanMainPath {mainPath}");
        }

        if (womanLoop.objectReferenceValue != null)
        {
            string loopPath = Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(womanLoop.objectReferenceValue);
            EditorGUILayout.LabelField($"WomanLoopPath {loopPath}");
        }

        so.ApplyModifiedProperties();

    }
}
