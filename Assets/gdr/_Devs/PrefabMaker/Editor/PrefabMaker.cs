using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabMaker : EditorWindow
{
    public Object go;
    public Object folder;
    string prefabName = "Enter name";
    string createAt = "";
    public bool isZeroSmoothness;
    public int additionalAnimations;
    public Object[] animations;

    [MenuItem("Tools/Prefab Maker")]

    public static void ShowWindow()
    {
        GetWindow(typeof(PrefabMaker));
    }

    void OnGUI()
    {
        SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
        serializedObject.Update();

        ShowAutoFolderButtons();

        if (folder != null)
        {
            MainModel();

            if (go != null)
            {
                var goOldName = go.name;

                SettingsDraw();

                SerializedProperty serializedPropertyAnimations = serializedObject.FindProperty("animations");
                EditorGUILayout.PropertyField(serializedPropertyAnimations);

                string path = $"{createAt}/{prefabName}";

                bool isPrefabExist = AssetDatabase.IsValidFolder(path);
                if (!isPrefabExist)
                {
                    CreateButton(path, goOldName);

                    RenameAnimations();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Asset already exist");
                    EditorGUILayout.Toggle(isPrefabExist);
                    EditorGUILayout.EndHorizontal();

                    RenameAnimations();
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void ExtractMaterials(string assetPath, string destinationPath, string prefix)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(Material)
                                         select x;

        int id = 0;
        foreach (Object item in enumerable)
        {
            string path = System.IO.Path.Combine(destinationPath, $"{prefix}Material{id}") + ".mat";
            id++;
            // string path = System.IO.Path.Combine(destinationPath, item.name) + ".mat";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(path);
            }
        }

        WriteAndImportAsset(assetPath);

        foreach (string item2 in hashSet)
        {
            WriteAndImportAsset(item2);
            var asset = AssetDatabase.LoadAssetAtPath(item2, typeof(Material));
            if (isZeroSmoothness)
                ((Material)asset).SetFloat("_Smoothness", 0f);
        }
    }

    public void ExtractAnimations(string assetPath, string destinationPath, string defaultName = "", bool isOneF = true)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(AnimationClip)
                                         select x;

        bool isOne = enumerable.Count() <= 2 && isOneF;
        int id = 0;
        foreach (Object item in enumerable)
        {
            if (!item.name.Contains("__preview__"))
            {
                AnimationClip ac = new AnimationClip();

                EditorUtility.CopySerialized(item, ac);
                if (string.IsNullOrEmpty(defaultName))
                {
                    if (isOne)
                    {
                        if (id == 0)
                            AssetDatabase.CreateAsset(ac, $"{destinationPath}/{prefabName}.anim");
                        else
                            AssetDatabase.CreateAsset(ac, $"{destinationPath}/{prefabName}{id}.anim");
                    }
                    else
                        AssetDatabase.CreateAsset(ac, $"{destinationPath}/{item.name.Replace('|', '_')}.anim");
                }
                else
                {
                    if (isOne)
                    {
                        if (id == 0)
                            AssetDatabase.CreateAsset(ac, $"{destinationPath}/{defaultName}.anim");
                        else
                            AssetDatabase.CreateAsset(ac, $"{destinationPath}/{item.name.Replace('|', '_')}.anim");
                    }
                    else
                    {
                        AssetDatabase.CreateAsset(ac, $"{destinationPath}/{item.name.Replace('|', '_')}.anim");
                    }
                }
                id++;
            }
        }
    }

    public void ExtractTextures(string assetPath, string destinationPath)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(Texture)
                                         select x;

        foreach (Object item in enumerable)
        {
            string path = System.IO.Path.Combine(destinationPath, item.name) + $".{AssetDatabase.GetAssetPath(item).Split('.')[1]}";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(assetPath);
            }
        }

        foreach (string item2 in hashSet)
            WriteAndImportAsset(item2);
    }

    private static void WriteAndImportAsset(string assetPath)
    {
        AssetDatabase.WriteImportSettingsIfDirty(assetPath);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }

    private void ShowAutoFolderButtons()
    {
        GUILayout.Label("Prefab Folder", EditorStyles.boldLabel);
        folder = EditorGUILayout.ObjectField(folder, typeof(Object), true);
        createAt = AssetDatabase.GetAssetPath(folder);
        if (GUILayout.Button($"Try Auto Assets/_Prefabs"))
        {
            var autoObj = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs", typeof(Object));
            if (autoObj != null)
            {
                folder = autoObj;
            }
        }
        if (GUILayout.Button($"Try Auto Assets/_Prefabs/Entities"))
        {
            var autoObj = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Entities", typeof(Object));
            if (autoObj != null)
            {
                folder = autoObj;
            }
        }
    }

    private void MainModel()
    {
        GUILayout.Space(20);
        GUILayout.Label("Object Main Model", EditorStyles.boldLabel);
        go = EditorGUILayout.ObjectField(go, typeof(Object), true);
    }

    private void SettingsDraw()
    {
        GUILayout.Space(20);
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        prefabName = EditorGUILayout.TextField("Prefab Name:", prefabName);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Is Zero Material Smoothness", EditorStyles.label);
        isZeroSmoothness = EditorGUILayout.Toggle(isZeroSmoothness);
        GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Is Remove Animations From Model", EditorStyles.label);
        //GUILayout.EndHorizontal();

        // GUILayout.BeginHorizontal();
        GUILayout.Label("Add Animation Models", EditorStyles.boldLabel);
    }

    private void CreateButton(string path, string goOldName)
    {
        if (GUILayout.Button("Create"))
        {
            AssetDatabase.CreateFolder($"{createAt}", prefabName);
            var modelPath = AssetDatabase.GetAssetPath(go);
            var modelExtension = modelPath.Split('.')[1];
            Debug.Log($"Move from {modelPath} to {path}");
            var movedModelPath = $"{path}/{prefabName}Model.{modelExtension}";
            AssetDatabase.MoveAsset(modelPath, movedModelPath);

            AssetDatabase.CreateFolder($"{path}", "Materials");
            ExtractMaterials(movedModelPath, $"{path}/Materials", prefabName);
            AssetDatabase.CreateFolder($"{path}", "Animations");
            ExtractAnimations(movedModelPath, $"{path}/Animations", goOldName);

            if (animations != null)
            {
                foreach (var additionalAnim in animations)
                {
                    if (additionalAnim != null)
                    {
                        ExtractAnimations(AssetDatabase.GetAssetPath(additionalAnim), $"{path}/Animations", additionalAnim.name, false);
                    }
                }
            }

            try
            {
                AssetDatabase.CreateFolder($"{path}", "Textures");
                ExtractTextures(movedModelPath, $"{path}/Textures");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Texture bug: {e}");
            }

            var SceneObject = Instantiate(go) as GameObject;
            PrefabUtility.SaveAsPrefabAsset(SceneObject, $"{path}/{prefabName}.prefab");
            DestroyImmediate(SceneObject);

            AssetDatabase.Refresh();
        }
    }

    private void RenameAnimations()
    {
        if (go != null && GUILayout.Button("Rename animations"))
        {
            var movedModelPath = AssetDatabase.GetAssetPath(go);
            // var movedModelPath = $"{path}/{prefabName}Model.fbx";
            ModelImporter importer = AssetImporter.GetAtPath(movedModelPath) as ModelImporter;
            // Debug.Log(importer.clipAnimations.Length);
            // Debug.Log(importer.defaultClipAnimations.Length);
            var animas = importer.defaultClipAnimations;
            // animas.Foreach
            animas.ToList().ForEach(x => x.name = x.name.Replace("Armature|", ""));
            animas.ToList().ForEach(x => x.name = x.name.Replace("rig|", ""));
            animas.ToList().ForEach(x => x.name = x.name.Replace("metarig|", ""));
            animas.ToList().ForEach(x => x.name = x.name.Replace("Corset|", ""));
            importer.clipAnimations = animas;

            AssetDatabase.ImportAsset(movedModelPath);
        }
    }
}
