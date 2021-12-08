using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
// V1.5.3
// Dont replace with generic<T> classes. Unity will not serialize internal values!
/* Use example:
    [NaughtyAttributes.Button()]
    void GetNames()
    {
        DataTypes.GetNamesAll(this);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    [NaughtyAttributes.Button]
    void FindByNamesEverywhere()
    {
        DataTypes.FindByNamesAll(this);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    [NaughtyAttributes.Button]
    void FindByNamesInChildren()
    {
        DataTypes.FindByNamesChildren(name, this);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    [NaughtyAttributes.Button]
    void RenameObjectsLikeScriptNames()
    {
        DataTypes.RenameObjectsLikeScriptNames(this);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
*/

/*
 * MonoString use example (low typization):
   
    [Header("Panels")]

    [SerializeField]
    public MonoString fishSelectPanel; // This will be shown into inspector and rename panels

    private void Initialize() // Somewhere in UIManager
    {
        fishSelectPanel.Value<FishPanel>(); // You can use low bounded typization reference
    }

 * ScriptString and MonoString use example (strong typization):
 
    [Header("Panels")]

    [SerializeField]
    private MonoString fishSelectPanel; // This will be shown into inspector and rename panels
    public ScriptString<FishPanel> m_fishSelectPanel; // This is actual reference which is used in code with strong typization

    private void Initialize() // Somewhere in UIManager
    {
        m_fishSelectPanel = new ScriptString<FishPanel>(fishSelectPanel); // Recommend to use strong typization caused by no cast at all

        m_fishSelectPanel.Value.enabled = true; // Now you have access to script FishPanel without any casting
    }
*/
public static class DataTypes
{
    public static List<MonoBehaviour> m_scriptTypes;

    public static void GetNamesAll(object mono)
    {
        var bindingFlags = System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.NonPublic |
                   System.Reflection.BindingFlags.Public;
        var fieldValues = mono.GetType()
                             .GetFields(bindingFlags)
                             .Select(field => field.GetValue(mono))
                             .ToList();

        foreach (var item in fieldValues)
        {
            ButtonString buttonString = item as ButtonString;
            if (buttonString != null)
            {
                Debug.Log($"{item} SaveName");
                buttonString.SaveName();
                continue;
            }
            TextString textString = item as TextString;
            if (textString != null)
            {
                Debug.Log($"{item} SaveName");
                textString.SaveName();
                continue;
            }
            InputFieldString inputFieldString = item as InputFieldString;
            if (inputFieldString != null)
            {
                Debug.Log($"{item} SaveName");
                inputFieldString.SaveName();
                continue;
            }
            ToggleString toggleString = item as ToggleString;
            if (toggleString != null)
            {
                Debug.Log($"{item} SaveName");
                toggleString.SaveName();
                continue;
            }
            SliderString sliderString = item as SliderString;
            if (sliderString != null)
            {
                Debug.Log($"{item} SaveName");
                sliderString.SaveName();
                continue;
            }
            ImageString imageString = item as ImageString;
            if (imageString != null)
            {
                Debug.Log($"{item} SaveName");
                imageString.SaveName();
                continue;
            }
            //TmpTextString tmpTextString = item as TmpTextString;
            //if (tmpTextString != null)
            //{
            //    Debug.Log($"{item} SaveName");
            //    tmpTextString.SaveName();
            //    continue;
            //}
            AnimatorString animatorString = item as AnimatorString;
            if (animatorString != null)
            {
                Debug.Log($"{item} SaveName");
                animatorString.SaveName();
                continue;
            }
            GameObjectString gameObjectString = item as GameObjectString;
            if (gameObjectString != null)
            {
                Debug.Log($"{item} SaveName");
                gameObjectString.SaveName();
                continue;
            }
            MonoString monoString = item as MonoString;
            if (monoString != null)
            {
                Debug.Log($"{item} SaveName");
                monoString.SaveName();
                continue;
            }
            IList list = item as IList;
            if (list != null)
            {
                Debug.Log($"ListExtraction {item}");
                foreach (var jtem in list)
                    GetNamesAll(jtem);
                continue;
            }
            if (item != null && item.GetType().DeclaringType == mono.GetType())
            {
                Debug.Log($"SerializableClassExtraction {item}");
                GetNamesAll(item);
                continue;
            }
            Debug.Log($"{(item != null ? $"{item.GetType()}" : "???")} IgnoredSave");
        }
    }

