using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ParentManipulator
{
    [MenuItem("Tools/Set sibling first %&z")] // Ctrl + alt + z
    private static void NewMenuOption()
    {
        foreach (var s in Selection.objects)
        {
            var selected = s;
            Transform tr = ((GameObject)selected).GetComponent<Transform>();
            Debug.Log(selected);
            tr.SetAsFirstSibling();
        }
    }

    [MenuItem("Tools/Set sibling last %&x")] // Ctrl + alt + x
    private static void NewMenuOptionOne()
    {
        foreach (var s in Selection.objects)
        {
            var selected = s;
            Transform tr = ((GameObject)selected).GetComponent<Transform>();
            Debug.Log(selected);
            tr.SetAsLastSibling();
        }
    }

    [MenuItem("Tools/Set parent null %#q")] // Ctrl + shift + q
    private static void NewMenuOptionTwo()
    {
        foreach (var s in Selection.objects)
        {
            var selected = s;
            Transform tr = ((GameObject)selected).GetComponent<Transform>();
            Debug.Log(selected);
            tr.parent = null;
        }
    }

    public static int parent = 0;

    [MenuItem("Tools/Create empty and set as parent for selected %#&w")] // Ctrl + shift + alt + w
    private static void NewMenuOptionThree()
    {
        GameObject empty = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.DestroyImmediate(empty.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(empty.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(empty.GetComponent<BoxCollider>());
        empty.name = "AParent " + parent++;

        foreach (var s in Selection.objects)
        {
            var selected = s;
            Transform tr = ((GameObject)selected).GetComponent<Transform>();
            Debug.Log(selected);
            tr.parent = empty.transform;
        }
    }

    [MenuItem("Tools/Create empty at top and set as parent for selected %#&e")] // Ctrl + shift + alt + e
    private static void NewMenuOptionThreeUp()
    {
        GameObject empty = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.DestroyImmediate(empty.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(empty.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(empty.GetComponent<BoxCollider>());
        empty.name = "AParent " + parent++;
        empty.GetComponent<Transform>().SetAsFirstSibling();

        foreach (var s in Selection.objects)
        {
            var selected = s;
            Transform tr = ((GameObject)selected).GetComponent<Transform>();
            Debug.Log(selected);
            tr.parent = empty.transform;
        }
    }

    [MenuItem("Tools/Move sibling 1 up %#z")] // Ctrl + shift + z
    private static void MoveUp()
    {
        List<Transform> sorter = new List<Transform>();
        foreach (var s in Selection.objects)
        {
            var selected = s;
            Transform tr = ((GameObject)selected).GetComponent<Transform>();
            sorter.Add(tr);
        }
        sorter.Sort((x, y) => x.GetSiblingIndex().CompareTo(y.GetSiblingIndex()));

        foreach (var s in sorter)
        {
            Transform tr = s;
            Debug.Log(tr + " " + tr.GetSiblingIndex());
            if (tr.GetSiblingIndex() - 1 > -1)
                tr.SetSiblingIndex(tr.GetSiblingIndex() - 1);
        }
    }

    [MenuItem("Tools/Move sibling 1 down %#x")] // Ctrl + shift + x
    private static void MoveDown()
    {
        List<Transform> sorter = new List<Transform>();
        foreach (var s in Selection.objects)
        {
            var selected = s;
            Transform tr = ((GameObject)selected).GetComponent<Transform>();
            sorter.Add(tr);
        }
        sorter.Sort((x,y) => y.GetSiblingIndex().CompareTo(x.GetSiblingIndex()) );

        foreach (var s in sorter)
        {
            Transform tr = s;
            Debug.Log(tr + " " + tr.GetSiblingIndex());
            tr.SetSiblingIndex(tr.GetSiblingIndex() + 1);
        }
    }
}
