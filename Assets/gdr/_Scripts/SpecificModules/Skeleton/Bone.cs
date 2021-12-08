using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public Vector3 myRotation;
    public Vector3 eulerAngle;
    public Quaternion quaternion;
    public V4 quat;

    public Transform leadingBone;
    public Transform parentBone;

    public float leadingBoneDistance;
    public float parentBoneDistance;

    public bool isFixedPosition;
    public bool isLimitedRotation;
    public Vector3 rotationLimitMin;
    public Vector3 rotationLimitMax;

    public Vector3 priorityPos;
    [System.Serializable]
    public class V4
    {
        public float x, y, z, w;
    }

    [NaughtyAttributes.Button]
    public void Init()
    {
        if (leadingBone != null)
            leadingBoneDistance = (transform.position - leadingBone.position).magnitude;
        if (parentBone != null)
            parentBoneDistance = (transform.position - parentBone.position).magnitude;
    }

    private void Update()
    {
        // Just info
        eulerAngle = transform.rotation.eulerAngles;
        quaternion = transform.rotation;
        quat.x = quaternion.x;
        quat.y = quaternion.y;
        quat.z = quaternion.z;
        quat.w = quaternion.w;
    }

    public void DoUpdate()
    {
        // Leading bone
        // If we arent fixed, we look at target direction and attach to it at desired position
        // This makes rotation things, we should try do math, if we can.
        if (leadingBone != null)
        {
            // var magnitude = (transform.position - leadingBone.position).magnitude;
            if (!isFixedPosition)
            {
                var dir = (leadingBone.position - transform.position).normalized;
                var pos = leadingBoneDistance * dir + transform.position;
                transform.position = pos;
                priorityPos = pos + leadingBoneDistance * dir;
            }
        }

        // If we have parent bone, we should now
        if (parentBone != null)
        {
            var magnitude = (transform.position - parentBone.position).magnitude;
            var dir = (parentBone.transform.position - transform.position).normalized;
            var pos = (magnitude - parentBoneDistance) * dir + transform.position;
            transform.position = pos;
        }

        transform.LookAt(leadingBone.position);

        //if (isLimitedRotation)
        //{
        //    var euler = transform.localRotation.eulerAngles;
        //    Debug.Log(euler);

        //    if (euler.x > 180f)
        //        euler.x -= 360f;
        //    if (euler.y > 180f)
        //        euler.y -= 360f;
        //    if (euler.z > 180f)
        //        euler.z -= 360f;
        //    euler.x = 180f - Mathf.Abs(euler.x);
        //    euler.y = 180f - Mathf.Abs(euler.y);
        //    euler.z = 180f - Mathf.Abs(euler.z);

        //    bool isLimited = false;

        //    if (euler.x < rotationLimitMin.x)
        //    {
        //        isLimited = true;
        //        euler.x = rotationLimitMin.x;
        //    }
        //    else if (euler.x > rotationLimitMax.x)
        //    {
        //        isLimited = true;
        //        euler.x = rotationLimitMax.x;
        //    }

        //    if (euler.y < rotationLimitMin.y)
        //    {
        //        isLimited = true;
        //        euler.y = rotationLimitMin.y;
        //    }
        //    else if (euler.y > rotationLimitMax.y)
        //    {
        //        isLimited = true;
        //        euler.y = rotationLimitMax.y;
        //    }

        //    if (euler.z < rotationLimitMin.z)
        //    {
        //        isLimited = true;
        //        euler.z = rotationLimitMin.z;
        //    }
        //    else if (euler.z > rotationLimitMax.z)
        //    {
        //        isLimited = true;
        //        euler.z = rotationLimitMax.z;
        //    }

        //    transform.localRotation = Quaternion.Euler(euler);

        //    if (isLimited)
        //    {
        //        var dir = transform.forward;
        //        var pos = leadingBoneDistance * dir + transform.position;
        //        leadingBone.transform.position = pos;
        //        leadingBone.transform.LookAt(leadingBone.GetComponent<Bone>().leadingBone);
        //    }
        //}
    }
}
