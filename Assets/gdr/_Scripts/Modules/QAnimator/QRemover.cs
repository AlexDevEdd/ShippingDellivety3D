using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QRemover : MonoBehaviour
{
    public bool isForCurrentTransformToo = false;

    [NaughtyAttributes.Button]
    public void EnableSMRs()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<SkinnedMeshRenderer>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       comp.enabled = true;
                   }
                   else
                       comp.enabled = true;
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"EnableSMRs");
    }

    [NaughtyAttributes.Button]
    public void EnableMRs()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<MeshRenderer>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       comp.enabled = true;
                   }
                   else
                       comp.enabled = true;
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"EnableMRs");
    }

    [NaughtyAttributes.Button(20)]
    public void DisableSMRs()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<SkinnedMeshRenderer>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       comp.enabled = false;
                   }
                   else
                       comp.enabled = false;
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"DisableSMRs");
    }


    [NaughtyAttributes.Button]
    public void DisableMRs()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<MeshRenderer>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       comp.enabled = false;
                   }
                   else
                       comp.enabled = false;
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"DisableMRs");
    }

    // [NaughtyAttributes.Button] - laggy
    public void RemovePS()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<ParticleSystem>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       Destroy(comp);
                   }
                   else
                       DestroyImmediate(comp);
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"RemovePS");
    }

    [NaughtyAttributes.Button(20)]
    public void RemoveQBones()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<QBone>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       Destroy(comp);
                   }
                   else
                       DestroyImmediate(comp);
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"RemoveColliders");
    }

    [NaughtyAttributes.Button]
    public void RemoveColliders()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<Collider>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       Destroy(comp);
                   }
                   else
                       DestroyImmediate(comp);
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"RemoveColliders");
    }

    [NaughtyAttributes.Button]
    public void RemoveMeshFilters()
    {
        UnityAction<Transform> action = (x) =>
           {
               var comp = x.GetComponent<MeshFilter>();
               if (comp != null)
               {
                   if (Application.isPlaying)
                   {
                       Destroy(comp);
                   }
                   else
                       DestroyImmediate(comp);
               }
           };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"RemoveMeshFilters");
    }

    [NaughtyAttributes.Button]
    public void RemoveMeshRenderers()
    {
        UnityAction<Transform> action = (x) =>
        {
            var comp = x.GetComponent<MeshRenderer>();
            if (comp != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(comp);
                }
                else
                    DestroyImmediate(comp);
            }
        };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"RemoveMeshRenderers");
    }

    [NaughtyAttributes.Button]
    public void RemoveSkinnedMeshRenderers()
    {
        UnityAction<Transform> action = (x) =>
        {
            var comp = x.GetComponent<SkinnedMeshRenderer>();
            if (comp != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(comp);
                }
                else
                    DestroyImmediate(comp);
            }
        };
        if (isForCurrentTransformToo)
            action.Invoke(transform);

        ApplyToTransforms(transform, action);
        Debug.Log($"RemoveSkinnedMeshRenderers");
    }

    private void ApplyToTransforms(Transform tr, UnityAction<Transform> action)
    {
        foreach (Transform t in tr)
        {
            action.Invoke(t);
            ApplyToTransforms(t, action);
        }

    }
}
