using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethProgress : MonoBehaviour
{
    public int toothCount = 1;
    public float animationDelay = 0.5f;

    public Transform toothPrefab;

    [SerializeField] Material cleanMaterial;
    private int cleanedTeeth;

    public void SetFullTooth()
    {
        if (cleanedTeeth == transform.childCount)
            return;

        transform.GetChild(cleanedTeeth).GetComponent<MeshRenderer>().material = cleanMaterial;
        transform.GetChild(cleanedTeeth).GetComponent<Tween>().PlayTween("bump");

        cleanedTeeth++;
    }

    [Button]
    public void SpawnTeeth()
    {
        cleanedTeeth = 0;

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
