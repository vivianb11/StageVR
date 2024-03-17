using NaughtyAttributes;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Tween : MonoBehaviour
{
    private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [SerializeField]
    public TweenMontage[] tweenMontages = new TweenMontage[0];

    public bool autoPlayMontage;

    private Coroutine tweenCoroutine, tweenPropCoroutine, delayedEventCoroutine;
    private int playedMontageIndex;
    private Transform startTransform, endTransform;

    private void OnEnable()
    {
        if (autoPlayMontage)
            PlayMontages();
    }

#if UNITY_EDITOR
    [SerializeField]
    private int tweenIndex;

    [Button]
    private void PlayTween()
    {
        string key = tweenMontages[tweenIndex].name;

        PlayTween(key);
    }
#endif

    [Button]
    public void PlayMontages()
    {
        tweenCoroutine = StartCoroutine(TweenMontage(tweenMontages));
    }

    public TweenMontage PlayTween(string key)
    {
        TweenMontage montage = tweenMontages.Where(item => item.name == key).ToArray()[0];

        playedMontageIndex = Array.IndexOf(tweenMontages, montage);

        tweenCoroutine = StartCoroutine(TweenCoroutine(montage.tweenProperties, montage.speed));

        if (montage is null)
            Debug.LogError("TweenMontage not found :" + key);

        return montage;
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

    private float GetMontageDuration(TweenMontage montage)
    {
        float duration = 0f;

        foreach(var item in montage.tweenProperties)
        {
            duration += item.duration;
        }

        return duration / montage.speed;
    }

    private Transform GetMontageEndTransform(TweenMontage montage)
    {
        Transform endTransform = startTransform;

        foreach (var item in montage.tweenProperties)
        {
            switch (item.propertie)
            {
                case TweenProperty.Properties.POSITION:
                    endTransform.position = item.to;
                    break;
                case TweenProperty.Properties.LOCAL_POSITION:
                    endTransform.localPosition = item.to;
                    break;
                case TweenProperty.Properties.SCALE:
                    endTransform.localScale = item.to;
                    break;
                case TweenProperty.Properties.ROTATION:
                    endTransform.rotation = Quaternion.Euler(item.to);
                    break;
            }
        }

        return endTransform;
    }

    private void SetProperty(TweenProperty.Properties property, Vector3 value)
    {
        switch (property)
        {
            case TweenProperty.Properties.POSITION:
                transform.position = value;
                break;
            case TweenProperty.Properties.LOCAL_POSITION:
                transform.localPosition = value;
                break;
            case TweenProperty.Properties.SCALE:
                transform.localScale = value;
                break;
            case TweenProperty.Properties.ROTATION:
                transform.rotation = Quaternion.Euler(value);
                break;
        }
    }

    public IEnumerator TweenMontage(TweenMontage[] montages)
    {
        for (int i = 0; i < montages.Length; i++)
        {
            var montage = montages[i];

            if (i > 0 && montage.waitPreviousMontage)
                yield return new WaitForSeconds(GetMontageDuration(montages[i - 1]));

            playedMontageIndex = tweenMontages.ToList().IndexOf(montage);

            StartCoroutine(TweenCoroutine(montage.tweenProperties, montage.speed));
        }
    }

    public IEnumerator TweenCoroutine(TweenProperty[] tweenProperties, float speed)
    {
        startTransform = transform;
        endTransform = GetMontageEndTransform(tweenMontages[playedMontageIndex]);

        for (int i = 0; i < tweenProperties.Length; i++)
        {
            var tween = tweenProperties[i];

            if (i > 0 && tween.waitForPreviousTweenProperty)
                yield return new WaitForSeconds(tweenProperties[i - 1].duration / speed);

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

            tweenPropCoroutine = StartCoroutine(TweenPropertyCoroutine(tween.propertie, from, tween.to, tween.duration / speed, tween.curve));
            delayedEventCoroutine = StartCoroutine(InvokeDelay(tween.completed, tween.duration / speed));
        }

        yield return new WaitForSeconds(tweenProperties[tweenProperties.Length - 1].duration / speed);

        tweenMontages[playedMontageIndex].completed?.Invoke();
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

    private IEnumerator InvokeDelay(UnityEvent unityEvent, float delay)
    {
        yield return new WaitForSeconds(delay);

        unityEvent?.Invoke();
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

    public bool IsRunning()
    {
        return tweenCoroutine != null;
    }

    [Button]
    public bool SkipMontage()
    {
        if(tweenCoroutine is null)
            return false;

        StopCoroutine(tweenCoroutine);
        StopCoroutine(tweenPropCoroutine);
        StopCoroutine(delayedEventCoroutine);

        tweenCoroutine = null;
        tweenPropCoroutine = null;
        delayedEventCoroutine = null;

        tweenMontages[playedMontageIndex].completed?.Invoke();

        this.transform.position = endTransform.position;
        this.transform.localScale = endTransform.localScale;
        this.transform.rotation = endTransform.rotation;

        return true;
    }

    [Button]
    public bool StopMontage()
    {
        if (tweenCoroutine is null)
            return false;

        StopAllCoroutines();

        tweenCoroutine = null;
        tweenPropCoroutine = null;
        delayedEventCoroutine = null;

        return true;
    }

    [Button]
    public bool CancelMontage()
    {
        if(tweenCoroutine is null)
            return false;

        StopCoroutine(tweenCoroutine);
        StopCoroutine(tweenPropCoroutine);
        StopCoroutine(delayedEventCoroutine);

        tweenCoroutine = null;
        tweenPropCoroutine = null;
        delayedEventCoroutine = null;

        this.transform.position = startTransform.position;
        this.transform.localPosition = startTransform.localPosition;
        this.transform.localScale = startTransform.localScale;
        this.transform.rotation = startTransform.rotation;

        return true;
    }
}

[Serializable]
public struct TweenProperty
{
    public TweenProperty(TweenProperty.Properties propertie, bool waitForPreviousTweenProperty = false, bool useDynamicFrom = true, Vector3 from = new Vector3(), Vector3 to = new Vector3(), float duration = 1f, AnimationCurve animationCurve = default)
    {
        this.propertie = propertie;
        this.waitForPreviousTweenProperty = waitForPreviousTweenProperty;
        this.useDynamicFrom = useDynamicFrom;
        this.from = from;
        this.to = to;
        this.duration = duration;
        this.curve = new AnimationCurve();
        this.completed = new UnityEvent();
    }

    public enum Properties
    {
        POSITION, LOCAL_POSITION, SCALE, ROTATION
    }

    public Properties propertie;
    public bool waitForPreviousTweenProperty;

    [Space(20)]
    public bool useDynamicFrom;
    public Vector3 from;
    public Vector3 to;

    [Space(20)]
    public float duration;
    public AnimationCurve curve;

    [Space(20)]
    public UnityEvent completed;
}

[Serializable]
public class TweenMontage
{
    public TweenMontage(string name, bool waitPreviousMontage = false, float speed = 1f, TweenProperty[] tweenProperties = default)
    {
        this.name = name;
        this.waitPreviousMontage = waitPreviousMontage;
        this.speed = speed;
        this.tweenProperties = tweenProperties;
        this.completed = new UnityEvent();
    }

    [Space(10)]
    public string name;

    [Header("Montage Settings")]
    public bool waitPreviousMontage;

    public float speed = 1f;

    [Space(10)]
    public TweenProperty[] tweenProperties;
    
    [Space(10)]
    public UnityEvent completed;
}

public enum Ease
{
    Linear, InOut, OutBack, InElastic
}