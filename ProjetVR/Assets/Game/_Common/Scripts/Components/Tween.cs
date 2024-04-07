using NaughtyAttributes;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Tween : MonoBehaviour
{
    [SerializeField]
    public TweenMontage[] tweenMontages = new TweenMontage[0];

    public bool autoPlayMontage;

    private int playedMontageIndex;

    private void OnEnable()
    {
        if (autoPlayMontage)
            PlayMontages();
    }

#if UNITY_EDITOR
    [SerializeReference]
    private int tweenIndex;

    [Button]
    private void PlayTween()
    {
        if (tweenIndex < 0 || tweenIndex >= tweenMontages.Length)
        {
            Debug.LogError("Index Out Of Range");
            return;
        }

        string key = tweenMontages[tweenIndex].name;

        PlayTween(key);
    }
#endif

    [Button]
    public void PlayMontages()
    {
        StartCoroutine(MontagesCoroutine(tweenMontages));
    }

    public void PlayTween(string key)
    {
        TweenMontage tweenMontage = tweenMontages.Where(item => item.name == key).ToArray()[0];

        if (tweenMontage == null)
        {
            Debug.LogError("TweenMontage not found :" + key);
            return;
        }

        playedMontageIndex = tweenMontages.ToList().IndexOf(tweenMontage);

        StartCoroutine(PropertiesCoroutine(tweenMontage.tweenProperties, tweenMontage.speed));
    }

    public void PlayTween(string key, out TweenMontage tweenMontage)
    {
        tweenMontage = tweenMontages.Where(item => item.name == key).ToArray()[0];

        if (tweenMontage == null)
        {
            Debug.LogError("TweenMontage not found :" + key);
            return;
        }

        playedMontageIndex = tweenMontages.ToList().IndexOf(tweenMontage);

        StartCoroutine(PropertiesCoroutine(tweenMontage.tweenProperties, tweenMontage.speed));
    }

    public void TweenMove(Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(PropertyCoroutine(TweenProperty.Properties.POSITION, transform.position, targetPosition, tweenTime, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f)));
    }

    public void TweenLocalMove(Vector3 targetPosition, float tweenTime)
    {
        StartCoroutine(PropertyCoroutine(TweenProperty.Properties.LOCAL_POSITION, transform.position, targetPosition, tweenTime, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f)));
    }

    public void TweenScale(Vector3 targetScale, float tweenTime)
    {
        StartCoroutine(PropertyCoroutine(TweenProperty.Properties.SCALE, transform.position, targetScale, tweenTime, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f)));
    }

    public void TweenRotation(Vector3 targetRotation, float tweenTime)
    {
        StartCoroutine(PropertyCoroutine(TweenProperty.Properties.ROTATION, transform.position, targetRotation, tweenTime, AnimationCurve.EaseInOut(0f, 0f, 1f, 1f)));
    }

    public float GetMontageDuration(TweenMontage montage)
    {
        float duration = 0f;

        foreach(var item in montage.tweenProperties)
        {
            duration += item.duration;
        }

        return duration / montage.speed;
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

    public IEnumerator MontagesCoroutine(TweenMontage[] montages)
    {
        for (int i = 0; i < montages.Length; i++)
        {
            var montage = montages[i];

            if (i > 0 && montage.waitPreviousMontage)
                yield return new WaitForSeconds(GetMontageDuration(montages[i - 1]));

            playedMontageIndex = tweenMontages.ToList().IndexOf(montage);

            montage.started?.Invoke();
            StartCoroutine(InvokeDelay(montage.completed, GetMontageDuration(montages[i])));
            StartCoroutine(PropertiesCoroutine(montage.tweenProperties, montage.speed));
        }
    }

    public IEnumerator PropertiesCoroutine(TweenProperty[] tweenProperties, float speed)
    {
        for (int i = 0; i < tweenProperties.Length; i++)
        {
            var tweenPropertie = tweenProperties[i];

            if (i > 0 && tweenPropertie.waitForPreviousTweenProperty)
                yield return new WaitForSeconds(tweenProperties[i - 1].duration / speed);

            Vector3 from = tweenPropertie.from;
            Vector3 to = tweenPropertie.to;

            switch (tweenPropertie.propertie)
            {
                case TweenProperty.Properties.POSITION:
                    from = tweenPropertie.useDynamicFrom ? transform.position : tweenPropertie.from;
                    to = tweenPropertie.useGameObjectTo ? tweenPropertie.toObject.position : tweenPropertie.to;
                    break;
                case TweenProperty.Properties.LOCAL_POSITION:
                    from = tweenPropertie.useDynamicFrom ? transform.localPosition : tweenPropertie.from;
                    break;
                case TweenProperty.Properties.SCALE:
                    from = tweenPropertie.useDynamicFrom ? transform.localScale : tweenPropertie.from;
                    break;
                case TweenProperty.Properties.ROTATION:
                    from = tweenPropertie.useDynamicFrom ? transform.eulerAngles : tweenPropertie.from;
                    break;
            }

            StartCoroutine(PropertyCoroutine(tweenPropertie.propertie, from, to, tweenPropertie.duration / speed, tweenPropertie.curve));
            StartCoroutine(InvokeDelay(tweenPropertie.completed, tweenPropertie.duration / speed));
        }
    }

    private IEnumerator PropertyCoroutine(TweenProperty.Properties property, Vector3 startPosition, Vector3 targetPosition, float tweenTime, AnimationCurve ease)
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

    [Button]
    public void StopMontage()
    {
        StopAllCoroutines();
    }

    [Button]
    public void SkipMontage()
    {
        if (playedMontageIndex == -1)
            return;

        StopAllCoroutines();

        TweenMontage montage = tweenMontages[playedMontageIndex];
        playedMontageIndex = -1;

        foreach (TweenProperty property in montage.tweenProperties)
        {
            SetProperty(property.propertie, property.to);
        }

        montage.completed?.Invoke();
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
    public bool waitForPreviousTweenProperty;

    [Space(20)]
    public bool useDynamicFrom;
    public bool useGameObjectTo;
    public Vector3 from;
    public Vector3 to;
    public Transform toObject;

    [Space(20)]
    public float duration;
    public AnimationCurve curve;

    [Space(20)]
    public UnityEvent completed;
}

[Serializable]
public class TweenMontage
{
    [Space(10)]
    public string name;

    [Header("Montage Settings")]
    public bool waitPreviousMontage;

    public float speed = 1f;

    [Space(10)]
    public TweenProperty[] tweenProperties;

    [Space(10)]
    public UnityEvent started;
    public UnityEvent completed;
}