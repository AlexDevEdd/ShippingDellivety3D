using System.Collections.Generic;
using UnityEngine;

public class CharacterChecker : MonoBehaviour
{
    [Header("Cloner")]
    public List<Transform> from;
    public List<Transform> to;

    [Header("Bones Checker")]
    public Transform root;
    public List<Transform> transforms;
    public List<string> matches;
    public List<string> differences;

    [NaughtyAttributes.Button]
    void CopyFromTo()
    {
        int count = 0;
        for (int i = 0; i < to.Count; i++)
        {
            for (int j = 0; j < from.Count; j++)
            {
                if (to[i].name == from[j].name)
                {
                    to[i].localPosition = from[j].localPosition;
                    to[i].localRotation = from[j].localRotation;
                    to[i].localScale = from[j].localScale;
                    count++;
                }
            }
        }
        Debug.Log($"to count: {to.Count}, from count: {from.Count}, matches: {count}");
    }

    [NaughtyAttributes.Button(20)]
    public void GetAllBoneNamesFromSMR()
    {
        transforms = new List<Transform>();
        var smr = GetComponent<SkinnedMeshRenderer>();
        if (smr == null)
            smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr == null)
        {
            Debug.LogError($"You didn't have smr");
            return;
        }
        var _bones = smr.bones;
        foreach (var item in _bones)
        {
            transforms.Add(item);
            Debug.Log($"{item}");
        }
    }

    [NaughtyAttributes.Button]
    public void SetAllBonesFromCurrentTransformsList()
    {
        var smr = GetComponent<SkinnedMeshRenderer>();
        if (smr == null)
            smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr == null)
        {
            Debug.LogError($"You didn't have smr");
            return;
        }
        smr.bones = transforms.ToArray();
    }

    [NaughtyAttributes.Button]
    public void GetDifferences()
    {
        matches = new List<string>();
        differences = new List<string>();
        var smr = GetComponent<SkinnedMeshRenderer>();
        if (smr == null)
            smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr == null)
        {
            Debug.LogError($"You didn't have smr");
            return;
        }
        var _bones = smr.bones;
        bool isIs;
        foreach (var item in _bones)
        {
            isIs = false;
            foreach (var g in transforms)
            {
                if (item.name == g.name)
                {
                    matches.Add(item.name);
                    isIs = true;
                }
            }
            if (!isIs)
            {
                differences.Add(item.name);
            }
        }
    }

    [NaughtyAttributes.Button]
    public void RefillTransformsListByMatchesInTransform()
    {
        List<Transform> newBones = new List<Transform>();
        var insides = root.GetComponentsInChildren<Transform>();
        for (int i = 0; i < transforms.Count; i++)
        {
            foreach (var item in insides)
            {
                if (item.name == transforms[i].name)
                {
                    newBones.Add(item);
                    break;
                }
            }
        }
        transforms = newBones;
    }
}
