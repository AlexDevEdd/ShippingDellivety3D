using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayLink : MonoBehaviour
{
    public static bool isDebug;
    public static Color debugColor;

    [SerializeField] private WayPoint p1;
    public WayPoint P1 => p1;
    [SerializeField] private WayPoint p2;
    public WayPoint P2 => p2;

    public bool isInitialized = false;
    public bool isBuilt = false;

    [Header("Additional Info")]
    public float distance; // (p1 - p2).magnitude

    private void Awake()
    {
        WayPointSystem.Instance.RegisterLink(this);

        if (p1 == null || p2 == null) return;

        SetWayPoints(p1, p2);
    }

    public void SetWayPoints(WayPoint p1, WayPoint p2)
    {
        if (p1 == null || p2 == null)
            Debug.LogError($"Cant set empty p1, p2");
        this.p1 = p1;
        this.p2 = p2;
        if (!p1.wayLinks.Contains(this))
            p1.wayLinks.Add(this);
        if (!p2.wayLinks.Contains(this))
            p2.wayLinks.Add(this);
        isInitialized = true;
    }

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            if (isBuilt)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(p1.transform.position, p2.transform.position);
            }
            else
            {
                Gizmos.color = debugColor;
                Gizmos.DrawLine(p1.transform.position, p2.transform.position);
            }
        }
        else
        {
            if (isBuilt)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(p1.transform.position, p2.transform.position);
            }
            else
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(p1.transform.position, p2.transform.position);
            }
        }
    }

    public void Build()
    {
        isBuilt = true;
    }

    public void Destruct()
    {
        isBuilt = false;
    }

    public bool Contains(WayPoint p)
    {
        return p1 == p || p2 == p;
    }

    public WayPoint GetInverse(WayPoint p)
    {
        if (p == p1)
            return p2;
        if (p == p2)
            return p1;
        return null;
    }

    public void CalcDistance()
    {
        if (p1 == null || p2 == null)
            return;

        distance = (p1.transform.position - p2.transform.position).magnitude;
    }
}
