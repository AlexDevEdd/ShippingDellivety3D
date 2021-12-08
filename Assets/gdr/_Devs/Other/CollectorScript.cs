using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The main purpose of this script - to be able collect gameObject-s at startup even with SetActive(false) value. V2.6.1
public class CollectorScript : MonoBehaviour
{
    #region Singleton Init
    private static CollectorScript _instance;

    void Awake() // Init in order
    {
        Init();
        //if (_instance == null)
        //    Init();
        //else if (_instance != this)
        //{
        //    Debug.Log("IsDestroyed");
        //    Destroy(gameObject);
        //}
    }

    public static CollectorScript Instance // Init not in order
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
        _instance = FindObjectOfType<CollectorScript>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    List<GameObject> m_allGameObjects;
    Dictionary<string, GameObject> m_allGameObjectsDictionary;

    void Initialize()
    {
        m_allGameObjects = new List<GameObject>();
        m_allGameObjects.AddRange(Resources.FindObjectsOfTypeAll<GameObject>());
        m_allGameObjects.RemoveAll((x) => x.hideFlags != HideFlags.None);
        m_allGameObjectsDictionary = new Dictionary<string, GameObject>();
        foreach (var item in m_allGameObjects)
        {
            if (!m_allGameObjectsDictionary.ContainsKey(item.name) && item.scene.name != null)
                m_allGameObjectsDictionary.Add(item.name, item);
        }
        // Init data here
        if (isDebug) DLog.D($"CollectorScript is Initialized");
        enabled = true;
    }

    //[NaughtyAttributes.Button]
    //private void DestroyThatThing()
    //{
    //    var allGameObjects = new List<GameObject>();
    //    allGameObjects.AddRange(Resources.FindObjectsOfTypeAll<GameObject>());
    //    if (allGameObjects.Find((x) => x.hideFlags == HideFlags.HideInInspector) != null)
    //    {
    //        Debug.Log("Has one");
    //    }
    //}

    [NaughtyAttributes.Button("Re-initialize")]
    public void Reinitialize()
    {
        Initialize();
    }

    public T[] GetSceneGameObjects<T>()
    {
        List<T> collection = new List<T>();
        foreach (var item in m_allGameObjects)
            collection.Add(item.GetComponent<T>());
        collection.RemoveAll(x => x == null); // Filter output
        return collection.ToArray();
    }

    public GameObject[] GetSceneGameObjects(string _objectNameInScene)
    {
        List<GameObject> collection = new List<GameObject>();
        foreach (var item in m_allGameObjects)
            if (item.name.Equals(_objectNameInScene))
                collection.Add(item);
        return collection.ToArray();
    }

    public T[] GetSceneGameObjects<T>(string _objectNameInScene)
    {
        List<T> collection = new List<T>();
        foreach (var item in m_allGameObjects)
            if (item.name.Equals(_objectNameInScene))
                collection.Add(item.GetComponent<T>());
        collection.RemoveAll(x => x == null); // Filter output
        return collection.ToArray();
    }

    public GameObject[] GetSceneGameObjectsNameContain(string _objectNameContainInScene)
    {
        List<GameObject> collection = new List<GameObject>();
        foreach (var item in m_allGameObjects)
            if (item.name.Contains(_objectNameContainInScene))
                collection.Add(item);
        collection.RemoveAll(x => x == null); // Filter output
        return collection.ToArray();
    }

    public GameObject GetSceneGameObject(string _objectNameInScene)
    {
        if (m_allGameObjectsDictionary.ContainsKey(_objectNameInScene))
            return m_allGameObjectsDictionary[_objectNameInScene];
        if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        return null;
    }

    public GameObject GetSceneGameObject(string _objectNameInScene, string _rootName, string _parentName)
    {
        foreach (var item in m_allGameObjects)
        {
            if (item.name.Equals(_objectNameInScene) && item.transform.root.name.Equals(_rootName) && item.transform.parent.name.Equals(_parentName))
            {
                return item;
            }
            /*
            else if (item.name.Equals(_objectNameInScene))
                Debug.Log($"{item.name} {item.transform.root.name} != {_rootName} ->{item.transform.root.name.Equals(_rootName)}<- {item.transform.parent.name} != {_parentName} ->{item.transform.parent.name.Equals(_parentName)}<-");
            */
        }
        if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        return null;
    }

    public T GetSceneGameObject<T>()
    {
        m_allGameObjects.RemoveAll(x => x == null);
        foreach (var item in m_allGameObjects)
            if (item.GetComponent<T>() != null)
                    return item.GetComponent<T>();
        return default;
    }

