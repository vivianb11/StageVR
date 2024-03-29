using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCrosshair : MonoBehaviour
{
    public GameObject cross;

    // Start is called before the first frame update
    void Start()
    {
        cross = GameObject.FindGameObjectWithTag("Cursor");

        cross.GetComponent<CrosshairLerp>().lerpSpeed = 1;
    }
}
