using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    public Animator animator;

    public bool isSetLookAtWeight;
    public float setLookAtWeight_weight, setLookAtWeight_body, setLookAtWeight_head, setLookAtWeight_eyes;
    public bool isSetLookRotation;
    public Transform lookAtTarget;
    public bool isSetIKPositionWeight;
    public AvatarIKGoal goal;
    public float goalValue;
    public Transform goalTarget;
    public bool isSetIKRotationWeight;
    public float rotationValue;
    public Transform rotationTr;
    public Quaternion quaternion;

    //public Transform bone;
    //public Vector3 pos;

    private void Update()
    {
        if (isSetIKRotationWeight)
            quaternion = rotationTr.rotation;
    }

    //private void OnAnimatorMove()
    //{
    //    bone.position = pos;
    //}

    private void OnAnimatorIK(int layerIndex)
    {
        if (isSetLookAtWeight)
            animator.SetLookAtWeight(setLookAtWeight_weight, setLookAtWeight_body, setLookAtWeight_head, setLookAtWeight_eyes);
        if (isSetLookRotation)
            animator.SetLookAtPosition(lookAtTarget.position);

        if (isSetIKPositionWeight)
        {
            animator.SetIKPositionWeight(goal, goalValue);
            animator.SetIKPosition(goal, goalTarget.position);
        }

        if (isSetIKRotationWeight)
        {
            animator.SetIKRotationWeight(goal, rotationValue);
            animator.SetIKRotation(goal, quaternion);
        }
        //bone.position = pos;
    }
}