    public static void FindByNamesAll(object mono)
    {
        var bindingFlags = System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.NonPublic |
                   System.Reflection.BindingFlags.Public;
        var fieldValues = mono.GetType()
                             .GetFields(bindingFlags)
                             .Select(field => field.GetValue(mono))
                             .ToList();

        foreach (var item in fieldValues)
        {
            ButtonString buttonString = item as ButtonString;
            if (buttonString != null)
            {
                Debug.Log($"{item} LoadByName");
                buttonString.LoadByName();
                continue;
            }
            TextString textString = item as TextString;
            if (textString != null)
            {
                Debug.Log($"{item} LoadByName");
                textString.LoadByName();
                continue;
            }
            InputFieldString inputFieldString = item as InputFieldString;
            if (inputFieldString != null)
            {
                Debug.Log($"{item} LoadByName");
                inputFieldString.LoadByName();
                continue;
            }
            ToggleString toggleString = item as ToggleString;
            if (toggleString != null)
            {
                Debug.Log($"{item} LoadByName");
                toggleString.LoadByName();
                continue;
            }
            SliderString sliderString = item as SliderString;
            if (sliderString != null)
            {
                Debug.Log($"{item} LoadByName");
                sliderString.LoadByName();
                continue;
            }
            ImageString imageString = item as ImageString;
            if (imageString != null)
            {
                Debug.Log($"{item} LoadByName");
                imageString.LoadByName();
                continue;
            }
            //TmpTextString tmpTextString = item as TmpTextString;
            //if (tmpTextString != null)
            //{
            //    Debug.Log($"{item} LoadByName");
            //    tmpTextString.LoadByName();
            //    continue;
            //}
            AnimatorString animatorString = item as AnimatorString;
            if (animatorString != null)
            {
                Debug.Log($"{item} LoadByName");
                animatorString.LoadByName();
                continue;
            }
            GameObjectString gameObjectString = item as GameObjectString;
            if (gameObjectString != null)
            {
                Debug.Log($"{item} LoadByName");
                gameObjectString.LoadByName();
                continue;
            }
            MonoString monoString = item as MonoString;
            if (monoString != null)
            {
                Debug.Log($"{item} LoadByName");
                monoString.LoadByName();
                continue;
            }
            IList list = item as IList;
            if (list != null)
            {
                Debug.Log($"ListExtraction {item}");
                foreach (var jtem in list)
                    FindByNamesAll(jtem);
                continue;
            }
            if (item != null && item.GetType().DeclaringType == mono.GetType())
            {
                Debug.Log($"SerializableClassExtraction {item}");
                FindByNamesAll(item);
                continue;
            }
            Debug.Log($"{(item != null ? $"{item.GetType()}" : "???")} IgnoredLoad");
        }
    }

