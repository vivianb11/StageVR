using UnityEngine;

[ExecuteInEditMode]
public class MaterialChanger : MonoBehaviour
{
    private Renderer rend;

    public Material[] materials = new Material[1];

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Update()
    {
        if (rend is null)
        {
            rend = GetComponent<Renderer>();
        }
    

        if (Application.isEditor && !Application.isPlaying)
        {
            rend.material = materials[0];
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
