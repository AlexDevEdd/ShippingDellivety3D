using UnityEngine;

public class RaycastHelper : MonoBehaviour
{
    public static Vector3 CameraRayPoint(Camera camera, float distanceFromCamera)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * distanceFromCamera;
    }

    /// <returns>Is raycasted something</returns>
    public static bool DoRaycast(Camera camera, out RaycastHit raycastHit, float maxDistance)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, maxDistance, LayerMask.GetMask(new string[] { "Default" })))
            return true;
        return false;
    }

    /// <returns>Is raycasted something</returns>
    public static bool DoRaycast(Camera camera, out RaycastHit raycastHit, float maxDistance, string layer)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, maxDistance, LayerMask.GetMask(new string[] { layer })))
            return true;
        return false;
    }

    /// <returns>Is world position on front of camera</returns>
    public static bool WorldToScreen(Camera camera, Vector3 worldObjectPos, out Vector2 screenPos)
    {
        screenPos = camera.WorldToScreenPoint(worldObjectPos);
        if (Vector3.Dot(camera.transform.forward, (worldObjectPos - camera.transform.position)) > 0)
            return true;
        return false;
    }
}
