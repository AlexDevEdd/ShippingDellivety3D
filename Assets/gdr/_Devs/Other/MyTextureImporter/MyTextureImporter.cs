#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyTextureImporter : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public int maxSize;
        public Vector2 pixelMustBeInRange;
    }
    public List<Data> data;
    public List<Texture2D> textures;

    [NaughtyAttributes.Button]
    void InitData()
    {
        data.Clear();
        data.Add(new Data() { maxSize = 2048, pixelMustBeInRange = new Vector2(1101, 9999) });
        data.Add(new Data() { maxSize = 1024, pixelMustBeInRange = new Vector2(541, 1100) });
        data.Add(new Data() { maxSize = 512, pixelMustBeInRange = new Vector2(281, 540) });
        data.Add(new Data() { maxSize = 256, pixelMustBeInRange = new Vector2(141, 280) });
        data.Add(new Data() { maxSize = 128, pixelMustBeInRange = new Vector2(71, 140) });
        data.Add(new Data() { maxSize = 64, pixelMustBeInRange = new Vector2(0, 70) });
    }

    [NaughtyAttributes.Button]
    void MakeMax()
    {
        foreach (var item in textures.ToArray())
        {
            string path = AssetDatabase.GetAssetPath(item);
            Debug.Log(path);

            if (path.Contains(".ttf") || path.Contains(".otf") || path.Contains(".asset"))
            {
                textures.Remove(item);
                continue;
            }

            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);

            importer.maxTextureSize = 2048;
        }
    }

    [NaughtyAttributes.Button]
    void Exeecute()
    {
        foreach (var item in textures)
        {
            string path = AssetDatabase.GetAssetPath(item);
            
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);

            int max = Mathf.Max(item.height, item.width);
            int target = 2048;
            foreach(var d in data)
            {
                if (max >= d.pixelMustBeInRange.x && max <= d.pixelMustBeInRange.y)
                {
                    target = d.maxSize;
                    break;
                }
            }

            Debug.Log($"path {path} {importer.npotScale} h:{item.height},w:{item.width},max:{max},target:{target}");

            importer.maxTextureSize = target;

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        AssetDatabase.Refresh();

        //
        //importer.wrapMode = TextureWrapMode.Clamp;
        //importer.textureType = TextureImporterType.Advanced;
        //importer.mipmapEnabled = false;
        //
        //importer.textureFormat = TextureImporterFormat.PVRTC_RGB4;

        ////TextureImporterSettings tis = new TextureImporterSettings();
        ////importer.ReadTextureSettings(tis);
        ////tis.ApplyTextureType(TextureImporterType.Advanced, false);

        //AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }
}
#endif