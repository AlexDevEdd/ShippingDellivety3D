using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public string nameOfObject = "Enemy";
    [Header("Auto")]
    public GameObject linkToEnemy;
    [Header("Place Model Here")]
    public GameObject modelToAttach;
    public bool isHasBoxCollider;
    public bool isUseModelBoxCollider;
    [NaughtyAttributes.HideIf("isUseModelBoxCollider")]
    public Vector3 fixedBoxColliderSize;
    public bool isForceRemoveModelCollider;
    [Header("Place Animator Runtime Controller")]
    public RuntimeAnimatorController animatorOnModel;
    public EnemyBase enemyScript;
    public bool isUsePhysicRigidbody;
    public bool isUseGravity;
    public bool isKinematic;
    [Header("Enemy Data")]
    public EnemyBase.EnemyData enemyData;

    public AnimationClip deathClip;
    public AnimationClip walkClip;
    public AnimationClip runClip;
    public AnimationClip idleClip;
    public List<AnimationClip> animations;

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

    [Header("Testing")]
    public int testAnimationToPlayId = 0;
    public float testAnimationCrossfade = 0f;

    private GameObject model;

    [NaughtyAttributes.Button]
    private void Create()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (linkToEnemy != null)
                DestroyImmediate(linkToEnemy);
            linkToEnemy = new GameObject();
            linkToEnemy.name = nameOfObject;
            linkToEnemy.transform.position = transform.position;
            linkToEnemy.transform.rotation = transform.rotation;
            enemyScript = linkToEnemy.AddComponent<EnemyBase>();
            model = null;
            if (modelToAttach != null)
                model = Instantiate(modelToAttach, linkToEnemy.transform);
            else
            {
                model = GameObject.CreatePrimitive(PrimitiveType.Cube);
                model.transform.SetParent(linkToEnemy.transform);
            }
            model.transform.position = Vector3.zero;
            model.transform.rotation = Quaternion.identity;

            BoxCollider bc = null;
            if (isHasBoxCollider)
            {
                bc = AddBoxCollider(isUseModelBoxCollider && model != null);
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            Animator animator = null;
            if (model != null && animatorOnModel != null)
            {
                animator = model.GetComponent<Animator>();
                if (animator == null)
                    animator = model.AddComponent<Animator>();
                animator.runtimeAnimatorController = animatorOnModel;
            }
            enemyScript.deathClip = deathClip;
            enemyScript.walkClip = walkClip;
            enemyScript.runClip = runClip;
            enemyScript.idleClip = idleClip;
            enemyScript.animator = animator;
            enemyScript.model = model;
            enemyScript.bc = bc;
            if (isUsePhysicRigidbody)
            {
                var rb = linkToEnemy.AddComponent<Rigidbody>();
                rb.useGravity = isUseGravity;
                rb.isKinematic = isKinematic;
                enemyScript.rb = rb;
            }
            enemyScript.animations = animations;
            enemyScript.stat = enemyData;
            linkToEnemy.layer = layerID;
            var trs = linkToEnemy.GetComponentsInChildren<Transform>();
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

            var playerBC = linkToEnemy.GetComponent<BoxCollider>();
            bool isPlayerBCExist = playerBC != null;
            if (!isPlayerBCExist)
                playerBC = linkToEnemy.AddComponent<BoxCollider>();

            playerBC.size = bounds.size;
            playerBC.center = bounds.center;
            return playerBC;
        }
        else
        {
            var playerBC = linkToEnemy.GetComponent<BoxCollider>();
            bool isPlayerBCExist = playerBC != null;
            if (!isPlayerBCExist)
                linkToEnemy.AddComponent<BoxCollider>();
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
        enemyScript.PlayAnimation(testAnimationToPlayId, testAnimationCrossfade);
    }
}