    public static void FindByRootNamesChildren(string nameOfMono, object mono, string rootName = "")
    {
        var bindingFlags = System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.NonPublic |
                   System.Reflection.BindingFlags.Public;
        var fieldValues = mono.GetType()
                             .GetFields(bindingFlags)
                             .Select(field => field.GetValue(mono))
                             .ToList();
        var isRootNameOverride = (!string.IsNullOrEmpty(rootName));

        foreach (var item in fieldValues)
        {
            ButtonString buttonString = item as ButtonString;
            if (buttonString != null)
            {
                Debug.Log($"{item} LoadByName");
                buttonString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            TextString textString = item as TextString;
            if (textString != null)
            {
                Debug.Log($"{item} LoadByName");
                textString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            InputFieldString inputFieldString = item as InputFieldString;
            if (inputFieldString != null)
            {
                Debug.Log($"{item} LoadByName");
                inputFieldString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            ToggleString toggleString = item as ToggleString;
            if (toggleString != null)
            {
                Debug.Log($"{item} LoadByName");
                toggleString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            SliderString sliderString = item as SliderString;
            if (sliderString != null)
            {
                Debug.Log($"{item} LoadByName");
                sliderString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            ImageString imageString = item as ImageString;
            if (imageString != null)
            {
                Debug.Log($"{item} LoadByName");
                imageString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            //TmpTextString tmpTextString = item as TmpTextString;
            //if (tmpTextString != null)
            //{
            //    Debug.Log($"{item} LoadByName");
            //    tmpTextString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
            //    continue;
            //}
            AnimatorString animatorString = item as AnimatorString;
            if (animatorString != null)
            {
                Debug.Log($"{item} LoadByName");
                animatorString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            GameObjectString gameObjectString = item as GameObjectString;
            if (gameObjectString != null)
            {
                Debug.Log($"{item} LoadByName");
                gameObjectString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            MonoString monoString = item as MonoString;
            if (monoString != null)
            {
                Debug.Log($"{item} LoadByName");
                monoString.LoadByRootChildName(isRootNameOverride ? rootName : nameOfMono);
                continue;
            }
            IList list = item as IList;
            if (list != null)
            {
                Debug.Log($"ListExtraction {item}");
                foreach (var jtem in list)
                    FindByRootNamesChildren(isRootNameOverride ? rootName : nameOfMono, jtem);
                continue;
            }
            if (item != null && item.GetType().DeclaringType == mono.GetType())
            {
                Debug.Log($"SerializableClassExtraction {item}");
                FindByRootNamesChildren(isRootNameOverride ? rootName : nameOfMono, item);
                continue;
            }
            Debug.Log($"{(item != null ? $"{item.GetType()}" : "???")} IgnoredLoad");
        }
    }

    public static void FindByParentNamesChildren(string nameOfMono, object mono, string parentName = "")
    {
        var bindingFlags = System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.NonPublic |
                   System.Reflection.BindingFlags.Public;
        var fieldValues = mono.GetType()
                             .GetFields(bindingFlags)
                             .Select(field => field.GetValue(mono))
                             .ToList();
        var isParentNameOverride = (!string.IsNullOrEmpty(parentName));

        foreach (var item in fieldValues)
        {
            ButtonString buttonString = item as ButtonString;
            if (buttonString != null)
            {
                Debug.Log($"{item} LoadByName");
                buttonString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            TextString textString = item as TextString;
            if (textString != null)
            {
                Debug.Log($"{item} LoadByName");
                textString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            InputFieldString inputFieldString = item as InputFieldString;
            if (inputFieldString != null)
            {
                Debug.Log($"{item} LoadByName");
                inputFieldString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            ToggleString toggleString = item as ToggleString;
            if (toggleString != null)
            {
                Debug.Log($"{item} LoadByName");
                toggleString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            SliderString sliderString = item as SliderString;
            if (sliderString != null)
            {
                Debug.Log($"{item} LoadByName");
                sliderString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            ImageString imageString = item as ImageString;
            if (imageString != null)
            {
                Debug.Log($"{item} LoadByName");
                imageString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            //TmpTextString tmpTextString = item as TmpTextString;
            //if (tmpTextString != null)
            //{
            //    Debug.Log($"{item} LoadByName");
            //    tmpTextString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
            //    continue;
            //}
            AnimatorString animatorString = item as AnimatorString;
            if (animatorString != null)
            {
                Debug.Log($"{item} LoadByName");
                animatorString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            GameObjectString gameObjectString = item as GameObjectString;
            if (gameObjectString != null)
            {
                Debug.Log($"{item} LoadByName");
                gameObjectString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            MonoString monoString = item as MonoString;
            if (monoString != null)
            {
                Debug.Log($"{item} LoadByName");
                monoString.LoadByParentChildName(isParentNameOverride ? parentName : nameOfMono);
                continue;
            }
            IList list = item as IList;
            if (list != null)
            {
                Debug.Log($"ListExtraction {item}");
                foreach (var jtem in list)
                    FindByParentNamesChildren(isParentNameOverride ? parentName : nameOfMono, jtem);
                continue;
            }
            if (item != null && item.GetType().DeclaringType == mono.GetType())
            {
                Debug.Log($"SerializableClassExtraction {item}");
                FindByParentNamesChildren(isParentNameOverride ? parentName : nameOfMono, item);
                continue;
            }
            Debug.Log($"{(item != null ? $"{item.GetType()}" : "???")} IgnoredLoad");
        }
    }

    public static void RenameObjectsLikeScriptNames(object mono, string forListOnly = "")
    {
        var bindingFlags = System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.NonPublic |
                   System.Reflection.BindingFlags.Public;
        var fieldValues = mono.GetType()
                             .GetFields(bindingFlags)
                             .Select(field => field.GetValue(mono))
                             .ToList();
        var fields = mono.GetType()
                             .GetFields(bindingFlags);

        for (int i = 0; i < fieldValues.Count; i++)
        {
            ButtonString buttonString = fieldValues[i] as ButtonString;
            if (buttonString != null && buttonString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                buttonString.Value.name = newName;
                buttonString.m_nameOfObject = newName;
                buttonString.LoadByName();
                continue;
            }
            TextString textString = fieldValues[i] as TextString;
            if (textString != null && textString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                textString.Value.name = newName;
                textString.m_nameOfObject = newName;
                textString.LoadByName();
                continue;
            }
            InputFieldString inputFieldString = fieldValues[i] as InputFieldString;
            if (inputFieldString != null && inputFieldString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                inputFieldString.Value.name = newName;
                inputFieldString.m_nameOfObject = newName;
                inputFieldString.LoadByName();
                continue;
            }
            ToggleString toggleString = fieldValues[i] as ToggleString;
            if (toggleString != null && toggleString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                toggleString.Value.name = newName;
                toggleString.m_nameOfObject = newName;
                toggleString.LoadByName();
                continue;
            }
            SliderString sliderString = fieldValues[i] as SliderString;
            if (sliderString != null && sliderString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                sliderString.Value.name = newName;
                sliderString.m_nameOfObject = newName;
                sliderString.LoadByName();
                continue;
            }
            ImageString imageString = fieldValues[i] as ImageString;
            if (imageString != null && imageString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                imageString.Value.name = newName;
                imageString.m_nameOfObject = newName;
                imageString.LoadByName();
                continue;
            }
            //TmpTextString tmpTextString = fieldValues[i] as TmpTextString;
            //if (tmpTextString != null && tmpTextString.Value != null)
            //{
            //    string newName = fields[i].Name.Replace("m_", "") + forListOnly;
            //    if (newName.StartsWith("_"))
            //        newName = newName.Substring(1);
            //    newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
            //    Debug.Log(newName);
            //    tmpTextString.Value.name = newName;
            //    tmpTextString.m_nameOfObject = newName;
            //    tmpTextString.LoadByName();
            //    continue;
            //}
            AnimatorString animatorString = fieldValues[i] as AnimatorString;
            if (animatorString != null && animatorString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                animatorString.Value.name = newName;
                animatorString.m_nameOfObject = newName;
                animatorString.LoadByName();
                continue;
            }
            GameObjectString gameObjectString = fieldValues[i] as GameObjectString;
            if (gameObjectString != null && gameObjectString.Value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                gameObjectString.Value.name = newName;
                gameObjectString.m_nameOfObject = newName;
                gameObjectString.LoadByName();
                continue;
            }
            MonoString monoString = fieldValues[i] as MonoString;
            if (monoString != null && monoString.m_value != null)
            {
                string newName = fields[i].Name.Replace("m_", "") + forListOnly;
                if (newName.StartsWith("_"))
                    newName = newName.Substring(1);
                newName = $"{char.ToUpper(newName[0])}{newName.Substring(1)}";
                Debug.Log(newName);
                monoString.m_value.name = newName;
                monoString.m_nameOfObject = newName;
                monoString.LoadByName();
                continue;
            }
            IList list = fieldValues[i] as IList;
            if (list != null)
            {
                Debug.Log($"ListExtraction {fieldValues[i]}");
                int id = 0;
                foreach (var jtem in list)
                {
                    RenameObjectsLikeScriptNames(jtem, $" ({id})");
                    id++;
                }
                continue;
            }
            if (fieldValues[i] != null && fieldValues[i].GetType().DeclaringType == mono.GetType())
            {
                Debug.Log($"SerializableClassExtraction {fieldValues[i]}");
                RenameObjectsLikeScriptNames(fieldValues[i]);
                continue;
            }
            Debug.Log($"{(fieldValues[i] != null ? $"{fieldValues[i].GetType()}" : "???")} IgnoredRename");
        }
    }
}

[System.Serializable]
public class GameObjectString
{
    public string m_nameOfObject;
    public GameObject Value;
    public GameObject gameObject => Value;

    public Transform transform => Value.transform;

    public void SetActive(bool value)
    {
        Value.SetActive(value);
    }

    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

[System.Serializable]
public class AnimatorString
{
    public string m_nameOfObject;
    public Animator Value;

    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

[System.Serializable]
public class ButtonString
{
    public string m_nameOfObject;
    public Button Value;
    public Button.ButtonClickedEvent onClick => Value.onClick;
    private GameObject _gameObject;
    public GameObject gameObject => _gameObject == null ? _gameObject = Value.gameObject : _gameObject;
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

[System.Serializable]
public class TextString
{
    public string m_nameOfObject;
    public Text Value;
    public string text
    {
        get
        {
            return Value.text;
        }
        set
        {
            Value.text = value;
        }
    }
    private GameObject _gameObject;
    public GameObject gameObject => _gameObject == null ? _gameObject = Value.gameObject : _gameObject;
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

[System.Serializable]
public class InputFieldString
{
    public string m_nameOfObject;
    public InputField Value;

    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

[System.Serializable]
public class SliderString
{
    public string m_nameOfObject;
    public Slider Value;
    public Slider.SliderEvent onValueChanged
    {
        get
        {
            return Value.onValueChanged;
        }
        set
        {
            Value.onValueChanged = value;
        }
    }
    public float value
    {
        get
        {
            return Value.value;
        }
        set
        {
            Value.value = value;
        }
    }

    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

[System.Serializable]
public class ToggleString
{
    public string m_nameOfObject;
    public Toggle Value;
    public bool isOn
    {
        set
        {
            Value.isOn = value;
        }
        get
        {
            return Value.isOn;
        }
    }
    public Toggle.ToggleEvent onValueChanged
    {
        get
        {
            return Value.onValueChanged;
        }
        set
        {
            Value.onValueChanged = value;
        }
    }
    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

[System.Serializable]
public class ImageString
{
    public string m_nameOfObject;
    public Image Value;
    public float fillAmount
    {
        get
        {
            return Value.fillAmount;
        }
        set
        {
            Value.fillAmount = value;
        }
    }

    public void SaveName()
    {
        if (Value != null)
            m_nameOfObject = Value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
    }
}

//[System.Serializable]
//public class TmpTextString
//{
//    public string m_nameOfObject;
//    //public TMPro.TextMeshProUGUI Value;

//    public void SaveName()
//    {
//        if (Value != null)
//            m_nameOfObject = Value.name;
//        else if (string.IsNullOrEmpty(m_nameOfObject))
//            m_nameOfObject = "";
//    }
//    public void LoadByName()
//    {
//        if (!string.IsNullOrEmpty(m_nameOfObject))
//            CollectorScript.Instance.InitProperty(ref Value, m_nameOfObject);
//    }
//    public void LoadByRootChildName(string rootName)
//    {
//        if (!string.IsNullOrEmpty(m_nameOfObject))
//            CollectorScript.Instance.InitPropertyWhereRootIs(ref Value, m_nameOfObject, rootName);
//    }
//    public void LoadByParentChildName(string parentName)
//    {
//        if (!string.IsNullOrEmpty(m_nameOfObject))
//            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref Value, m_nameOfObject, parentName);
//    }
//}

[System.Serializable]
public class MonoString
{
    public string m_nameOfObject;
    public MonoBehaviour m_value;
    public T Value<T>() where T : MonoBehaviour
    {
        if (m_value == null)
            return null;
        return m_value as T;
    }

    public void SaveName()
    {
        if (m_value != null)
            m_nameOfObject = m_value.name;
        else if (string.IsNullOrEmpty(m_nameOfObject))
            m_nameOfObject = "";
    }
    public void LoadByName()
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitProperty(ref m_value, m_nameOfObject);
    }
    public void LoadByRootChildName(string rootName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereRootIs(ref m_value, m_nameOfObject, rootName);
    }
    public void LoadByParentChildName(string parentName)
    {
        if (!string.IsNullOrEmpty(m_nameOfObject))
            CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref m_value, m_nameOfObject, parentName);
    }
}

public class ScriptString<T> : MonoString where T : MonoBehaviour
{
    public ScriptString(MonoString monoString)
    {
        m_nameOfObject = monoString.m_nameOfObject;
        m_value = monoString.m_value;
    }
    public T Value
    {
        get
        {
            if (m_value == null)
                return null;
            return m_value as T;
        }
    }
}