    public T GetSceneGameObject<T>(string _objectNameInScene)
    {
        if (m_allGameObjectsDictionary.ContainsKey(_objectNameInScene))
            return m_allGameObjectsDictionary[_objectNameInScene].GetComponent<T>();
        if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        return default;
    }

    public void InitProperty(ref GameObject _property, string _propertyName)
    {
        if (_property == null)
        {
            _property = GetSceneGameObject(_propertyName);
            if (_property == null)
            if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        }
    }

    public void InitProperty(ref Transform _property, string _propertyName)
    {
        if (_property == null)
        {
            _property = GetSceneGameObject<Transform>(_propertyName);
            if (_property == null)
                if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        }
    }

    public void InitProperty(ref RectTransform _property, string _propertyName)
    {
        if (_property == null)
        {
            _property = GetSceneGameObject<RectTransform>(_propertyName);
            if (_property == null)
                if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        }
    }

    public void InitProperty(ref Rigidbody _property, string _propertyName)
    {
        if (_property == null)
        {
            _property = GetSceneGameObject<Rigidbody>(_propertyName);
            if (_property == null)
                if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        }
    }

    public void InitProperty<T>(ref T _property, string _propertyName)
    {
        if (_property == null || _property.Equals(default(T)))
        {
            _property = GetSceneGameObject<T>(_propertyName);
            if (_property == null)
                if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
        }
        else
                if (isDebug) DLog.D($"Already initialized property didnt inited");
    }

    public void InitPropertyWhereRootIs<T>(ref T _property, string _propertyName, string _rootName)
    {
        if (_property == null)
        {
            Transform[] sceneInstances = GetSceneGameObjects<Transform>(_propertyName);
            foreach (var item in sceneInstances)
            {
                if (item.transform.root.name.Equals(_rootName))
                {
                    _property = item.GetComponent<T>();
                    if (_property == null)
                        if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
                    return;
                }
            }
        }
    }

    public void InitPropertyWhereAnyParentIs(ref GameObject _property, string _propertyName, string _parentName)
    {
        if (_property == null)
        {
            Transform[] sceneInstances = GetSceneGameObjects<Transform>(_propertyName);
            foreach (var item in sceneInstances)
            {
                Transform _parent = item.transform.parent;
                while (_parent != null)
                {
                    if (_parent.name.Equals(_parentName))
                    {
                        _property = item.gameObject;
                        if (_property == null)
                            if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
                        return;
                    }
                    else
                        _parent = _parent.parent;
                }
            }
        }
    }

    public void InitPropertyWhereAnyParentIs<T>(ref T _property, string _propertyName, string _parentName)
    {
        if (_property == null || _property.GetType() == typeof(Transform))
        {
            Transform[] sceneInstances = GetSceneGameObjects<Transform>(_propertyName);
            foreach (var item in sceneInstances)
            {
                Transform _parent = item.transform.parent;
                while (_parent != null)
                {
                    if (_parent.name.Equals(_parentName))
                    {
                        _property = item.GetComponent<T>();
                        if (_property == null)
                            if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
                        return;
                    }
                    else
                        _parent = _parent.parent;
                }
            }
        }
    }

    public List<GameObject> FindAllWhereAnyParentIs(string _propertyName, string _parentName)
    {
        GameObject[] sceneInstances = GetSceneGameObjectsNameContain(_propertyName);
        List<GameObject> m_result = new List<GameObject>();
        if (string.IsNullOrEmpty(_parentName))
        {
            m_result.AddRange(sceneInstances);
            return m_result;
        }
        else
        {
            foreach (var item in sceneInstances)
            {
                Transform _parent = item.transform.parent;
                while (_parent != null)
                {
                    if (_parent.name.Contains(_parentName))
                    {
                        m_result.Add(item);
                        _parent = null;
                    }
                    else
                        _parent = _parent.parent;
                }
            }
            return m_result;
        }
    }

    public void InitPropertyWhereRootParentIs<T>(ref T _property, string _propertyName, string _rootName, string _parentName)
    {
        if (_property == null)
        {
            Transform[] sceneInstances = GetSceneGameObjects<Transform>(_propertyName);
            foreach (var item in sceneInstances)
            {
                if (item.transform.root.name.Equals(_rootName) && item.transform.parent.Equals(_parentName))
                {
                    _property = item.GetComponent<T>();
                    if (_property == null)
                        if (isDebug) DLog.D($"Probably you must Re-initialize Collector through context menu");
                    return;
                }
            }
        }
    }
}
