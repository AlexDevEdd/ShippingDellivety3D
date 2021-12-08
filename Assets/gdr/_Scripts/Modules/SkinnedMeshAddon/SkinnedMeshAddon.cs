using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshAddon : MonoBehaviour
{
    public bool isAttachOnAwake = true;
    [Header("Assign here your new mesh")]
    public SkinnedMeshRenderer currentMeshRenderer;
    [Header("Assign here mesh with skeleton you want to attach")]
    public SkinnedMeshRenderer donorMeshRenderer;
    [Header("Set if root bones arent the same (try button first)")]
    public Transform customDonorBoneRoot;
    [Header("Only This Should Be Filled At runtime")]
    public Transform[] currentBones;

    [Header("Init only things")]
    public Transform[] donorBones;
    public List<string> donorNames;
    public List<string> currentNames;

    private bool isChangeMade = false;

    private void Awake()
    {
        if (isAttachOnAwake)
            AttachMesh();
    }

    // Should work when used duplicate
    //[NaughtyAttributes.Button]
    //public void Clone()
    //{
    //    currentMeshRenderer.bones = donorMeshRenderer.bones;
    //}

    [NaughtyAttributes.Button(20)]
    public void AttachMesh()
    {
        if (!Application.isPlaying)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"You can attach mesh only at runtime", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer == null)
        {
            Debug.LogError($"You didnt initialized addon on smr {gameObject.name}");
            return;
        }
        currentMeshRenderer.bones = currentBones;
    }

    // Causes too much problems
    //[NaughtyAttributes.Button]
    //private void EditorInit()
    //{
    //    if (Application.isPlaying)
    //        return;

    //    if (currentMeshRenderer == null)
    //    {
    //        currentMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    //        if (currentMeshRenderer == null)
    //        {
    //            Debug.LogError($"No SkinnedMeshRenderer found on object {gameObject.name}");
    //            return;
    //        }
    //    }

    //    if (currentMeshRenderer.rootBone == donorMeshRenderer.rootBone)
    //    {
    //        Debug.LogError($"Same root bone used! Dont change it before doing editor init");
    //    }

    //    donorNames = GetBoneNames(donorMeshRenderer);
    //    currentNames = GetBoneNames(currentMeshRenderer);

    //    donorBones = donorMeshRenderer.bones;
    //    currentBones = new Transform[donorBones.Length];

    //    // Set null links to old bones
    //    for (int i = 0; i < currentBones.Length; i++)
    //        currentBones[i] = null;

    //    // Now we should link donor bones to current ones, just find indexes by names and assign them
    //    for (int j = 0; j < currentNames.Count; j++)
    //    {
    //        var currentName = currentNames[j];
    //        for (int i = 0; i < donorNames.Count; i++)
    //        {
    //            var donorName = donorNames[i];

    //            if (donorName == currentName)
    //            {
    //                currentBones[j] = donorBones[i];
    //                i = donorNames.Count;
    //            }
    //        }
    //    }

    //    currentMeshRenderer.rootBone = donorMeshRenderer.rootBone;
    //}

    [NaughtyAttributes.Button(20)]
    private void Editor_InitWithoutRootBoneChange()
    {
        if (Application.isPlaying)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"You can InitWithoutRootBoneChange only in editor mode", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer == null)
        {
            currentMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (currentMeshRenderer == null)
            {
                Debug.LogError($"No SkinnedMeshRenderer found on object {gameObject.name}");
                return;
            }
        }

        if (currentMeshRenderer.rootBone == null)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"You dont have root bone attached to current smr", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer.rootBone == donorMeshRenderer.rootBone || currentMeshRenderer.rootBone == customDonorBoneRoot)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"You cant do this until you have same root bone on both smrs. Probably you should redo all steps and set root bone at the end of all operations.\nProblem: \"root bones already synced\".", "Ok");
#endif
            return;
        }

        donorNames = GetBoneNames(donorMeshRenderer, customDonorBoneRoot);
        currentNames = GetBoneNames(currentMeshRenderer);

        donorBones = donorMeshRenderer.bones;
        currentBones = new Transform[donorBones.Length];

        // Set null links to old bones
        for (int i = 0; i < currentBones.Length; i++)
            currentBones[i] = null;

        // Now we should link donor bones to current ones, just find indexes by names and assign them
        for (int j = 0; j < currentNames.Count; j++)
        {
            var currentName = currentNames[j];
            for (int i = 0; i < donorNames.Count; i++)
            {
                var donorName = donorNames[i];

                if (donorName == currentName)
                {
                    currentBones[j] = donorBones[i];
                    i = donorNames.Count;
                }
            }
        }

        if (currentBones.Length == 0)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Warning", $"Something went wrong, bones count: 0", "Ok");
