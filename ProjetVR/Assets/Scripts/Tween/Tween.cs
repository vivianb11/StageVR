using System.Collections;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public AnimationCurve ease = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private IEnumerator ScaleCoroutine(Vector3 targetScale, float tweenTime)
    {
        Vector3 startScale = transform.localScale;
        float currentTime = 0.0f;

        while (currentTime < tweenTime)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, ease.Evaluate(currentTime / tweenTime));

            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void TweenScale(Vector3 targetScale, float tweenTime)
    {
        StartCoroutine(ScaleCoroutine(targetScale, tweenTime));
    }
}
