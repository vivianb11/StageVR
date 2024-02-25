using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public TweenProperty[] tweenProperty;

    public bool playOnStart;

    private void Start()
    {
        if (playOnStart)
            Play();
    }

    [Button]
    public void Play()
    {
        StartCoroutine(TweenCoroutine());
    }

    public IEnumerator TweenCoroutine()
    {
        for (int i = 0; i < tweenProperty.Length; i++)
        {
            var tween = tweenProperty[i];

            if (i > 0 && tween.waitForPrevious)
                yield return new WaitForSeconds(tweenProperty[i - 1].duration);

            Vector3 from = tween.from;

            switch (tween.propertie)
            {
                case TweenProperty.Properties.POSITION:
                    from = tween.useDynamicFrom ? transform.position : tween.from;
                    break;
                case TweenProperty.Properties.LOCAL_POSITION:
                    from = tween.useDynamicFrom ? transform.localPosition : tween.from;
                    break;
                case TweenProperty.Properties.SCALE:
                    from = tween.useDynamicFrom ? transform.localScale : tween.from;
                    break;
                case TweenProperty.Properties.ROTATION:
                    from = tween.useDynamicFrom ? transform.eulerAngles : tween.from;
                    break;
            }

            StartCoroutine(TweenPropertyCoroutine(tween.propertie, from, tween.to, tween.duration, tween.curve));
        }
    }

    public void TweenMove(Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(TweenPropertyCoroutine(TweenProperty.Properties.POSITION, transform.position, targetPosition, tweenTime, curve));
    }

    public void TweenLocalMove(Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(TweenPropertyCoroutine(TweenProperty.Properties.LOCAL_POSITION, transform.localPosition, targetPosition, tweenTime, curve));
    }

    public void TweenScale(Vector3 targetScale, float tweenTime)
    {
        StartCoroutine(TweenPropertyCoroutine(TweenProperty.Properties.SCALE, transform.localScale, targetScale, tweenTime, curve));
    }

    public void TweenRotation(Vector3 targetRotation, float tweenTime)
    {
        StartCoroutine(TweenPropertyCoroutine(TweenProperty.Properties.ROTATION, transform.eulerAngles, targetRotation, tweenTime, curve));
    }

    private void SetProperty(TweenProperty.Properties property, Vector3 value)
    {
        switch (property)
        {
            case global::TweenProperty.Properties.POSITION:
                transform.position = value;
                break;
            case global::TweenProperty.Properties.LOCAL_POSITION:
                transform.localPosition = value;
                break;
            case global::TweenProperty.Properties.SCALE:
                transform.localScale = value;
                break;
            case global::TweenProperty.Properties.ROTATION:
                transform.rotation = Quaternion.Euler(value);
                break;
        }
    }

    private IEnumerator TweenPropertyCoroutine(TweenProperty.Properties property, Vector3 startPosition, Vector3 targetPosition, float tweenTime, AnimationCurve ease)
    {
        SetProperty(property, startPosition);
        float currentTime = 0.0f;

        while (currentTime < tweenTime)
        {
            currentTime += Time.deltaTime;

            Vector3 lerpPosition = Vector3.LerpUnclamped(startPosition, targetPosition, ease.Evaluate(currentTime / tweenTime));
            SetProperty(property, lerpPosition);

            yield return null;
        }

        SetProperty(property, targetPosition);
    }

    public void SetEase(Ease ease)
    {
        curve.ClearKeys();

        switch (ease)
        {
            case Ease.Linear:
                curve.AddKey(0f, 0f);
                curve.AddKey(1f, 1f);
                break;
            case Ease.InOut:
                curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                break;
            case Ease.OutBack:
                for (float t = 0; t <= 1; t += 0.01f)
                {
                    curve.AddKey(new Keyframe(t, EaseOutBack(t)));
                }
                break;
            case Ease.InElastic:
                for (float t = 0; t <= 1; t += 0.01f)
                {
                    curve.AddKey(new Keyframe(t, EaseInElastic(t)));
                }
                break;
        }
    }

    private static float EaseOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * (float)Mathf.Pow(x - 1, 3) + c1 * (float)Mathf.Pow(x - 1, 2);
    }

    public static float EaseInElastic(float x)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return x == 0 ? 0 : x == 1 ? 1 : -(float)Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4);
    }

}

[Serializable]
public struct TweenProperty
{
    public enum Properties
    {
        POSITION, LOCAL_POSITION, SCALE, ROTATION
    }

    public Properties propertie;
    [Space(10)]
    public bool waitForPrevious;
    public bool useDynamicFrom;
    public Vector3 from;
    public Vector3 to;
    [Space(10)]
    public float duration;

    public AnimationCurve curve;
}

public enum Ease
{
    Linear, InOut, OutBack, InElastic
}