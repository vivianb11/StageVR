using System.Collections;
using UnityEngine;

public class MainMode : MonoBehaviour
{
    public GameObject tooth;
    public GameObject toothSpawnPoint;

    public float startDelay;

    private void Start()
    {
        StartCoroutine(StartDelay(startDelay));
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject instance = Instantiate(tooth, toothSpawnPoint.transform);

        instance.transform.localScale = Vector3.zero;
        instance.transform.DoScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
}