#endif
        }
        else
        {
            int broken = 0;
            foreach (var item in currentBones)
            {
                if (item == null)
                    broken++;
            }
            if (broken > 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog($"Warning", $"Something went wrong, broken bones: {broken}/{currentBones.Length}", "Ok");
#endif
            }
            else
                isChangeMade = true;
        }
    }

    [NaughtyAttributes.Button(20)]
    private void Editor_InitWithSelfBones()
    {
        if (Application.isPlaying)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"You can InitWithSelfBones only in editor mode", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer == null)
        {
            currentMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (currentMeshRenderer == null)
            {
                Debug.LogError($"No SkinnedMeshRenderer found on object {gameObject.name}");
                return;
            }
        }

        if (currentMeshRenderer.rootBone == null)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"You dont have root bone attached to current smr", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer.rootBone == donorMeshRenderer.rootBone || currentMeshRenderer.rootBone == customDonorBoneRoot)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"You cant do this until you have same root bone on both smrs. Probably you should redo all steps and set root bone at the end of all operations.\nProblem: \"root bones already synced\".", "Ok");
#endif
            return;
        }

        donorNames = GetBoneNames(donorMeshRenderer, customDonorBoneRoot, true);
        currentNames = GetBoneNames(currentMeshRenderer);

        if (customDonorBoneRoot != null)
            donorBones = GetBoneTransforms(customDonorBoneRoot);
        else
            donorBones = GetBoneTransforms(donorMeshRenderer.rootBone);
        currentBones = new Transform[currentMeshRenderer.bones.Length];

        // Set null links to old bones
        for (int i = 0; i < currentBones.Length; i++)
            currentBones[i] = null;

        // Now we should link donor bones to current ones, just find indexes by names and assign them
        for (int j = 0; j < currentNames.Count; j++)
        {
            var currentName = currentNames[j];
            for (int i = 0; i < donorNames.Count; i++)
            {
                var donorName = donorNames[i];

                if (donorName == currentName)
                {
                    currentBones[j] = donorBones[i];
                    i = donorNames.Count;
                }
            }
        }

        if (currentBones.Length == 0)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Warning", $"Something went wrong, bones count: 0", "Ok");
#endif
        }
        else
        {
            int broken = 0;
            foreach (var item in currentBones)
            {
                if (item == null)
                    broken++;
            }
            if (broken > 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog($"Warning", $"Something went wrong, broken bones: {broken}/{currentBones.Length}", "Ok");
#endif
            }
            else
                isChangeMade = true;
        }
    }

    [NaughtyAttributes.Button(20)]
    private void SetCustomBoneWithSameName()
    {
        if (currentMeshRenderer == null)
        {
            currentMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (currentMeshRenderer == null)
            {
                Debug.LogError($"No SkinnedMeshRenderer found on object {gameObject.name}");
                return;
            }
        }

        if (currentMeshRenderer.rootBone == donorMeshRenderer.rootBone)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"We will assign custom bone, but your root bones already synced. I Hope you know what you are doing...", "Ok");
