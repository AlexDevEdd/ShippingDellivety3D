using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyReuse : MonoBehaviour
{
    [SerializeField] private bool isReuseOnEnable = true;
    [SerializeField] private List<Rigidbody> rigidbodies;
    private List<Vector3> defaultLocalPos;
    private List<Quaternion> defaultLocalRotation;

    void Awake()
    {
        defaultLocalPos = new List<Vector3>();
        defaultLocalRotation = new List<Quaternion>();
        foreach (var rb in rigidbodies)
        {
            defaultLocalPos.Add(rb.transform.localPosition);
            defaultLocalRotation.Add(rb.transform.localRotation);
        }
    }

    private void OnEnable()
    {
        if (isReuseOnEnable)
            Reuse();
    }

    public void Reuse()
    {
        int count = rigidbodies.Count;
        for (int i = 0; i < count; i++)
        {
            var tr = rigidbodies[i].transform;
            tr.localPosition = defaultLocalPos[i];
            tr.localRotation = defaultLocalRotation[i];
            rigidbodies[i].velocity = Vector3.zero;
            rigidbodies[i].angularVelocity = Vector3.zero;
        }
    }
}
