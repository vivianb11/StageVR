using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using System.ComponentModel;

public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;

    public UnityEvent OnClean;

    private float maxToothPasteAmount;
    private float toothPasteAmount;

    private ToothManager toothGenerator;
    private MeshRenderer mR;

    private void Start()
    {
        toothGenerator = transform.parent.GetComponent<ToothManager>();
        mR = GetComponent<MeshRenderer>();
    }

    public void FixedUpdate()
    {
        if (teethState == TeethState.Clean)
            return;

        switch(teethState)
        {
            case TeethState.Dirty:
                DirtyBehavior();
                break;
            case TeethState.Tartar:
                TartarBehavior();
                break;
            case TeethState.Decay:
                DecayBehavior();
                break;
        }
    }

    private void SetMaterials()
    {
        switch (teethState)
        {
            case TeethState.Clean:
                mR.material = toothGenerator.cleanMaterial;
                break;
            case TeethState.Dirty:
                mR.material = toothGenerator.dirtyMaterial;
                break;
            case TeethState.Tartar:
                mR.material = toothGenerator.tartarMaterial;
                break;
            case TeethState.Decay:
                mR.material = toothGenerator.decayMaterial;
                break;
        }
    }

    public void SwitchTeethState(TeethState newTeethState)
    {
        teethState = newTeethState;

        SetMaterials();
    }

    [Button("Clean Cell")]
    private void CleanCell()
    {
        teethState = TeethState.Clean;

        OnClean.Invoke();
    }

    private void DecayBehavior()
    {
        Debug.LogWarning("DecayBehavior Not Implemented!");
    }

    private void TartarBehavior()
    {
        Debug.LogWarning("TartarBehavior Not Implemented!");
    }

    private void DirtyBehavior()
    {
        Debug.LogWarning("DirtyBehavior Not Implemented!");
    }
}
