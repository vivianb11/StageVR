using System.Collections;
using UnityEngine;

public class FeedbackScale : MonoBehaviour
{
    [SerializeField] float scaleOffset = 1.05f;
    [SerializeField] [Range(0f, 1f)] float scaleLerpSpeed = 0.15f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private IEnumerator ScaleTimer(Vector3 targetScale)
    {
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleLerpSpeed);
            yield return null;
        }
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
}
