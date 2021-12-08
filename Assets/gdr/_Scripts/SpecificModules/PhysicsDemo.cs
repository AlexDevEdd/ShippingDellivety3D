using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDemo : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Main Settings")]
    public float force;
    public enum UpdateType
    {
        Update,
        FixedUpdate,
        LateUpdate
    }
    public UpdateType updateType;

    public enum ForceType
    {
        AddExplosionForce,
        AddForce,
        AddForceAtPosition,
        AddRelativeForce,
        AddRelativeTorque,
        AddTorque
    }
    public ForceType forceType;

    public ForceMode forceMode;

    public Vector3 resetPos;
    public Vector3 resetRot;

    [Header("Option: AddExplosionForce")]
    public float explosionForce;
    public Vector3 explosionPosition;
    public float explosionRadius;
    public float upwardsModifier;

    [Header("Option: AddForceAtPosition")]
    public Vector3 position;

    [Header("Stats")]
    public float execTime = 0f;
    public float deltaTime = 0f;

    private void Update()
    {
        if (updateType == UpdateType.Update)
        {
            Action();
        }
    }

    private void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
        {
            Action();
        }
    }

    private void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
        {
            Action();
        }
    }

    private void Action()
    {
        switch (forceType)
        {
            case ForceType.AddExplosionForce:
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, forceMode);
                break;
            case ForceType.AddForce:
                rb.AddForce(transform.forward * Time.deltaTime * force, forceMode);
                break;
            case ForceType.AddForceAtPosition:
                rb.AddForceAtPosition(transform.forward * Time.deltaTime * force, position, forceMode);
                break;
            case ForceType.AddRelativeForce:
                rb.AddRelativeForce(transform.forward * Time.deltaTime * force, forceMode);
                break;
            case ForceType.AddRelativeTorque:
                rb.AddRelativeTorque(transform.forward * Time.deltaTime * force, forceMode);
                break;
            case ForceType.AddTorque:
                rb.AddTorque(transform.forward * Time.deltaTime * force, forceMode);
                break;
        }
        deltaTime += Time.deltaTime;
    }

    [NaughtyAttributes.Button]
    private void EnableFor1Sec()
    {
        enabled = true;
        execTime = Time.time;
        deltaTime = 0f;
        CoroutineActions.ExecuteAction(1f, () =>
        {
            execTime = Time.time - execTime;
            enabled = false;
        });
    }

    [NaughtyAttributes.Button]
    private void EnableFor1SecAndFreeze()
    {
        enabled = true;
        execTime = Time.time;
        deltaTime = 0f;
        CoroutineActions.ExecuteAction(1f, () =>
        {
            execTime = Time.time - execTime;
            Freeze();
            enabled = false;
        });
    }

    [NaughtyAttributes.Button]
    private void Freeze()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    [NaughtyAttributes.Button]
    private void ResetPos()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = resetPos;
        transform.rotation = Quaternion.Euler(resetRot);
    }
}
