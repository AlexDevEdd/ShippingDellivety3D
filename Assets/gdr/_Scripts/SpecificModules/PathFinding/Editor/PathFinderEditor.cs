using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathFinder))]
public class PathFinderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PathFinder pathFinder = (PathFinder)target;

        if (GUILayout.Button("InitPathPointsFromWaypointSystem"))
        {
            pathFinder.InitPathPointsFromWaypointSystem();
        }
    }
}
