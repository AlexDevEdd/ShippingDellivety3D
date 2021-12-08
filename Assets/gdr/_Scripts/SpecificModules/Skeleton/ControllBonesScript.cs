using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllBonesScript : MonoBehaviour
{
    public GameObject bonePointPrefab;
    public Transform bonePointParent;
    public List<Bone> bones;

    public class BoneControll
    {
        public Bone bone;
        public float distance;
        public Transform bonePoint;
    }
    [Header("Auto")]
    public List<BoneControll> boneControlls;
    private GameObject selectedPoint = null;

    private void Awake()
    {
        boneControlls = new List<BoneControll>();
    }

    private void Start()
    {
        InitBones();
    }

    public void InitBones()
    {
        boneControlls = new List<BoneControll>();
        foreach (var bone in bones)
        {
            var uiPoint = Instantiate(bonePointPrefab, bonePointParent);
            uiPoint.transform.position = Camera.main.WorldToScreenPoint(bone.transform.position);
            boneControlls.Add(new BoneControll()
            {
                bone = bone,
                distance = (Camera.main.transform.position - bone.transform.position).magnitude,
                bonePoint = uiPoint.transform
            });
        }
    }

    public void Update()
    {
        MoveUIPoints();

        foreach(var bc in boneControlls)
        {
            if (selectedPoint == bc.bonePoint)
            {
                var ray = Camera.main.ScreenPointToRay(bc.bonePoint.position);
                bc.distance = (Camera.main.transform.position - bc.bone.transform.position).magnitude;
                bc.bone.transform.position = ray.origin + ray.direction * bc.distance;
            }
            else
            {
                var screenPos = Camera.main.WorldToScreenPoint(bc.bone.transform.position);
                bc.bonePoint.position = screenPos;
                bc.distance = (Camera.main.transform.position - bc.bone.transform.position).magnitude;
            }
        }
    }

    private void MoveUIPoints()
    {
        if (Input.GetMouseButton(0))
        {
            selectedPoint = GetRaycastedUIElement("BonePoint");
            if (selectedPoint != null)
            {
                selectedPoint.transform.position = Input.mousePosition;
            }
        }
        else
            selectedPoint = null;
    }

    private GameObject GetRaycastedUIElement(string contains)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.name.Contains(contains))
                return EventSystem.current.currentSelectedGameObject;
        }
        return null;
    }
}


//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(MyMonoClass))]
//public class MyMonoClassEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        base.OnInspectorGUI();

//        MyMonoClass script = (MyMonoClass)target;

//        serializedObject.ApplyModifiedProperties();
//    }
//}

