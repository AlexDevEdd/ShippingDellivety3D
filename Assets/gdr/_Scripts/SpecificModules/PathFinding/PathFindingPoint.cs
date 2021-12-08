using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingPoint : MonoBehaviour
{
    private List<PathFindingPoint> availableMovements;

    public List<PathFindingPoint> AvailableMovements => availableMovements;

    public void InitAvailableMovements(List<PathFindingPoint> pathFindingPoints)
    {
        availableMovements = new List<PathFindingPoint>();
        availableMovements.AddRange(pathFindingPoints);
    }

    public void DrawPathToAvailableMovements()
    {
        foreach (PathFindingPoint path in availableMovements)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, path.transform.position);
        }
    }
}