#endif
        }

        foreach (var item in donorMeshRenderer.bones)
        {
            if (item.name == currentMeshRenderer.rootBone.name)
            {
                customDonorBoneRoot = item;
                return;
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog($"Info", $"Custom Donor Bone isnt found! Probably these bones have different names. Attach it manually", "Ok");
#endif
    }

    [NaughtyAttributes.ButtonShowIf("isChangeMade", NaughtyAttributes.ColorEnum.red)]
    private void RB1()
    {
        if (currentMeshRenderer == null)
        {
            currentMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (currentMeshRenderer == null)
            {
                Debug.LogError($"No SkinnedMeshRenderer found on object {gameObject.name}");
                return;
            }
        }

        if (currentMeshRenderer.rootBone == donorMeshRenderer.rootBone)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"Already same root bones in use", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer.rootBone.name != donorMeshRenderer.rootBone.name)
        {
            bool isApply = true;
#if UNITY_EDITOR
            isApply = UnityEditor.EditorUtility.DisplayDialog($"Warning", $"Do you really want apply \"{donorMeshRenderer.rootBone.name}\" to \"{currentMeshRenderer.rootBone.name}\" root bone slot? They have different names and probably they will have different hierarchy. Continue?", "Yes", "No");
#endif
            if (isApply)
            {
                currentMeshRenderer.rootBone = donorMeshRenderer.rootBone;
                isChangeMade = false;
            }
        }
        else
        {
            currentMeshRenderer.rootBone = donorMeshRenderer.rootBone;
            isChangeMade = false;
        }
    }

    [NaughtyAttributes.ButtonShowIf("isChangeMade", NaughtyAttributes.ColorEnum.red)]
    private void RB2()
    {
        if (currentMeshRenderer == null)
        {
            currentMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (currentMeshRenderer == null)
            {
                Debug.LogError($"No SkinnedMeshRenderer found on object {gameObject.name}");
                return;
            }
        }

        if (currentMeshRenderer.rootBone == donorMeshRenderer.rootBone)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"Already same root bones in use", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer.rootBone.name == donorMeshRenderer.rootBone.name)
        {
            currentMeshRenderer.rootBone = donorMeshRenderer.rootBone;
            isChangeMade = false;
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"Root bone names arent the same \"{donorMeshRenderer.rootBone.name}\" and \"{currentMeshRenderer.rootBone.name}\"", "Ok");
#endif
        }
    }

    [NaughtyAttributes.ButtonShowIf("isChangeMade", NaughtyAttributes.ColorEnum.red)]
    private void RB3()
    {
        if (currentMeshRenderer == null)
        {
            currentMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (currentMeshRenderer == null)
            {
                Debug.LogError($"No SkinnedMeshRenderer found on object {gameObject.name}");
                return;
            }
        }

        if (currentMeshRenderer.rootBone == donorMeshRenderer.rootBone)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog($"Info", $"Already same root bones in use", "Ok");
#endif
            return;
        }

        if (currentMeshRenderer.rootBone.name == donorMeshRenderer.rootBone.name)
            currentMeshRenderer.rootBone = donorMeshRenderer.rootBone;
        else
        {
            foreach (var item in donorMeshRenderer.bones)
            {
                if (item.name == currentMeshRenderer.rootBone.name)
                {
                    bool isApply = true;
#if UNITY_EDITOR
                    isApply = UnityEditor.EditorUtility.DisplayDialog($"Info", $"We can attach \n\"{GetFullName(item)}\" to \n\"{GetFullName(currentMeshRenderer.rootBone)}\" root bone slot, continue?", "Yes", "No");
#endif
                    if (isApply)
                    {
                        currentMeshRenderer.rootBone = item;
                        isChangeMade = false;
                    }

                    return;
                }
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog($"Info", $"Cant find \"{currentMeshRenderer.rootBone.name} bone in donor smr, probably you should assign it manually\"", "Ok");
#endif
    }

    private string GetFullName(Transform tr, string name = "")
    {
        if (tr.parent != null)
            name = $"{GetFullName(tr.parent)}/{tr.name}";
        else
            return tr.name;
        return name;
    }

    private Transform[] GetBoneTransforms(Transform root)
    {
        List<Transform> allBones = new List<Transform>();
        List<string> allNames = new List<string>();
        GetAllTransforms(root, root, allBones, allNames, "");
        return allBones.ToArray();
    }

    private List<string> GetBoneNames(SkinnedMeshRenderer smr, Transform customRoot = null, bool isGetAll = false)
    {
        // Set notaion root/ribcage/hand_l/etc
        var usedBones = smr.bones; // actual bones
        List<Transform> allBones = new List<Transform>();
        List<string> allNames = new List<string>();
        if (customRoot != null)
            GetAllTransforms(customRoot, customRoot, allBones, allNames, "");
        else
            GetAllTransforms(smr.rootBone, smr.rootBone, allBones, allNames, "");
        if (isGetAll)
            return allNames;
        List<string> usedNames = new List<string>();
        foreach (var donorUsedBone in usedBones)
        {
            for (int i = 0; i < allBones.Count; i++)
            {
                var bone = allBones[i];
                var nameB = allNames[i];

                if (donorUsedBone == bone)
                {
                    usedNames.Add(nameB);
                    i = allBones.Count;
                }
            }
        }
        return usedNames;
    }

    private void GetAllTransforms(Transform tr, Transform root, List<Transform> result, List<string> names, string name)
    {
        if (tr == root)
            name = $"root";
        else
            name = $"{name}/{tr.name}";

        names.Add(name);
        result.Add(tr);

        foreach (Transform t in tr)
            GetAllTransforms(t, root, result, names, name);
    }
}
