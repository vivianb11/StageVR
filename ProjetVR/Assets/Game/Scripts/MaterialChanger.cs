using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialChanger : MonoBehaviour
{
    private Renderer rend;

    public Material[] materials = new Material[1];

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];
    }

    public void Update()
    {
        if (rend is null)
        {
            rend = GetComponent<Renderer>();
        }

        if (Application.isEditor)
        {
            materials[0] = rend.material;
        }
    }

    public void ChangeMaterial(int index)
    {
        if (index < materials.Length)
        {
            rend.material = materials[index];
        }
    }
}
