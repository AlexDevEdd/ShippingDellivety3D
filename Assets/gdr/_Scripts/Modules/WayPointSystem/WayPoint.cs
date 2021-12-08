using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [NaughtyAttributes.ReadOnly]
    public List<WayLink> wayLinks;
    public SerializableDictionaryBase<int, string> asdfwl;

    private void Awake()
    {
        WayPointSystem.Instance.RegisterWP(this);
    }

    public WayLink TryGetLinkToPoint(WayPoint p)
    {
        foreach(var link in wayLinks)
        {
            if (link.Contains(p))
                return link;
        }
        return null;
    }

    public List<WayPoint> GetNeighborPoints()
    {
        var list = new List<WayPoint>();
        foreach(var link in wayLinks)
        {
            list.Add(link.GetInverse(this));
        }
        return list;
    }

    public List<WayPoint> GetBuiltNeighborPoints()
    {
        var list = new List<WayPoint>();
        foreach (var link in wayLinks)
        {
            if (link.isBuilt)
            {
                var wp = link.GetInverse(this);
                list.Add(wp);
            }
        }
        return list;
    }

    [NaughtyAttributes.Button]
    private void Editor_RemoveLinks()
    {
        if (!Application.isPlaying)
            wayLinks = new List<WayLink>();
    }
}
