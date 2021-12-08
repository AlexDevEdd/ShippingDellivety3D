using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkeletonDraw : MonoBehaviour
{
    public enum BoneType
    {
        Pelvis = 0,
        Head = 1,
    }

    public List<Transform> bones;

    public bool isDrawGizmoConnections;

    public List<string> boneNameContains;

    [NaughtyAttributes.Button]
    public void InitBoneNames1()
    {
        boneNameContains = new List<string>();
        boneNameContains.Add("Pelvis");
        boneNameContains.Add("Head");
    }

    [NaughtyAttributes.Button]
    public void ConnectBonesByNames()
    {
        int id = 0;
        var trs = GetComponentsInChildren<Transform>();
        trs.ToList().Add(transform);
        bones = new List<Transform>();
        foreach (var item in boneNameContains)
        {
            foreach(var tr in trs)
            {
                if (tr.name.Contains(item) || tr.name.Contains($"{char.ToUpper(item[0])}{item.Remove(0, 1)}"))
                {
                    bones.Add(tr);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isDrawGizmoConnections)
        {

        }
    }
}
