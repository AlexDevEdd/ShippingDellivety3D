using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointSystem : MonoBehaviour
{
    #region Singleton Init
    private static WayPointSystem _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
        {
            Debug.Log($"Destroying {gameObject.name}, caused by one singleton instance");
            Destroy(gameObject);
        }
    }

    public static WayPointSystem Instance // Init not in order
    {
        get
        {
            if (_instance == null)
                Init();
            return _instance;
        }
        private set { _instance = value; }
    }

    static void Init() // Init script
    {
        _instance = FindObjectOfType<WayPointSystem>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public GameObject wayPointPrefab;
    public GameObject wayLinkPrefab; // cause of prefab, and cause all in one place, it's added to WebSystem and not to another script like WebLinker
    public Transform wayLinksTr;
    private List<WayLink> wayLinks = new List<WayLink>();
    private List<WayPoint> wayPoints = new List<WayPoint>();
    public List<WayPoint> WayPoints => wayPoints;

    [Header("Path Finding")]
    private Dictionary<(WayPoint from, WayPoint to), WayPoint> pathMap = new Dictionary<(WayPoint from, WayPoint to), WayPoint>(); // from, to => next
    private Dictionary<(WayPoint from, WayPoint to), WayPoint> pathMapBuilt = new Dictionary<(WayPoint from, WayPoint to), WayPoint>(); // from, to => next

    [Header("Test Path")]
    public WayPoint testPathFrom;
    public WayPoint testPathTo;
    public WayPoint testNextPoint;

    void Initialize()
    {
        // Init data here
        enabled = true;
    }

    public void RegisterWP(WayPoint wp)
    {
        wayPoints.Add(wp);
    }

    public void RegisterLink(WayLink wl)
    {
        wayLinks.Add(wl);
    }

    public WayPoint AddPoint(Transform parent = null)
    {
        GameObject point;
        if (wayPointPrefab == null)
        {
            point = new GameObject("WayPoint");
            point.transform.SetParent(parent);
            point.transform.localPosition = Vector3.zero;
            point.transform.localRotation = Quaternion.identity;
            point.AddComponent<WayPoint>();
        }
        else
            point = Instantiate(wayPointPrefab, parent);
        var script = point.GetComponent<WayPoint>();
        // wayPoints.Add(script);
        return script;
    }

    public WayLink AddLink(WayPoint p1, WayPoint p2)
    {
        if (p1 == null || p2 == null) return null;
        if (p1.TryGetLinkToPoint(p2) != null) return null;

        GameObject link;
        if (wayPointPrefab == null)
        {
            link = new GameObject("WayLink");
            link.transform.SetParent(wayLinksTr);
            link.transform.localPosition = Vector3.zero;
            link.transform.localRotation = Quaternion.identity;
            link.AddComponent<WayLink>();
        }
        else
            link = Instantiate(wayLinkPrefab, wayLinksTr);
        var script = link.GetComponent<WayLink>();
        script.SetWayPoints(p1, p2);
        // wayLinks.Add(script);
        return script;
    }

    public WayPoint GetNearestPoint(Vector3 pos)
    {
        WayPoint wp = null;
        float distance = 1e36f;
        foreach (var p in wayPoints)
        {
            float d = (p.transform.position - pos).magnitude;
            if (d < distance)
            {
                wp = p;
                distance = d;
            }
        }
        return wp;
    }

    public WayPoint GetNextMovePoint(WayPoint current, WayPoint next, bool isBuiltOnly = false)
    {
        var cortage = (current, next);
        var path = (isBuiltOnly ? pathMapBuilt : pathMap);
        if (path != null && path.ContainsKey(cortage))
            return path[cortage];
        return null;
    }

    public List<WayPoint> GetMovePath(WayPoint current, WayPoint next, bool isBuiltOnly = false)
    {
        var cortage = (current, next);
        var path = (isBuiltOnly ? pathMapBuilt : pathMap);
        if (path != null && path.ContainsKey(cortage))
        {
            var list = new List<WayPoint>();
            var p = current;
            list.Add(p); // Add first
            while (p != next)
            {
                p = GetNextMovePoint(p, next);
                list.Add(p); // Add p1,p2,p3,next
            }
        }
        return null;
    }

    public bool IsLinkAdded(WayPoint p1, WayPoint p2, bool isBuiltOnly = false)
    {
        foreach (var link in p1.wayLinks)
        {
            if (link.Contains(p2))
            {
                if (isBuiltOnly)
                    return link.isBuilt;
                else
                    return true;
            }
        }
        return false;
    }

    public void DestroyPoints()
    {
        foreach (var wp in wayPoints)
        {
            if (wp != null)
            {
                if (Application.isPlaying)
                    Destroy(wp.gameObject);
                else
                    DestroyImmediate(wp.gameObject);
            }
        }
        wayPoints = new List<WayPoint>();
    }

    public void DestroyLinks()
    {
        foreach (var link in wayLinks)
        {
            if (link != null)
            {
                if (Application.isPlaying)
                    Destroy(link.gameObject);
                else
                    DestroyImmediate(link.gameObject);
            }
        }
        wayLinks = new List<WayLink>();
    }

    [NaughtyAttributes.Button]
    public void BuildPath() // Should be very heavy operation. Be careful
    {
        pathMap = new Dictionary<(WayPoint from, WayPoint to), WayPoint>();

        foreach (var link in wayLinks)
            link.CalcDistance();

        int waypointCount = wayPoints.Count;
        for (int i = 0; i < waypointCount; i++)
            for (int j = 0; j < waypointCount; j++)
            {
                var from = wayPoints[i];
                var to = wayPoints[j];
                if (from == to)
                    TryAdd(from, to, to); // Self to self
                else if (from.TryGetLinkToPoint(to) != null) // direct path is the fastest by default
                    TryAdd(from, to, to); // Neighbors
                else
                {
                    var cortageResult = BuildFromPointToPoint(from, to);
                    TryAdd(from, to, cortageResult.resultPath[1]);
                }
                var cortage = (from, to);
                if (pathMap.ContainsKey(cortage))
                    Debug.Log($"Contains {from} => {to}");
            }

        Debug.Log($"Created paths: {pathMap.Count}");
    }

    [NaughtyAttributes.Button]
    private void TestFromTo()
    {
        var cortage = (testPathFrom, testPathTo);
        if (pathMap.ContainsKey(cortage))
            testNextPoint = pathMap[cortage];
    }

    private (bool isPathFound, List<WayPoint> resultPath) BuildFromPointToPoint(WayPoint from, WayPoint to)
    {
        Queue<List<WayPoint>> paths = new Queue<List<WayPoint>>();
        var initPath = new List<WayPoint>();
        initPath.Add(from);
        paths.Enqueue(initPath);

        Dictionary<WayPoint, float> wpDistances = new Dictionary<WayPoint, float>();

        bool isPathFound = false;
        List<WayPoint> resultPath = new List<WayPoint>();

        while (paths.Count > 0)
        {
            var rootPath = paths.Dequeue();
            var wp = rootPath[rootPath.Count - 1];
            var neighbors = wp.GetNeighborPoints();
            foreach (var n in neighbors)
            {
                var newPath = new List<WayPoint>();
                newPath.AddRange(rootPath);
                newPath.Add(n);
                var distance = GetDistance(newPath);

                if (wpDistances.ContainsKey(n)) // Process only points with lowest distance
                {
                    if (wpDistances[n] > distance)
                    {
                        if (n == to)
                        {
                            isPathFound = true;
                            resultPath = newPath;
                        }
                        else
                            paths.Enqueue(newPath);
                        wpDistances[n] = distance;
                    }
                    // Else skip same or higher distances
                }
                else
                {
                    if (n == to)
                    {
                        isPathFound = true;
                        resultPath = newPath;
                    }
                    else
                        paths.Enqueue(newPath);
                    wpDistances.Add(n, distance);
                }
            }
        }

        return (isPathFound, resultPath);
    }

    private float GetDistance(List<WayPoint> path)
    {
        float distance = 0f;
        for (int i = 1; i < path.Count; i++)
            distance += (path[i - 1].transform.position - path[i].transform.position).magnitude;
        return distance;
    }

    private void TryAdd(WayPoint from, WayPoint to, WayPoint next)
    {
        //if (pathMap == null)
        //    pathMap = new Dictionary<(WayPoint from, WayPoint to), WayPoint>();
        var cortage = (from, to);
        if (pathMap.ContainsKey(cortage))
            pathMap[cortage] = next;
        else
            pathMap.Add(cortage, next);
    }
}
