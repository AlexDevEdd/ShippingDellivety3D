using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRotationHandle : MonoBehaviour
{
    private static bool isDrawGizmo = true;

    [Header("Settings")]
    public Transform anchorTr;
    public Transform handleTr;
    public float minDistance;
    public float maxDistance;

    [Header("Limits")]
    public bool isLimitVertical = true;
    public float rotationLimitVertical = 45f;
    public float rotationLimitHorizontal = 45f;

    [Header("Info")]
    public float currentVertical;
    public float currentHorizontal;
    public float currentDistance;

    // Just to show how it works we put it into Update, you should call FixHandle manual in your script!
    private void Update()
    {
        // Main logic is to use this after your calculations to fix position of item
        // And then call method with fixed pos
        FixHandle();
    }

    public void FixHandle()
    {
        // vector to handle (current)
        var vector = handleTr.position - anchorTr.position;

        // recalc distance
        var distance = vector.magnitude;
        if (distance > maxDistance)
            distance = maxDistance;
        if (distance < minDistance)
            distance = minDistance;

        // get normalized vector to handle
        var norm = vector.normalized;

        // try put handle normal on plane, plane is anchorTr
        var normProjected = Vector3.ProjectOnPlane(norm, anchorTr.up);
        // calc normal and projected angle in between
        var angleY = Vector3.Angle(anchorTr.forward, normProjected);

        // current pos without changes either than distance recalculated
        var pos = anchorTr.position + norm * distance;

        // if angle exceed Y limit
        if (angleY > rotationLimitHorizontal)
        {
            // calc real angle between [-90, 90] works well
            var realAngleY = Vector3.SignedAngle(anchorTr.forward, normProjected, anchorTr.up);
            float targetRotation;
            if (realAngleY < 0f)
                targetRotation = realAngleY + rotationLimitHorizontal;
            else
                targetRotation = realAngleY - rotationLimitHorizontal;

            var quat = Quaternion.LookRotation(norm, anchorTr.up) * Quaternion.Euler(0f, -targetRotation, 0f);
            var newDir = quat * Vector3.forward;
            pos = newDir * distance + anchorTr.position;
        }

        // calc new vector to Y fixed pos of handle
        vector = pos - anchorTr.position;
        // recalc distance if necessary (hope not)
        distance = vector.magnitude;
        if (distance > maxDistance)
            distance = maxDistance;
        if (distance < minDistance)
            distance = minDistance;
        // new normal vector is here
        norm = vector.normalized;
        // project norm vector to plane again, get forward vector norm
        normProjected = Vector3.ProjectOnPlane(norm, anchorTr.up);
        // Get angle in horizontal axis
        var angleX = Vector3.Angle(norm, normProjected);

        var realAngleX2 = Vector3.SignedAngle(norm, normProjected, Vector3.up); // there is no up probably
        var locNorm2 = anchorTr.InverseTransformDirection(norm);
        // Debug.Log($"realAngleX:{realAngleX2}, locNorm2: {locNorm2}");

        if (isLimitVertical && angleX > rotationLimitVertical)
        {
            // Debug.Log($"realAngleX:{realAngleX}");
            var locNorm = anchorTr.InverseTransformDirection(norm);
            float targetRotation;
            if (locNorm.y < 0f)
                targetRotation = -angleX + rotationLimitVertical;
            else
                targetRotation = angleX - rotationLimitVertical;

            var quat = Quaternion.LookRotation(norm, anchorTr.up) * Quaternion.Euler(targetRotation, 0f, 0f);

            var newDir = quat * Vector3.forward;
            pos = newDir * distance + anchorTr.position;
        }

        handleTr.position = pos;

        // show info
        vector = handleTr.position - anchorTr.position;
        currentDistance = vector.magnitude;
        norm = vector.normalized;
        normProjected = Vector3.ProjectOnPlane(norm, anchorTr.up);
        // calc normal and projected angle in between
        currentHorizontal = Vector3.SignedAngle(anchorTr.forward, normProjected, anchorTr.up);
        currentVertical = Vector3.SignedAngle(norm, normProjected, anchorTr.up);

        var loc = anchorTr.InverseTransformDirection(norm);
        if (loc.y < 0f)
            currentVertical = -currentVertical;
    }

    private void OnDrawGizmos()
    {
        if (!isDrawGizmo) return;

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(anchorTr.position, minDistance);
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(anchorTr.position, maxDistance);

        // Draw forward
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(anchorTr.position, anchorTr.position + anchorTr.forward);

        // Draw limit Y
        Gizmos.color = Color.green;
        Gizmos.DrawLine(anchorTr.position, anchorTr.position + anchorTr.rotation * Quaternion.Euler(0f, rotationLimitHorizontal, 0f) * Vector3.forward);
        Gizmos.DrawLine(anchorTr.position, anchorTr.position + anchorTr.rotation * Quaternion.Euler(0f, -rotationLimitHorizontal, 0f) * Vector3.forward);

        // Draw limit X
        if (isLimitVertical)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(anchorTr.position, anchorTr.position + anchorTr.rotation * Quaternion.Euler(rotationLimitVertical, 0f, 0f) * Vector3.forward);
            Gizmos.DrawLine(anchorTr.position, anchorTr.position + anchorTr.rotation * Quaternion.Euler(-rotationLimitVertical, 0f, 0f) * Vector3.forward);
        }
    }
}
