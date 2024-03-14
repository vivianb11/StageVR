using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialChanger : MonoBehaviour
{
    private Renderer rend;

    public Material[] materials;

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

        if (Application.isEditor && materials[0] != rend.material)
        {
            rend.sharedMaterial = materials[0];
        }
    }

    public void ChangeMaterial(int index)
    {
        if (index < materials.Length)
        {
            rend.sharedMaterial = materials[index];
        }
    }
}
