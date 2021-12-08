using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// V 1.1
public class QAnimator : MonoBehaviour
{
    public bool isInited = false;

    [Tooltip("If count of visible and not visible bones dont match, use this bool, it will try to init only those bones which exist in visible model and skip that ones which were inside invisible skeleton only")]
    public bool useNames = true;

    public bool isIgnoreBonesWithAnimatorOnIt = true;
    private List<string> ignoreAnimatorNameList;

    public bool isUseWeight;
    private bool IsWeightShow() { return isUseWeight && !isUseWeightDistanceBased; }
    [NaughtyAttributes.ShowIf("isUseWeight")]
    public bool isUseWeightDistanceBased;
    [NaughtyAttributes.ShowIf("IsWeightShow")]
    [Range(0,1)]
    public float weight = 1f;
    private bool IsAnimationCurveShow() { return isUseWeight && isUseWeightDistanceBased; }
    [NaughtyAttributes.ShowIf("IsAnimationCurveShow")]
    public AnimationCurve weightCurve;

    public SyncTypes syncTypesOnInit = (SyncTypes)15;

    public Transform invisibleRoot;
    public Transform visibleRoot;

    private List<Transform> invisibleBones;
    private List<Transform> visibleBones;

    public List<ControllableBone> controllableBones;

    private void Start()
    {
        // Update one more time to real links
        for (int i = 0; i < controllableBones.Count; i++)
        {
            var qbone = controllableBones[i].tr.GetComponent<QBone>();
            if (qbone != null)
            {
                var states = qbone.controllableBone.syncTypes;
                qbone.controllableBone = controllableBones[i];
                qbone.controllableBone.syncTypes = states;
            }
            else
            {
                qbone = controllableBones[i].tr.gameObject.AddComponent<QBone>();
                qbone.controllableBone = controllableBones[i];
            }
        }
    }

    [NaughtyAttributes.Button]
    public void Init()
    {
        isInited = false;

        if (invisibleRoot == null)
            Debug.LogError($"invisibleRoot is null");

        if (visibleRoot == null)
            Debug.LogError($"visibleRoot is null");

        if (isIgnoreBonesWithAnimatorOnIt)
            ignoreAnimatorNameList = new List<string>();

        invisibleBones = new List<Transform>();
        visibleBones = new List<Transform>();

        InitTransforms(invisibleRoot, invisibleRoot, invisibleBones);
        InitTransforms(visibleRoot, visibleRoot, visibleBones);

        if (useNames)
        {
            DistinctListsByNames();
        }

        if (invisibleBones.Count != visibleBones.Count)
        {
            Debug.LogError($"Object count dont match, invisible count: {invisibleBones.Count}, visible count: {visibleBones.Count}");
            return;
        }

        controllableBones = new List<ControllableBone>();
        for(int i = 0; i < visibleBones.Count; i++)
        {
            ControllableBone controllableBone = new ControllableBone()
            { 
                tr = visibleBones[i], 
                animationBone = invisibleBones[i], 
                isControlledByAnimation = true,
                syncTypes = syncTypesOnInit
            };
            controllableBones.Add(controllableBone);

            var qbone = visibleBones[i].GetComponent<QBone>();
            if (qbone != null)
                qbone.controllableBone = controllableBone;
            else
            {
                qbone = visibleBones[i].gameObject.AddComponent<QBone>();
                qbone.controllableBone = controllableBone;
            }
        }

        invisibleBones = new List<Transform>();
        visibleBones = new List<Transform>();

        isInited = true;
    }

