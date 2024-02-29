using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethProgress : MonoBehaviour
{
    public int toothCount = 1;
    public float animationDelay = 0.5f;

    public Transform toothPrefab;

    [Button]
    private void SpawnTeeth()
    {
        StopAllCoroutines();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        List<Tween> tweens = new List<Tween>();

        for (int i = 0; i < toothCount; i++)
        {
            Transform newTooth = Instantiate(toothPrefab, transform);
            newTooth.localScale = Vector3.zero;

            tweens.Add(newTooth.GetComponent<Tween>());
        }

        StartCoroutine(SpawnAnimation(tweens.ToArray(), animationDelay));
    }

    private IEnumerator SpawnAnimation(Tween[] tweener, float delay)
    {
        foreach (Tween tween in tweener)
        {
            tween.PlayTween("spawn");

            yield return new WaitForSeconds(delay);
        }
    }
}
