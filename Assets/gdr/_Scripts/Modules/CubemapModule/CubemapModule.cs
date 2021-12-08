#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CubemapModule : MonoBehaviour
{
    public Cubemap cubemap;

    [ContextMenu("Create")]
    private void ToCubemap()
    {
        Camera.main.RenderToCubemap(cubemap);
    }
}
#endif