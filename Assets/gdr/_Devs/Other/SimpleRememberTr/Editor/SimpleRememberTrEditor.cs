using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleRememberTr))]
public class SimpleRememberTrEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        SimpleRememberTr script = (SimpleRememberTr)target;

        if (GUILayout.Button("RememberPos"))
        {
            script.RememberPos();
        }
        if (GUILayout.Button("SwapStartAndEnd"))
        {
            script.SwapStartAndEnd();
        }
        if (GUILayout.Button("Clear"))
        {
            script.Clear();
        }
        if (GUILayout.Button("CreateAnchorGO"))
        {
            script.CreateAnchorGO();
        }
        if (GUILayout.Button("CreateAnchorAtEndPosGO"))
        {
            script.CreateAnchorAtEndPosGO();
        }
        if (GUILayout.Button("Undo"))
        {
            script.Undo();
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.LabelField("Press Ctrl + Shift to select GO");
    }

    public void OnSceneGUI()
    {
        if (Event.current.control && Event.current.shift)
        {
            Vector2 guiPosition = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            SimpleRememberTr script = (SimpleRememberTr)target;
            if (Physics.Raycast(ray, out RaycastHit hit, 200f))
            {
                script.m_target = hit.collider.gameObject;
            }
            else
            {
                script.m_target = null;
            }
        }
    }
}

