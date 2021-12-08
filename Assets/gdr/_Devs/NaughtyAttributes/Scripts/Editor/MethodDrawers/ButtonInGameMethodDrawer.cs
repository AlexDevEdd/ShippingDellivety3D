using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [MethodDrawer(typeof(ButtonInGameAttribute))]
    public class ButtonInGameMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length == 0)
            {
                ButtonInGameAttribute buttonAttribute = (ButtonInGameAttribute)methodInfo.GetCustomAttributes(typeof(ButtonInGameAttribute), true)[0];
                string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;

                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = buttonAttribute.m_color;

                float space = buttonAttribute.Space;

                if (space > 0f)
                    GUILayout.Space(space);

                if (GUILayout.Button(buttonText, style))
                {
                    if (Application.isPlaying)
                        methodInfo.Invoke(target, null);
                    else
                        Debug.LogWarning("You can press this button only in game mode!");
                }
            }
            else
            {
                string warning = typeof(ButtonInGameAttribute).Name + " works only on methods with no parameters";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}
