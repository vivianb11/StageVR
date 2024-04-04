using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public MeshRenderer rend;

    [Range(0f, 1f)]
    public float value;

    private void FixedUpdate()
    {
        rend.sharedMaterial.SetFloat("_force", value);
    }
}
