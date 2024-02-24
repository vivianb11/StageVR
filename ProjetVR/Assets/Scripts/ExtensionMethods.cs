using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethods
{
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
}