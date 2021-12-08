using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [MethodDrawer(typeof(ButtonShowIfAttribute))]
    public class ButtonShowIfMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
        {
            //if (!CanDrawProperty(target, ))
            //    return;
            if (methodInfo.GetParameters().Length == 0)
            {
                ButtonShowIfAttribute buttonAttribute = (ButtonShowIfAttribute)methodInfo.GetCustomAttributes(typeof(ButtonShowIfAttribute), true)[0];
                //string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;
                string buttonText = methodInfo.Name;
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

        public ButtonShowIfAttribute GetButtonShowAttribute(MethodInfo methodInfo)
        {
            return (ButtonShowIfAttribute)methodInfo.GetCustomAttributes(typeof(ButtonShowIfAttribute), true)[0];
        }

        public bool CanDrawProperty(UnityEngine.Object target, MethodInfo methodInfo)
        {
            ButtonShowIfAttribute showIfAttribute = (ButtonShowIfAttribute)methodInfo.GetCustomAttributes(typeof(ButtonShowIfAttribute), true)[0];
            List<bool> conditionValues = new List<bool>();
            foreach (var condition in showIfAttribute.Conditions)
            {
                FieldInfo conditionField = ReflectionUtility.GetField(target, condition);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    conditionValues.Add((bool)conditionField.GetValue(target));
                }

                MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, condition);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    conditionValues.Add((bool)conditionMethod.Invoke(target, null));
                }
            }

            if (conditionValues.Count > 0)
            {
                bool draw;
                if (showIfAttribute.ConditionOperator == ConditionOperator.And)
                {
                    draw = true;
                    foreach (var value in conditionValues)
                    {
                        draw = draw && value;
                    }
                }
                else
                {
                    draw = false;
                    foreach (var value in conditionValues)
                    {
                        draw = draw || value;
                    }
                }

                if (showIfAttribute.Reversed)
                {
                    draw = !draw;
                }

                return draw;
            }
            else
            {
                string warning = showIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);

                return true;
            }
        }
    }
}
