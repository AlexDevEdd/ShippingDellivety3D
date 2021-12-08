using SWS;
using System.Linq;
using UnityEngine;

namespace SWS
{
    public static class PathManagerExtension
    {
        public enum Type
        {
            Point,
            PointLine,
            CurwedPoint,
            CurwedPointLine
        }

        public static Vector3 GetNearCurvePoint(this PathManager pathManager, Vector3 hitPoint, Type type)
        {
            Vector3 result;
            if (type == Type.Point || type == Type.PointLine)
            {
                var wps = pathManager.waypoints;
                result = GetNearCurvePoint(wps, hitPoint, type);
            }
            else // type == Type.CurwedPoint / CurwedPointLine
            {
                Vector3[] wpPositions = pathManager.GetPathPoints();
                var wpsCurved = WaypointManager.GetCurved(wpPositions);
                result = GetNearCurvePoint(wpsCurved, hitPoint, type);
            }
            return result;
        }

        public static Quaternion GetNearCurveRotation(this PathManager pathManager, Vector3 hitPoint, Type type)
        {
            Quaternion result;
            if (type == Type.Point || type == Type.PointLine)
            {
                var wps = pathManager.waypoints;
                result = GetNearCurveRotation(wps, hitPoint, type);
            }
            else // type == Type.CurwedPoint / CurwedPointLine
            {
                Vector3[] wpPositions = pathManager.GetPathPoints();
                var wpsCurved = WaypointManager.GetCurved(wpPositions);
                result = GetNearCurveRotation(wpsCurved, hitPoint, type);
            }
            return result;
        }

        private static Quaternion GetNearCurveRotation(Transform[] wps, Vector3 hitPoint, Type type)
        {
            return GetNearCurveRotation(wps.Select(x => x.position).ToArray(), hitPoint, type);
        }

        private static Quaternion GetNearCurveRotation(Vector3[] wps, Vector3 hitPoint, Type type)
        {
            Quaternion result;
            if (type == Type.Point)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = GetRotationForVector(wps, nearIndex);
            }
            else if (type == Type.PointLine)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = GetRotationForVector(wps, nearIndex);
            }
            else if (type == Type.CurwedPoint)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = GetRotationForVector(wps, nearIndex);
            }
            else // if (type == Type.CurwedPointLine)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = GetRotationForVector(wps, nearIndex);
            }
            return result;
        }

        private static Quaternion GetRotationForVector(Vector3[] wps, int id)
        {
            int nextId = id + 1;
            if (nextId >= wps.Length)
            {
                nextId--;
                id--;
            }
            return GetRotationForVector(wps[id], wps[nextId]);
        }

        private static Quaternion GetRotationForVector(Vector3 current, Vector3 next)
        {
            var direction = next - current;
            return Quaternion.FromToRotation(Vector3.forward, direction);
        }

        private static Vector3 GetNearCurvePoint(Transform[] wps, Vector3 hitPoint, Type type)
        {
            return GetNearCurvePoint(wps.Select(x => x.position).ToArray(), hitPoint, type);
        }

        private static Vector3 GetNearCurvePoint(Vector3[] wps, Vector3 hitPoint, Type type)
        {
            Vector3 result;
            if (type == Type.Point)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = wps[nearIndex];
            }
            else if (type == Type.PointLine)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = GetPointOnLine(wps, nearIndex, hitPoint);
            }
            else if (type == Type.CurwedPoint)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = wps[nearIndex];
            }
            else // if (type == Type.CurwedPointLine)
            {
                var nearIndex = GetNearestIndexWaypoint(wps, hitPoint);
                result = GetPointOnLine(wps, nearIndex, hitPoint);
            }
            return result;
        }

        private static int GetNearestIndexWaypoint(Vector3[] wps, Vector3 hitPoint)
        {
            int index = 0;

            // var wps = pathManager.waypoints;
            float minDistance = float.PositiveInfinity;

            int id = 0;
            foreach (var wp in wps)
            {
                var distance = Vector3.Distance(hitPoint, wp);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = id;
                }
                id++;
            }
            return index;
        }

        private static Vector3 GetPointOnLine(Vector3[] points, int currentIndex, Vector3 point)
        {
            var prevIndex = currentIndex - 1;
            if (prevIndex < 0) prevIndex = 0;
            var nextIndex = currentIndex + 1;
            if (nextIndex > points.Length - 1) nextIndex = points.Length - 1;

            var distancePrev = Vector3.Distance(points[prevIndex], point);
            var distanceNext = Vector3.Distance(points[nextIndex], point);

            if (distanceNext < distancePrev)
            {
                if (nextIndex == currentIndex)
                    return point;

                var vector = points[nextIndex] - points[currentIndex];
                var pointGlobal = point - points[currentIndex];
                var project = Vector3.Project(pointGlobal, vector);
                return project + points[currentIndex];
            }
            else
            {
                if (prevIndex == currentIndex)
                    return point;

                var vector = points[prevIndex] - points[currentIndex];
                var pointGlobal = point - points[currentIndex];
                var project = Vector3.Project(pointGlobal, vector);
                return project + points[currentIndex];
            }
        }
    }
}