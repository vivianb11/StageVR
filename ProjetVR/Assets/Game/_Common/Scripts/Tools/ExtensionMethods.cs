using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ExtensionMethods
{
    #region TextMesh

    public static int GetNumberOfLines(this TextMesh textMesh)
    {
        return textMesh.text.Split('\n').Length;
    }

    public static string[] GetLines(this TextMesh textMesh)
    {
        return textMesh.text.Split('\n');
    }

    public static string GetLongestLine(this TextMesh textMesh)
    {
        string[] lines = textMesh.GetLines();
        int longestLineIndex = 0;

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length > lines[longestLineIndex].Length)
                longestLineIndex = i;
        }

        return lines[longestLineIndex];
    }

    public static string GetShortestLine(this TextMesh textMesh)
    {
        string[] lines = textMesh.GetLines();
        int shortestLineIndex = 0;

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length < lines[shortestLineIndex].Length)
                shortestLineIndex = i;
        }

        return lines[shortestLineIndex];
    }

    #endregion

    #region List

    public static T DebugList<T>(this List<T> list)
    {
        string debugString = "";

        for (int i = 0; i < list.Count; i++)
        {
            debugString += list[i].ToString() + ", ";
        }

        Debug.Log(debugString);

        return default;
    }

    public static T DebugList<T>(this List<T> list, string message)
    {
        string debugString = message + " ";

        for (int i = 0; i < list.Count; i++)
        {
            debugString += list[i].ToString() + ", ";
        }

        Debug.Log(debugString);

        return default;
    }

    #endregion

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T PickRandom<T>(this IList<T> list)
    {
        if (list.Count == 0)
            return default;

        T item = list[Random.Range(0, list.Count)];

        return item;
    }

    #region String
    public static string SetColor(this string text, Color color)
    {
        string output;
        output = string.Format("<color={0}>{1}</color>", color.ToHex(), text);
        return output;
    }

    public static string ToHex(this Color c)
    {
        return string.Format("#{0:x2}{1:x2}{2:x2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255f);
    }

    public static int SearchPatternBegin(this string text, int startIndex, string pattern)
    {
        int beginPattern = -1;

        for (int i = startIndex; i < text.Length; i++)
        {
            if (text[i] == pattern[0])
            {
                string tmp = "";

                for (int j = i; j < i + pattern.Length; j++)
                {
                    tmp += text[j];
                }

                if (tmp == pattern)
                {
                    beginPattern = i;
                    break;
                }

            }
        }

        return beginPattern;
    }

    public static int SearchPatternEnd(this string text, int startIndex, string pattern)
    {
        int beginPattern = -1;

        for (int i = startIndex; i < text.Length; i++)
        {
            if (text[i] == pattern[0])
            {
                string tmp = "";

                for (int j = i; j < i + pattern.Length; j++)
                {
                    tmp += text[j];
                }

                if (tmp == pattern)
                {
                    beginPattern = i + pattern.Length;
                    break;
                }

            }
        }

        return beginPattern;
    }

    public static string[] SplitPattern(this string text, string startSlice, string endSlice)
    {
        List<string> result = new List<string>();

        while (text.Contains(startSlice) && text.Contains(endSlice))
        {
            string newSlice = "";

            for (int j = text.SearchPatternEnd(0, startSlice); j < text.SearchPatternBegin(0, endSlice); j++)
            {
                newSlice += text[j];
            }

            result.Add(newSlice);

            text = text.Remove(text.SearchPatternEnd(0, startSlice), text.SearchPatternBegin(0, endSlice));
        }

        return result.ToArray();
    }
    #endregion
}