using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFeedback : MonoBehaviour
{
    [SerializeField] float scaleOffset = 1.0f;
    [SerializeField] [Range(0f, 1f)] float scaleLerpSpeed;

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

        Vector3 targetScale = transform.localScale + new Vector3(scaleOffset, scaleOffset, scaleOffset);
        StartCoroutine(ScaleTimer(targetScale));
    }

    public void ScaleOut()
    {
        StopAllCoroutines();

        Vector3 targetScale = transform.localScale - new Vector3(scaleOffset, scaleOffset, scaleOffset);
        StartCoroutine(ScaleTimer(targetScale));
    }
}
