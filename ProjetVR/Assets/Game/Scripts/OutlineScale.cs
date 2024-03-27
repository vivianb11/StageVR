using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Outline))]
public class OutlineScale : MonoBehaviour
{
    [SerializeField] float widthOffset;
    [SerializeField] [Range(0f, 1f)] float scaleLerpSpeed;
    private float originalWidth;

    private Outline outlineEffect;

    private void Start()
    {
        outlineEffect = GetComponent<Outline>();
    }

    private IEnumerator ScaleTimer(float targetWidth)
    {
        while (outlineEffect.OutlineWidth != targetWidth)
        {
            outlineEffect.OutlineWidth = Mathf.Lerp(outlineEffect.OutlineWidth, targetWidth, scaleLerpSpeed);
            yield return null;
        }
    }

    public void ScaleIn()
    {
        StopAllCoroutines();


        float targetWidth = originalWidth * widthOffset;
        StartCoroutine(ScaleTimer(targetWidth));
    }

    public void ScaleOut()
    {
        StopAllCoroutines();

        StartCoroutine(ScaleTimer(originalWidth));
    }
}
