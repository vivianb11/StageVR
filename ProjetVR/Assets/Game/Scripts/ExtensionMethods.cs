using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
}