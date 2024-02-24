using System.Collections;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public void TweenScale(Vector3 targetScale, float tweenTime)
    {
        StartCoroutine(ScaleCoroutine(targetScale, tweenTime));
    }

    public void TweenMove(Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(MoveCoroutine(targetPosition, tweenTime));
    }

    private IEnumerator ScaleCoroutine(Vector3 targetScale, float tweenTime)
    {
        Vector3 startScale = transform.localScale;
        float currentTime = 0.0f;

        while (currentTime < tweenTime)
        {
            currentTime += Time.deltaTime;

            Vector3 lerpScale = Vector3.LerpUnclamped(startScale, targetScale, curve.Evaluate(currentTime / tweenTime));
            transform.localScale = lerpScale;

            yield return null;
        }

        transform.localScale = targetScale;
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, float tweenTime)
    {
        Vector3 startPosition = transform.position;
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

public enum Ease
{
    Linear, InOut, OutBack, InElastic
}