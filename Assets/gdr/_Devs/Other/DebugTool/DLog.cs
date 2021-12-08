using System.Runtime.CompilerServices;
using UnityEngine;

// V1.4.4
public static class DLog
{
    [MethodImpl(256)]
    public static void D()
    {
        string colorText = TryGetColor();
        var caller = GetCaller();

        Debug.Log($"<color={colorText}>{caller}</color>");
    }

    [MethodImpl(256)]
    public static void D(string text)
    {
        string colorText = TryGetColor();
        var caller = GetCaller();

        Debug.Log(GetString(text, colorText, caller));
    }

    [MethodImpl(256)]
    public static void D(Color color)
    {
        string colorText = GetColor(color);

        var caller = GetCaller();
        Debug.Log($"<color={colorText}>{caller}</color>");
    }

    [MethodImpl(256)]
    public static void D(string text, Color color)
    {
        string colorText = GetColor(color);
        var caller = GetCaller();

        Debug.Log(GetString(text, colorText, caller));
    }

    [MethodImpl(256)]
    private static string TryGetColor()
    {
        string colorText = $"#ffffff";
        var refl = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ReflectedType;
        try
        {
            var debugColor = refl.GetField("debugColor");
            Color c = (Color)debugColor.GetValue(refl);
            colorText = $"#{(int)(c.r * 255f):X2}{(int)(c.g * 255f):X2}{(int)(c.b * 255f):X2}";
        }
        catch (System.Exception e)
        {
            // Skipped
        }
        return colorText;
    }

    [MethodImpl(256)]
    private static string GetCaller()
    {
        var refl = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ReflectedType;
        var caller = refl.Name;
        return caller;
    }

    private static string GetColor(Color color)
    {
        return $"#{(int)(color.r * 255f):X2}{(int)(color.g * 255f):X2}{(int)(color.b * 255f):X2}";
    }

    private static string GetString(string text, string colorText, string caller)
    {
        var isEmptyString = string.IsNullOrEmpty(text);
        if (isEmptyString)
            return $"<color={colorText}>{caller}</color>";
        else
            return $"<color={colorText}>{caller}: {text}</color>";
    }
}