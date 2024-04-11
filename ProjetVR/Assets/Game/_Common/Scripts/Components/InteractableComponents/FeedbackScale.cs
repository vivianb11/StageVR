using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FeedbackScale : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float scaleLerpSpeed = 0.15f;

    private const float scaleOffset = 1.2f;

    private Vector3 originalScale;

    public UnityEvent FBFinish;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        transform.localScale = originalScale;
    }

    private IEnumerator ScaleTimer(Vector3 targetScale)
    {
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleLerpSpeed);
            yield return null;
        }

        FBFinish?.Invoke();
    }

    public void ScaleIn()
    {
        StopAllCoroutines();

        Vector3 targetScale = originalScale * scaleOffset;
        StartCoroutine(ScaleTimer(targetScale));
    }

    public void ScaleOut()
    {
        StopAllCoroutines();

        StartCoroutine(ScaleTimer(originalScale));
    }

    public void ForceStop()
    {
        StopAllCoroutines();
        transform.localScale = originalScale;
    }
}
