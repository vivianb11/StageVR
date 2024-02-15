using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethCellManager : MonoBehaviour
{
    public SO_TeethVarient teethVarientData;

    private void Awake()
    {
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<Collider>());
    }
}
