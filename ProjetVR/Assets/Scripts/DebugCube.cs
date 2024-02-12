using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCube : MonoBehaviour
{
    public void IncreaseSize(float size)
    {
        transform.localScale += new Vector3(size, size, size);
    }

    public void DecreaseSize(float size)
    {
        transform.localScale -= new Vector3(size, size, size);
    }
}
