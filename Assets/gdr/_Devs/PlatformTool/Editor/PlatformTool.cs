using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

// Tagging a class with the EditorTool attribute and no target type registers a global tool. Global tools are valid for any selection, and are accessible through the top left toolbar in the editor.
[EditorTool("Platform Tool")]
class PlatformTool : EditorTool
{
    // Serialize this value to set a default value in the Inspector.
    [SerializeField]
    Texture2D m_ToolIcon;

    GUIContent m_IconContent;

    void OnEnable()
    {
        m_IconContent = new GUIContent()
        {
            image = m_ToolIcon,
            text = "Platform Tool",
            tooltip = "Platform Tool"
        };
    }

    public override GUIContent toolbarIcon
    {
        get { return m_IconContent; }
    }

    public override void OnToolGUI(EditorWindow window)
    {
        var selection = Selection.activeGameObject;
        while (selection != null)
        {
            // var testUI = selection.GetComponent<TestUI>();
            var testUI = selection.GetComponent<GeneratedUI>();
            if (testUI != null)
            {
                using (new Handles.DrawingScope(Color.green))
                {
                    Handles.Label(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 1.8f + Vector3.left * Camera.main.pixelWidth / 2f, "ShowAllUI");
                    if (Handles.Button(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 2f + Vector3.left * Camera.main.pixelWidth / 2f, Quaternion.identity, 100f, 100f, Handles.SphereHandleCap))
                    {
                        testUI.ShowAllUI();
                    }
                }
                using (new Handles.DrawingScope(Color.red))
                {
                    Handles.Label(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 2f + Vector3.left * Camera.main.pixelWidth / 4f, "HideAllUI");
                    if (Handles.Button(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 2f + Vector3.left * Camera.main.pixelWidth / 4f, Quaternion.identity, 100f, 100f, Handles.SphereHandleCap))
                    {
                        testUI.HideAllUI();
                    }
                }
                using (new Handles.DrawingScope(Color.blue))
                {
                    Handles.Label(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 1.8f + Vector3.right * Camera.main.pixelWidth / 4f, "PositionEditorOnly");
                    if (Handles.Button(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 2f + Vector3.right * Camera.main.pixelWidth / 4f, Quaternion.identity, 100f, 100f, Handles.SphereHandleCap))
                    {
                        testUI.PositionEditorOnly();
                    }
                }
                using (new Handles.DrawingScope(Color.cyan))
                {
                    Handles.Label(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 2f + Vector3.right * Camera.main.pixelWidth / 2f, "PositionGameReady");
                    if (Handles.Button(testUI.transform.position + Vector3.up * Camera.main.pixelHeight / 2f + Vector3.right * Camera.main.pixelWidth / 2f, Quaternion.identity, 100f, 100f, Handles.SphereHandleCap))
                    {
                        testUI.PositionGameReady();
                    }
                }
                break;
            }
            else if (selection.transform.parent != null)
                selection = selection.transform.parent.gameObject;
            else
                selection = null;
        }
    }
}