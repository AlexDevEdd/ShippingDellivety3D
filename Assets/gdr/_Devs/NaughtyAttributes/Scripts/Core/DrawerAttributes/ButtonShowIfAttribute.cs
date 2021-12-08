using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonShowIfAttribute : DrawerAttribute
    {
        public string[] Conditions { get; private set; }
        public ConditionOperator ConditionOperator { get; private set; }
        public bool Reversed { get; protected set; }
        public UnityEngine.Color m_color;
        public float Space { get; private set; }

        public ButtonShowIfAttribute(string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
            m_color = UnityEngine.Color.black;
            this.Space = 0f;
        }

        public ButtonShowIfAttribute(float space, string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
            m_color = UnityEngine.Color.black;
            this.Space = space;
        }

        public ButtonShowIfAttribute(string condition, ColorEnum colorEnum)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
            m_color = PickColor(colorEnum);
            this.Space = 0f;
        }

        public ButtonShowIfAttribute(float space, string condition, ColorEnum colorEnum)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
            m_color = PickColor(colorEnum);
            this.Space = space;
        }

        public ButtonShowIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
            this.Space = 0f;
        }

        public ButtonShowIfAttribute(float space, ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
            this.Space = space;
        }

        public UnityEngine.Color PickColor(ColorEnum colorEnum)
        {
            switch (colorEnum)
            {
                case ColorEnum.black:
                    return UnityEngine.Color.black;
                case ColorEnum.white:
                    return UnityEngine.Color.white;
                case ColorEnum.red:
                    return UnityEngine.Color.red;
                case ColorEnum.magenta:
                    return UnityEngine.Color.magenta;
                case ColorEnum.blue:
                    return UnityEngine.Color.blue;
                case ColorEnum.green:
                    return UnityEngine.Color.green;
                case ColorEnum.gray:
                    return UnityEngine.Color.gray;
                case ColorEnum.yellow:
                    return UnityEngine.Color.yellow;
                case ColorEnum.cyan:
                    return UnityEngine.Color.cyan;
                default:
                    return UnityEngine.Color.black;
            }
        }
    }


    public enum ColorEnum
    {
        black = 0,
        white = 1,
        red = 2,
        blue = 4,
        green = 5,
        magenta = 6,
        gray = 7,
        yellow = 8,
        cyan = 10
    }
}
