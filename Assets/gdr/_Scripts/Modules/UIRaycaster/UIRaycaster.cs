using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class UIRaycaster
{
    private static GraphicRaycaster _raycaster;
    private static EventSystem _eventSystem;

    //public bool isRaycastedUI; // Update: Not relevant during probability of errors during execution order

    private static bool InitRaycasters()
    {
        if (_raycaster == null) _raycaster = GameObject.FindObjectOfType<GraphicRaycaster>();
        if (_eventSystem == null) _eventSystem = GameObject.FindObjectOfType<EventSystem>();

        if (_raycaster == null)
        {
            Debug.LogError($"Cant find any graphic raycaster");
            return false;
        }
        if (_eventSystem == null)
        {
            Debug.LogError($"Cant find any event system");
            return false;
        }
        return true;
    }

    public static void InitRaycasters(GraphicRaycaster gr, EventSystem es)
    {
        _raycaster = gr;
        _eventSystem = es;
    }

    public static bool IsRaycastedUI() // No info about raycasted things used
    {
        if (!InitRaycasters())
            return false;

        var pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
            return true;
        return false;
    }

    public static bool IsRaycastedUI(string skip) // No info about raycasted things used
    {
        if (!InitRaycasters())
            return false;

        var pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            bool isAllSkip = true;
            foreach (var r in results)
                if (r.gameObject.name != skip)
                    isAllSkip = false;
            if (!isAllSkip)
                return true;
        }
        return false;
    }

    public static bool IsRaycastedUI(params string[] skips) // No info about raycasted things used
    {
        if (!InitRaycasters())
            return false;

        var pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            foreach (var r in results)
            {
                bool isSkipped = false;
                foreach (var f in skips)
                {
                    if (r.gameObject.name == f)
                        isSkipped = true;
                }
                if (!isSkipped)
                    return true;
            }
        }
        return false;
    }
}
