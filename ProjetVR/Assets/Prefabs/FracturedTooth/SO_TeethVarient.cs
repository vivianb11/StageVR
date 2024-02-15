using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeethVarient_", menuName = "ScriptableObject/TeethVarients", order = 0)]
public class SO_TeethVarient : ScriptableObject
{
    public enum ExperimenalMode
    {
        WithGameobject,
        WithMaterial
    }

    [HideInInspector]
    public TeethState teethState;

    [Space(10)]
    public ExperimenalMode experimenalMode;

    [Header("Base Teeth")]
    public GameObject healthyTeeth;

    [Header("Teeth Varients")]
    [ShowIf("experimenalMode", ExperimenalMode.WithGameobject)]
    public GameObject dirtyTeeth;
    [ShowIf("experimenalMode", ExperimenalMode.WithGameobject)]
    public GameObject fracturedTeeth;
    [ShowIf("experimenalMode", ExperimenalMode.WithGameobject)]
    public GameObject tartarTeeth;

    [Header("Teeth Varients")]
    [ShowIf("experimenalMode", ExperimenalMode.WithMaterial)]
    public Material healthyTeethMaterial;
    [ShowIf("experimenalMode", ExperimenalMode.WithMaterial)]
    public Material dirtyTeethMaterial;
    [ShowIf("experimenalMode", ExperimenalMode.WithMaterial)]
    public Material fracturedTeethMaterial;
    [ShowIf("experimenalMode", ExperimenalMode.WithMaterial)]
    public Material tartarTeethMaterial;

    public GameObject GetTeeth()
    {
        switch (teethState)
        {
            case TeethState.Clean:
                
                if (experimenalMode == ExperimenalMode.WithGameobject)
                    return healthyTeeth;
                else
                {
                    healthyTeeth.GetComponent<MeshRenderer>().material = healthyTeethMaterial;
                    return healthyTeeth;
                }

            case TeethState.Dirty:
                
                if (experimenalMode == ExperimenalMode.WithGameobject)
                    return dirtyTeeth;
                else
                {
                    healthyTeeth.GetComponent<MeshRenderer>().material = dirtyTeethMaterial;
                    return healthyTeeth;
                }
            
            case TeethState.Fractured:
                
                if (experimenalMode == ExperimenalMode.WithGameobject)
                        return fracturedTeeth;
                else
                {
                    healthyTeeth.GetComponent<MeshRenderer>().material = fracturedTeethMaterial;
                    return healthyTeeth;
                }
            
            case TeethState.Tartar:
                
                if (experimenalMode == ExperimenalMode.WithGameobject)
                     return tartarTeeth;
                else
                {
                    healthyTeeth.GetComponent<MeshRenderer>().material = tartarTeethMaterial;
                    return healthyTeeth;
                }
            
            default:
                return healthyTeeth;
        }
    }

    public void RandomState()
    {
        teethState = (TeethState)Random.Range(0, 4);
    }
}

public enum TeethState
{
    Clean,
    Dirty,
    Tartar,
    Fractured
}