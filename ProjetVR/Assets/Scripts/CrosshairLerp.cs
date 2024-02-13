using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairLerp : MonoBehaviour
{
    public float lerpSpeed;

    private Transform parent;

    private void Start()
    {
        parent = transform.parent;

        transform.parent = null;
    }

    private void Update()
    {
        transform.position = parent.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, parent.rotation, lerpSpeed);
    }
}
