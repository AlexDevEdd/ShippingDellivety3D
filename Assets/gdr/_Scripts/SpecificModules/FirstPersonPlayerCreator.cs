using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerCreator : MonoBehaviour
{
    public string nameOfObject = "Player";
    [Header("Auto")]
    public GameObject linkToPlayer;
    [Header("Place Model Here")]
    public GameObject modelToAttach;
    public bool isCreateAdditionalModelAnchor = true;
    public Vector3 modelOffset;
    [Header("Camera Settings")]
    public float cameraOffsetY = 0f;
    public float cameraOffsetZ = 0f;
    public bool isPlaceTestCamera;
    [Header("Auto")]
    public Camera testCamera;
    [Header("Box Collider Settings`")]
    public bool isHasBoxCollider;
    public bool isUseModelBoxCollider;
    [NaughtyAttributes.HideIf("isUseModelBoxCollider")]
    public Vector3 fixedBoxColliderSize;
    public bool isForceRemoveModelCollider;
    [Header("Place Animator Runtime Controller")]
    public RuntimeAnimatorController animatorOnModel;
    public FirstPersonPlayer fps;
    public bool isUsePhysicRigidbody;
    public bool isUseGravity;
    public bool isKinematic;

    [Header("Shared")]
    public int layerID;
    //[Header("Auto Show Current Layer")]
    [NaughtyAttributes.ShowNativeProperty]
    public string SelectedLayer
    {
        get
        {
            return LayerMask.LayerToName(layerID);
        }
    }

    public FirstPersonPlayer.PlayerData playerData;

    public AnimationClip deathClip;
    public AnimationClip walkClip;
    public AnimationClip runClip;
    public AnimationClip idleClip;
    public List<AnimationClip> animations;

    [Header("Testing")]
    public int testAnimationToPlayId = 0;
    public float testAnimationCrossfade = 0f;

    private GameObject model;

    [NaughtyAttributes.Button]
    private void Create()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (linkToPlayer != null)
                DestroyImmediate(linkToPlayer);
            linkToPlayer = new GameObject();
            linkToPlayer.name = nameOfObject;

            var cameraY_GO = new GameObject();
            var cameraX_GO = new GameObject();
            cameraY_GO.name = $"CameraY";
            cameraX_GO.name = $"CameraX";

            cameraY_GO.transform.SetParent(linkToPlayer.transform);
            cameraX_GO.transform.SetParent(cameraY_GO.transform);
            cameraY_GO.transform.localPosition = new Vector3(0f, cameraOffsetY, 0f);
            cameraX_GO.transform.localPosition = Vector3.zero;
            cameraY_GO.transform.localRotation = Quaternion.identity;
            cameraX_GO.transform.localRotation = Quaternion.identity;

            var cameraHolderTr = new GameObject();
            cameraHolderTr.name = $"CameraHolderTr";
            cameraHolderTr.transform.SetParent(cameraX_GO.transform);
            cameraHolderTr.transform.localPosition = new Vector3(0f, 0f, cameraOffsetZ);
            cameraHolderTr.transform.localRotation = Quaternion.identity;

            if (isPlaceTestCamera)
            {
                var cameraGO = new GameObject();
                cameraGO.name = $"PlayerCamera";
                cameraGO.transform.SetParent(cameraHolderTr.transform);
                cameraGO.transform.localPosition = new Vector3(0f, 0f, 0f);
                cameraGO.transform.localRotation = Quaternion.identity;
                testCamera = cameraGO.AddComponent<Camera>();
            }
            linkToPlayer.transform.position = transform.position;
            linkToPlayer.transform.rotation = transform.rotation;
            fps = linkToPlayer.AddComponent<FirstPersonPlayer>();
            fps.currentCamera = testCamera;
            model = null;
            if (modelToAttach != null)
                model = Instantiate(modelToAttach, cameraX_GO.transform);
            else
            {
                model = GameObject.CreatePrimitive(PrimitiveType.Cube);
                model.transform.SetParent(cameraX_GO.transform);
            }
            model.transform.position = Vector3.zero;
            model.transform.rotation = Quaternion.identity;
            BoxCollider bc = null;
            if (isHasBoxCollider)
            {
                bc = AddBoxCollider(isUseModelBoxCollider && model != null);
            }

            if (isCreateAdditionalModelAnchor)
            {
                GameObject modelAnchor = new GameObject();
                modelAnchor.name = $"ModelAnchor";
                modelAnchor.layer = layerID;
                modelAnchor.transform.position = linkToPlayer.transform.position + model.transform.rotation * modelOffset;
                modelAnchor.transform.localRotation = transform.rotation;
                modelAnchor.transform.SetParent(linkToPlayer.transform);
                model.transform.SetParent(modelAnchor.transform);
                model.transform.position = linkToPlayer.transform.position;
            }
            else
            {
                model.transform.position = linkToPlayer.transform.position + model.transform.rotation * modelOffset;
            }
            model.transform.localRotation = Quaternion.identity;

            Animator animator = null;
            if (model != null && animatorOnModel != null)
            {
                animator = model.AddComponent<Animator>();
                animator.runtimeAnimatorController = animatorOnModel;
            }
            fps.deathClip = deathClip;
            fps.walkClip = walkClip;
            fps.runClip = runClip;
            fps.idleClip = idleClip;
            fps.animator = animator;
            fps.model = model;
            fps.bc = bc;
            if (isUsePhysicRigidbody)
            {
                var rb = linkToPlayer.AddComponent<Rigidbody>();
                rb.useGravity = isUseGravity;
                rb.isKinematic = isKinematic;
                fps.rb = rb;
            }
            fps.animations = animations;
            fps.stat = playerData;
            linkToPlayer.layer = layerID;
            var trs = linkToPlayer.GetComponentsInChildren<Transform>();
            foreach (Transform tr in trs)
            {
                tr.gameObject.layer = layerID;
            }
        }
    }

    private BoxCollider AddBoxCollider(bool isFromModel)
    {
        if (isUseModelBoxCollider)
        {
            var bounds = AddAndExpandBoxColliderFromModel(model.transform);

            var playerBC = linkToPlayer.GetComponent<BoxCollider>();
            bool isPlayerBCExist = playerBC != null;
            if (!isPlayerBCExist)
                playerBC = linkToPlayer.AddComponent<BoxCollider>();

            playerBC.size = bounds.size;
            playerBC.center = bounds.center;
            return playerBC;
        }
        else
        {
            var playerBC = linkToPlayer.GetComponent<BoxCollider>();
            bool isPlayerBCExist = playerBC != null;
            if (!isPlayerBCExist)
                linkToPlayer.AddComponent<BoxCollider>();
            return playerBC;
        }
    }

    private Bounds AddAndExpandBoxColliderFromModel(Transform targetTr)
    {
        var bounds = new Bounds();
        bool onceEncapsulated = false;
        foreach (Transform tr in targetTr)
        {
            if (tr.GetComponent<Renderer>() != null)
            {
                var bc = tr.gameObject.GetComponent<BoxCollider>();
                bool isModelHasBC = bc != null;
                if (!isModelHasBC)
                    bc = tr.gameObject.AddComponent<BoxCollider>();

                var boundsCollider = bc.bounds;
                // boundsCollider.size = Vector3.Scale(tr.rotation * bc.size, tr.lossyScale);
                boundsCollider.size = Vector3.Scale(bc.size, tr.lossyScale);
                Debug.Log(boundsCollider);

                if (!onceEncapsulated)
                {
                    onceEncapsulated = true;
                    bounds = boundsCollider;
                }
                else
                    bounds.Encapsulate(boundsCollider);

                if (!isModelHasBC)
                {
                    DestroyImmediate(bc, true);
                }
                else if (isForceRemoveModelCollider)
                {
                    DestroyImmediate(bc, true);
                }
            }
        }

        if (targetTr.GetComponent<BoxCollider>() != null)
        {
            var bc = targetTr.gameObject.GetComponent<BoxCollider>();
            bool isModelHasBC = bc != null;
            if (!isModelHasBC)
                bc = targetTr.gameObject.AddComponent<BoxCollider>();

            var boundsCollider = bc.bounds;
            // boundsCollider.size = Vector3.Scale(tr.rotation * bc.size, tr.lossyScale);
            boundsCollider.size = Vector3.Scale(bc.size, targetTr.lossyScale);
            Debug.Log(boundsCollider);

            if (!onceEncapsulated)
            {
                onceEncapsulated = true;
                bounds = boundsCollider;
            }
            else
                bounds.Encapsulate(boundsCollider);

            if (!isModelHasBC)
            {
                DestroyImmediate(bc, true);
            }
            else if (isForceRemoveModelCollider)
            {
                DestroyImmediate(bc, true);
            }
        }

        return bounds;
    }

    [NaughtyAttributes.Button(20)]
    private void TestAnimationPlay()
    {
        fps.PlayAnimation(testAnimationToPlayId, testAnimationCrossfade);
    }
}
