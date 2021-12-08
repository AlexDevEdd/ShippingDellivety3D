using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    public bool isInitialized { get; private set; } = false;

    private List<PathFindingPoint> points;

    public void InitPathPointsFromWaypointSystem()
    {
        points = new List<PathFindingPoint>();
        var wayPoints = WayPointSystem.Instance.WayPoints;

        if (wayPoints.Count < 1)
            Debug.LogError($"Empty waypoints");

        foreach(var wp in wayPoints)
        {
            var pfp = wp.GetComponent<PathFindingPoint>();
            var available = wp.GetBuiltNeighborPoints().Select((x) => x.GetComponent<PathFindingPoint>()).ToList();
            pfp.InitAvailableMovements(available);
            points.Add(pfp);
        }

        if (points.Count <= 2) throw new System.Exception("Not enough points to build a path");

        isInitialized = true;
    }

    public void Uninitialize()
    {
        isInitialized = false;
    }

    private void InitFromChildrenPathPoints()
    {
        points = new List<PathFindingPoint>();
        points = GetComponentsInChildren<PathFindingPoint>().ToList();

        if (points.Count <= 2) throw new System.Exception("Not enough points to build a path");
    }

    private void DrawPaths()
    {
        InitPathPointsFromWaypointSystem();

        foreach (PathFindingPoint point in points)
        {
            point.DrawPathToAvailableMovements();
        }
    }

    public PathFindingPoint GetTheNearestPathFindingPoint(Transform objectTransform)
    {
        PathFindingPoint theNearestPoint = null;
        float distance = Mathf.Infinity;

        foreach (PathFindingPoint point in points)
        {
            float newDistance = Vector3.Distance(objectTransform.position, point.transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                theNearestPoint = point;
            }
        }

        return theNearestPoint;
    }

    public void BuildPath(PathFindingPoint startPoint, PathFindingPoint targetPoint, List<PathFindingPoint> path)
    {
        path.Add(startPoint);

        bool isFound = false;
        PathFindingPoint nextMovingPoint = startPoint;

        Queue<PathFindingPoint> openList = new Queue<PathFindingPoint>();
        List<PathFindingPoint> closedList = new List<PathFindingPoint>();

        openList.Enqueue(targetPoint);
        closedList.Add(targetPoint);

        while (openList.Count > 0)
        {
            if (isFound) break;

            PathFindingPoint currentPoint = openList.Dequeue();

            if (currentPoint.AvailableMovements == null)
                Debug.LogError($"currentPoint.AvailableMovements == null {currentPoint.gameObject}");

            foreach (PathFindingPoint availablePoint in currentPoint.AvailableMovements)
            {
                if (closedList.Contains(availablePoint)) continue;

                if (availablePoint.Equals(startPoint))
                {
                    // Complete PathFinding
                    nextMovingPoint = currentPoint;
                    isFound = true;
                    break;
                }

                openList.Enqueue(availablePoint);
                closedList.Add(availablePoint);
            }
        }

        if (startPoint == null || targetPoint == null)
            Debug.LogError($"Are you sure you want go from ({startPoint}) to ({targetPoint})");
        if (nextMovingPoint == null)
            Debug.LogError($"Next point is null, sorry");
        if (targetPoint == null)
            Debug.LogError($"Target Point is null, sorry");
        if (nextMovingPoint.Equals(targetPoint))
        {
            // Full Complete Finding
            path.Add(nextMovingPoint);
        }
        else
        {
            // Find Next Moving Point
            BuildPath(nextMovingPoint, targetPoint, path);
        }
    }

    public PathFindingPoint GetSpiderPoint()
    {
        // TODO: Get the Nearest

        return points[0];
    }

    public PathFindingPoint GetNextRandomPoint(PathFindingPoint currentPoint)
    {
        PathFindingPoint newPoint;

        do
        {
            newPoint = points[Random.Range(0, points.Count)];
        }
        while (newPoint == currentPoint);

        return newPoint;
    }
}
