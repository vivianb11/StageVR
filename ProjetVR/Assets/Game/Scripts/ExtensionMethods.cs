using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethods
{
    #region Tween
    public static Tween DoScale(this Transform transform, Vector3 targetScale, float tweenTime)
    {
        Tween tween = GetTweenComponent(transform);
        tween.TweenScale(targetScale, tweenTime);

        return tween;
    }

    public static Tween DoMove(this Transform transform, Vector3 targetPosition, float tweenTime)
    {
        Tween tween = GetTweenComponent(transform);
        tween.TweenMove(targetPosition, tweenTime);

        return tween;
    }

    private static Tween GetTweenComponent(Transform transform)
    {
        Tween component;

        if (!transform.TryGetComponent(out component))
            component = transform.AddComponent<Tween>();

        return component;
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
        T item = list[Random.Range(0, list.Count)];

        return item;
    }
}