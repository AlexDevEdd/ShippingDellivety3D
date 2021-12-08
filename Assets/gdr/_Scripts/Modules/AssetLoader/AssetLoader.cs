#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetLoader<T> where T : Object
{
    public static List<T> GetItems(Object folder)
    {
        var items = new Item<T>();
        items.LoadAssets(folder);
        return items.items;
    }

    public class Item<T> where T : Object
    {
        public List<T> items;
        public void LoadAssets(Object folder)
        {
            items = new List<T>();
            var path = UnityEditor.AssetDatabase.GetAssetPath(folder);
            string[] guids = UnityEditor.AssetDatabase.FindAssets("", new string[] { path });
            foreach (var guid in guids)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var data = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
                items.Add(data);
            }
        }
    }
}
#endif