    private void FixedUpdate() // Thing is, animation is internal for physics update
    {
        if (!isInited)
            return;

        for (int i = 0; i < controllableBones.Count; i++)
        {
            var controllableBone = controllableBones[i];
            if (controllableBone.isControlledByAnimation)
            {
                var visible = controllableBone.tr;
                var invisible = controllableBone.animationBone;

                if ((int)controllableBone.syncTypes == -1)
                    controllableBone.syncTypes = (SyncTypes)15;

                if (isUseWeight)
                {
                    if (isUseWeightDistanceBased)
                    {
                        var distance = (visible.position - invisible.position).magnitude;
                        var weightDistance = weightCurve.Evaluate(distance);

                        if ((controllableBone.syncTypes & SyncTypes.Position) != 0)
                            visible.position = Vector3.Lerp(visible.position, invisible.position, weightDistance);
                        if ((controllableBone.syncTypes & SyncTypes.Rotation) != 0)
                            visible.rotation = Quaternion.Lerp(visible.rotation, invisible.rotation, weightDistance);
                        if ((controllableBone.syncTypes & SyncTypes.Scale) != 0)
                            visible.localScale = Vector3.Lerp(visible.localScale, invisible.localScale, weightDistance);
                    }
                    else
                    {
                        if ((controllableBone.syncTypes & SyncTypes.Position) != 0)
                            visible.position = Vector3.Lerp(visible.position, invisible.position, weight);
                        if ((controllableBone.syncTypes & SyncTypes.Rotation) != 0)
                            visible.rotation = Quaternion.Lerp(visible.rotation, invisible.rotation, weight);
                        if ((controllableBone.syncTypes & SyncTypes.Scale) != 0)
                            visible.localScale = Vector3.Lerp(visible.localScale, invisible.localScale, weight);
                    }
                }
                else
                {
                    if ((controllableBone.syncTypes & SyncTypes.Position) != 0)
                        visible.position = invisible.position;
                    if ((controllableBone.syncTypes & SyncTypes.Rotation) != 0)
                        visible.rotation = invisible.rotation;
                    if ((controllableBone.syncTypes & SyncTypes.Scale) != 0)
                        visible.localScale = invisible.localScale;
                }
                if ((controllableBone.syncTypes & SyncTypes.ActiveState) != 0)
                    if (invisible.gameObject.activeInHierarchy != visible.gameObject.activeInHierarchy)
                        visible.gameObject.SetActive(invisible.gameObject.activeInHierarchy);
            }
        }
    }

    private void DistinctListsByNames()
    {
        for(int i = 0; i < invisibleBones.Count; i++)
        {
            bool isCanExist = false;
            var invisibleBone = invisibleBones[i];
            var invisibleName = GetFullLocalName(invisibleBone, invisibleRoot);
            foreach(var visibleBone in visibleBones)
            {
                var visibleName = GetFullLocalName(visibleBone, visibleRoot);
                if (invisibleName == visibleName)
                {
                    isCanExist = true;
                    break;
                }
            }
            if (!isCanExist)
            {
                invisibleBones.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < visibleBones.Count; i++)
        {
            bool isCanExist = false;
            var visibleBone = visibleBones[i];
            var visibleName = GetFullLocalName(visibleBone, visibleRoot);
            foreach (var invisibleBone in invisibleBones)
            {
                var invisibleName = GetFullLocalName(invisibleBone, invisibleRoot);
                if (invisibleName == visibleName)
                {
                    isCanExist = true;
                    break;
                }
            }
            if (!isCanExist)
            {
                visibleBones.RemoveAt(i);
                i--;
            }
        }
    }

    private void InitTransforms(Transform tr, Transform root, List<Transform> list)
    {
        foreach (Transform t in tr)
        {
            if (isIgnoreBonesWithAnimatorOnIt && t.GetComponent<Animator>() != null)
            {
                ignoreAnimatorNameList.Add(GetFullLocalName(t, root));
            }
            else if (isIgnoreBonesWithAnimatorOnIt && ignoreAnimatorNameList.Contains(GetFullLocalName(t, root)))
            {
                continue;
            }
            else
            {
                list.Add(t);
                InitTransforms(t, root, list);
            }
        }

    }

    private string GetFullLocalName(Transform target, Transform root)
    {
        if (target == root)
            return $"root";
        if (target == null)
            return ""; // empty

        return $"{GetFullLocalName(target.parent, root)}/{target}";
    }
}

[System.Flags]
public enum SyncTypes
{
    Position = 1,
    Rotation = 2,
    Scale = 4,
    ActiveState = 8
}