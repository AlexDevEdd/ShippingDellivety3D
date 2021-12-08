using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCalculator : MonoBehaviour
{
    public List<Bone> boneOrdered;

    private void Update()
    {
        foreach(var bone in boneOrdered)
        {
            bone.DoUpdate();
        }
    }
}
