using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoxColliderByInternals : MonoBehaviour
{
    [ContextMenu("Create BoxCollider by Internals")]
    public void CreateBoxCollider()
    {
        var bx = gameObject.AddComponent<BoxCollider>();
        var trs = gameObject.GetComponentsInChildren<Transform>();
        Bounds b = new Bounds();
        for(int i = 0; i < trs.Length; i++)
        {
            if (trs[i].GetComponent<MeshRenderer>() != null)
            {
                b.Encapsulate(trs[i].GetComponent<MeshRenderer>().bounds);
            }
        }
        bx.center = b.center;
        bx.size = b.size;
    }
}
