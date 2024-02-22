using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethods
{
    public static void DoScale(this Transform transform, Vector3 targetScale, float tweenTime)
    {
        Tween component;

        if (!transform.TryGetComponent(out component))
            component = transform.AddComponent<Tween>();

        component.TweenScale(targetScale, tweenTime);
    }
}