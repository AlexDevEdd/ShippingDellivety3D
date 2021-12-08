using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [MethodDrawer(typeof(ButtonAttribute))]
    public class ButtonMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length == 0)
            {
                ButtonAttribute buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;

                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = buttonAttribute.m_color;

                float space = buttonAttribute.Space;

                if (space > 0f)
                    GUILayout.Space(space);

                if (GUILayout.Button(buttonText, style))
                {
                    methodInfo.Invoke(target, null);
                }
            }
            else
            {
                string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}
