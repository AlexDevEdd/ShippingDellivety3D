using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MyRename : MonoBehaviour
{
    public string nameTemplete;

    [NaughtyAttributes.Button]
    public void Rename()
    {
        List<Transform> transforms = new List<Transform>();
        transforms.AddRange(GetComponentsInChildren<Transform>());

        for (int i = 1; i < transforms.Count; i++)
            transforms[i].name = $"{nameTemplete} ({i})";
    }
}
