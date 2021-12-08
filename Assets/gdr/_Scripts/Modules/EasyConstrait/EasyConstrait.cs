using UnityEngine;

public class EasyConstrait : MonoBehaviour
{
    public Transform controllerTr;
    public Transform mainObjectTr;
    public Transform insideObjectTr;
    public Vector3 posOffset;
    public Vector3 rotOffset;

    private void Awake()
    {
        if (controllerTr == null)
            controllerTr = transform;
    }

    public void Update()
    {
        if (mainObjectTr == null || insideObjectTr == null)
            return;

        var offsetRot = controllerTr.rotation * Quaternion.Euler(rotOffset);
        var fromControllToThisRot = offsetRot * Quaternion.Inverse(insideObjectTr.rotation);
        var DoRotateOnAnotherObject = fromControllToThisRot * mainObjectTr.rotation;
        mainObjectTr.rotation = DoRotateOnAnotherObject;

        var vector = mainObjectTr.position - insideObjectTr.position;
        var pos = controllerTr.position + vector;
        mainObjectTr.position = pos + controllerTr.rotation * posOffset;
    }
}
