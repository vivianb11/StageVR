using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;

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
    }
}
