using UnityEngine;

public class QBone : MonoBehaviour
{
    public ControllableBone controllableBone;
}

[System.Serializable]
public class ControllableBone
{
    public Transform tr;
    public Transform animationBone;
    public SyncTypes syncTypes;
    public bool isControlledByAnimation = true;
}