using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheUtil
{
    private static Dictionary<string, string> stringDictionary = new Dictionary<string, string>();
    private static Dictionary<string, int> intDictionary = new Dictionary<string, int>();
    private static Dictionary<string, double> doubleDictionary = new Dictionary<string, double>();
    private static Dictionary<string, float> floatDictionary = new Dictionary<string, float>();
    private static Dictionary<string, bool> boolDictionary = new Dictionary<string, bool>();

    public static string Get(string key, string defaultValue = "")
    {
        if (!stringDictionary.ContainsKey(key))
        {
            var value = PlayerPrefs.GetString(key, defaultValue);
            stringDictionary.Add(key, value);
        }
        return stringDictionary[key];
    }

    public static int Get(string key, int defaultValue = 0)
    {
        if (!intDictionary.ContainsKey(key))
        {
            var value = PlayerPrefs.GetInt(key, defaultValue);
            intDictionary.Add(key, value);
        }
        return intDictionary[key];
    }

    public static double Get(string key, double defaultFalue = 0d)
    {
        if (!doubleDictionary.ContainsKey(key))
        {
            var value = double.Parse(PlayerPrefs.GetString(key, defaultFalue.ToString()));
            doubleDictionary.Add(key, value);
        }
        return doubleDictionary[key];
    }

    public static float Get(string key, float defaultValue = .0f)
    {
        if (!floatDictionary.ContainsKey(key))
        {
            var value = PlayerPrefs.GetFloat(key, defaultValue);
            floatDictionary.Add(key, value);
        }
        return floatDictionary[key];
    }

    public static bool Get(string key, bool defaultValue = false)
    {
        if (!boolDictionary.ContainsKey(key))
        {
            var value = GetBool(key, defaultValue);
            boolDictionary.Add(key, value);
        }
        return boolDictionary[key];
    }

    public static void Set(string key, string newValue)
    {
        //if (key == "energy")
        //    Debug.Log($"Energy New Value = {newValue}");

        if (!stringDictionary.ContainsKey(key))
            stringDictionary.Add(key, newValue);
        else
            stringDictionary[key] = newValue;
        PlayerPrefs.SetString(key, newValue);
    }

    public static void Set(string key, int newValue)
    {
        if (!intDictionary.ContainsKey(key))
            intDictionary.Add(key, newValue);
        else
            intDictionary[key] = newValue;
        PlayerPrefs.SetInt(key, newValue);
    }

    public static void Set(string key, double newValue)
    {
        if (!doubleDictionary.ContainsKey(key))
            doubleDictionary.Add(key, newValue);
        else
            doubleDictionary[key] = newValue;
        PlayerPrefs.SetString(key, newValue.ToString());
    }

    public static void Set(string key, float newValue)
    {
        if (!floatDictionary.ContainsKey(key))
            floatDictionary.Add(key, newValue);
        else
            floatDictionary[key] = newValue;
        PlayerPrefs.SetFloat(key, newValue);
    }

    public static void Set(string key, bool newValue)
    {
        if (!boolDictionary.ContainsKey(key))
            boolDictionary.Add(key, newValue);
        else
            boolDictionary[key] = newValue;
        SetBool(key, newValue);
    }

    #region Player Prefs Bool

    private static bool GetBool(string key)
    {
        if (PlayerPrefs.GetInt(key) == 0)
            return false;
        else
            return true;
    }

    private static bool GetBool(string key, bool defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;
        if (PlayerPrefs.GetInt(key) == 0)
            return false;
        else
            return true;
    }

    private static void SetBool(string key, bool value)
    {
        if (value == true)
            PlayerPrefs.SetInt(key, 1);
        else
            PlayerPrefs.SetInt(key, 0);
    }

    #endregion
}
