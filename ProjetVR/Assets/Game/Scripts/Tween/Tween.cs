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
        foreach (var tween in tweenProperty)
        {
            switch (tween.propertie)
            {
                case TweenProperty.Properties.POSITION:
                    Vector3 startPos = tween.useDynamicFrom ? transform.position : tween.from;
                    StartCoroutine(MoveCoroutine(startPos, tween.to, tween.duration, tween.curve));
                    break;
                case TweenProperty.Properties.LOCAL_POSITION:
                    Vector3 startLocalPos = tween.useDynamicFrom ? transform.localPosition : tween.from;
                    StartCoroutine(LocalMoveCoroutine(startLocalPos, tween.to, tween.duration, tween.curve));
                    break;
                case TweenProperty.Properties.SCALE:
                    Vector3 startScale = tween.useDynamicFrom ? transform.localScale : tween.from;
                    StartCoroutine(ScaleCoroutine(startScale, tween.to, tween.duration, tween.curve));
                    break;
                case TweenProperty.Properties.ROTATION:
                    Vector3 startRotation = tween.useDynamicFrom ? transform.eulerAngles : tween.from;
                    StartCoroutine(RotationCoroutine(startRotation, tween.to, tween.duration, tween.curve));
                    break;
            }
        }
    }

    public void TweenMove(Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(MoveCoroutine(transform.position, targetPosition, tweenTime, curve));
    }

    public void TweenLocalMove(Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(LocalMoveCoroutine(transform.localPosition, targetPosition, tweenTime, curve));
    }

    public void TweenScale(Vector3 targetScale, float tweenTime)
    {
        StartCoroutine(ScaleCoroutine(transform.localScale, targetScale, tweenTime, curve));
    }

    public void TweenRotation(Vector3 targetRotation, float tweenTime)
    {
        StartCoroutine(RotationCoroutine(transform.eulerAngles, targetRotation, tweenTime, curve));
    }

    private IEnumerator MoveCoroutine(Vector3 startPosition, Vector3 targetPosition, float tweenTime, AnimationCurve ease)
    {
        transform.position = startPosition;
        float currentTime = 0.0f;

        while (currentTime < tweenTime)
        {
            currentTime += Time.deltaTime;

            Vector3 lerpPosition = Vector3.LerpUnclamped(startPosition, targetPosition, curve.Evaluate(currentTime / tweenTime));
            transform.position = lerpPosition;

            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator LocalMoveCoroutine(Vector3 startPosition, Vector3 targetPosition, float tweenTime, AnimationCurve ease)
    {
        transform.localPosition = startPosition;
        float currentTime = 0.0f;

        while (currentTime < tweenTime)
        {
            currentTime += Time.deltaTime;

            Vector3 lerpPosition = Vector3.LerpUnclamped(startPosition, targetPosition, curve.Evaluate(currentTime / tweenTime));
            transform.localPosition = lerpPosition;

            yield return null;
        }

        transform.localPosition = targetPosition;
    }

    private IEnumerator ScaleCoroutine(Vector3 startScale, Vector3 targetScale, float tweenTime, AnimationCurve ease)
    {
        transform.localScale = startScale;
        float currentTime = 0.0f;

        while (currentTime < tweenTime)
        {
            currentTime += Time.deltaTime;

            Vector3 lerpScale = Vector3.LerpUnclamped(startScale, targetScale, ease.Evaluate(currentTime / tweenTime));
            transform.localScale = lerpScale;

            yield return null;
        }

        transform.localScale = targetScale;
    }

    private IEnumerator RotationCoroutine(Vector3 startRotation, Vector3 targetRotation, float tweenTime, AnimationCurve ease)
    {
        transform.rotation = Quaternion.Euler(startRotation);
        float currentTime = 0.0f;

        while (currentTime < tweenTime)
        {
            currentTime += Time.deltaTime;

            Vector3 lerpRotation = Vector3.LerpUnclamped(startRotation, targetRotation, curve.Evaluate(currentTime / tweenTime));
            transform.rotation = Quaternion.Euler(lerpRotation);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(targetRotation);
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