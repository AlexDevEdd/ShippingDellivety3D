using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolModule
{
    public static bool isDebug;
    public static Color debugColor;

    public static Dictionary<string, GameObject> prefabRegistry;
    public static Dictionary<string, Queue<GameObject>> pool;

    public static void Clear(bool isDestroyPoolGOs)
    {
        if (isDebug) DLog.D($"Clear {isDestroyPoolGOs}");
        prefabRegistry = new Dictionary<string, GameObject>();
        if (pool != null)
        {
            if (isDestroyPoolGOs)
            {
                foreach (var item in pool)
                {
                    while (item.Value.Count > 0)
                    {
                        var go = item.Value.Dequeue();
                        GameObject.Destroy(go);
                    }
                }
            }
        }
        pool = new Dictionary<string, Queue<GameObject>>();
    }

    public static void RegisterPrefab(GameObject prefab)
    {
        if (isDebug) DLog.D($"RegisterPrefab {prefab.name}");
        if (prefab.name.Contains("(Clone)"))
        {
            Debug.LogError($"Make sure you are adding prefab without (Clone) postfix here. What was passed: {prefab.name}");
            return;
        }
        if (prefabRegistry == null)
            prefabRegistry = new Dictionary<string, GameObject>();
        prefabRegistry.Add($"{prefab.name}(Clone)", prefab);
    }

    public static void Pool(GameObject go)
    {
        if (isDebug) DLog.D($"Pool {go.name}");
        bool isExist = IsPrefabRegistryContainsGameObject(go);
        if (!isExist)
            return;

        MakeSurePoolIsInitialized();

        bool isFirstKeyElement = IsFirstOccuranceInPool(go);
        if (isFirstKeyElement)
            AddFirstKey(go);
        else
            AddToExistingPool(go);
        go.SetActive(false);
    }

    public static GameObject Get(GameObject go)
    {
        return Get(go.name);
    }

    public static GameObject Get(string goName)
    {
        bool isUsePrefabName = !goName.Contains($"(Clone)");
        if (isDebug) DLog.D($"Get {goName}, {isUsePrefabName}");
        if (isUsePrefabName)
            goName = $"{goName}(Clone)";
        bool isExist = IsPrefabRegistryContainsGameObject(goName);
        if (!isExist)
            return null;

        MakeSurePoolIsInitialized();

        bool isFirstKeyElement = IsFirstOccuranceInPool(goName);
        if (isFirstKeyElement)
            return InstantiateNew(goName);
        bool isPoolHasAnyFree = pool[goName].Count > 0;
        if (isPoolHasAnyFree)
            return GetFromPool(goName);
        else
            return InstantiateNew(goName);
    }

    private static bool IsPrefabRegistryContainsGameObject(GameObject go)
    {
        return IsPrefabRegistryContainsGameObject(go.name);
    }

    private static bool IsPrefabRegistryContainsGameObject(string goName)
    {
        if (isDebug) DLog.D($"IsPrefabRegistryContainsGameObject {goName}");
        if (!prefabRegistry.ContainsKey(goName))
        {
            Debug.LogError($"Please Register({goName}) prefab before use it in pool system");
            return false;
        }
        return true;
    }

    private static void MakeSurePoolIsInitialized()
    {
        if (isDebug) DLog.D($"MakeSurePoolIsInitialized");
        if (pool == null)
            pool = new Dictionary<string, Queue<GameObject>>();
    }

    private static bool IsFirstOccuranceInPool(GameObject go)
    {
        return IsFirstOccuranceInPool(go.name);
    }

    private static bool IsFirstOccuranceInPool(string goName)
    {
        if (isDebug) DLog.D($"IsFirstOccuranceInPool {goName}");
        return !pool.ContainsKey(goName);
    }

    private static void AddFirstKey(GameObject go)
    {
        if (isDebug) DLog.D($"AddFirstKey {go.name}");
        var queue = new Queue<GameObject>();
        queue.Enqueue(go);
        pool.Add(go.name, queue);
    }

    private static void AddToExistingPool(GameObject go)
    {
        if (isDebug) DLog.D($"AddToExistingPool {go.name}");
        pool[go.name].Enqueue(go);
    }

    private static GameObject InstantiateNew(string goName)
    {
        if (isDebug) DLog.D($"InstantiateNew {goName}");
        var prefab = prefabRegistry[goName];
        GameObject go = GameObject.Instantiate(prefab, null);
        go.SetActive(true);
        return go;
    }

    private static GameObject GetFromPool(string goName)
    {
        GameObject go = pool[goName].Dequeue();
        go.SetActive(true);
        return go;
    }
}

