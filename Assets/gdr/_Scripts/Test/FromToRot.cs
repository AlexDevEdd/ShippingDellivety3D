using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromToRot : MonoBehaviour
{
    public Transform fromRotation;
    public Quaternion quat;

    public Transform addToRotation;

    public bool isSwap;
    public bool isInverseFirst;
    public bool isInverseSecond;

    [NaughtyAttributes.Button]
    private void SetupQuaternion()
    {
        quat = Quaternion.FromToRotation(Vector3.forward, fromRotation.forward);
    }

    [NaughtyAttributes.Button]
    private void ApplyQuaternion()
    {
        addToRotation.rotation *= GetQ(addToRotation.rotation, quat);
    }

    [NaughtyAttributes.Button]
    private void ApplyToVector()
    {
        addToRotation.position = GetQ(quat) * addToRotation.position;
    }

    private Quaternion GetQ(Quaternion one)
    {
        Quaternion q1;
            q1 = one;
        if (isInverseFirst)
            q1 = Quaternion.Inverse(q1);

        return q1;
    }

    private Quaternion GetQ(Quaternion one, Quaternion two)
    {
        Quaternion q1, q2;
        if (isSwap)
        {
            q1 = two;
            q2 = one;
        }
        else
        {
            q1 = one;
            q2 = two;
        }
        if (isInverseFirst)
            q1 = Quaternion.Inverse(q1);
        if (isInverseSecond)
            q2 = Quaternion.Inverse(q2);

        return q1 * q2;
    }
}
