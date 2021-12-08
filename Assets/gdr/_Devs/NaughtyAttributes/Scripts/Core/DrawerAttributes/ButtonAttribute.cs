using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : DrawerAttribute
    {
        public string Text { get; private set; }
        public float Space { get; private set; }
        public UnityEngine.Color m_color;

        public ButtonAttribute(string text = null)
        {
            this.Text = text;
            this.Space = 0f;
            m_color = UnityEngine.Color.white;
        }

        public ButtonAttribute(ColorEnum colorEnum, string text = null)
        {
            this.Text = text;
            this.Space = 0f;
            m_color = PickColor(colorEnum);
        }

        public ButtonAttribute(float space, string text = null)
        {
            this.Text = text;
            this.Space = space;
            m_color = UnityEngine.Color.white;
        }

        public ButtonAttribute(float space, ColorEnum colorEnum, string text = null)
        {
            this.Text = text;
            this.Space = space;
            m_color = PickColor(colorEnum);
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
}
