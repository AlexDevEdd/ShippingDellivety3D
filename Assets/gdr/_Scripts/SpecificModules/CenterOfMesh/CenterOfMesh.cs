using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMesh : MonoBehaviour
{
    [NaughtyAttributes.Button]
    void SpawnInCenter()
    {
        var mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
            var go = new GameObject("Test");
            Debug.Log(mf.sharedMesh.bounds);
            go.transform.position = transform.position + new Vector3(
                mf.sharedMesh.bounds.center.x * transform.lossyScale.x,
                mf.sharedMesh.bounds.center.y * transform.lossyScale.y,
                mf.sharedMesh.bounds.center.z * transform.lossyScale.z
                );
            go.transform.SetParent(transform);
        }
    }
}
