using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeethVarient_", menuName = "ScriptableObject/TeethVarients", order = 0)]
public class SO_TeethVarient : ScriptableObject
{
    [Header("Teeth Varients")]
    public Material healthyTeethMaterial;
    public Material dirtyTeethMaterial;
    public Material fracturedTeethMaterial;
    public Material tartarTeethMaterial;

    public TeethState RandomState()
    {
        return (TeethState)Random.Range(0, 5);
    }
}

public enum TeethState
{
    Clean,
    Dirty,
    Tartar,
    Fractured
}