using System;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(CellManager))]
public class CellBehavior : MonoBehaviour
{
    private CellManager teethCellManager;

    private ToothManager toothGenerator;

    private MeshRenderer mR;

    public bool Cleaned
    {
        get
        {
            return teethCellManager.teethCleaned;
        }
        set
        {
            if (value)
            {
                teethCellManager.teethState = TeethState.Clean; 
                SetMaterials();

                activated = false;
            }
            else
                Debug.LogWarning("Cannot unclean a cell");
        }
    }

    private bool _localActivated;

    public bool activated
    {
        get
        {
            return _localActivated;
        }
        set
        {
            _localActivated = value;
            
            SetMaterials();
        }
    }

    private void SetMaterials()
    {
        switch (teethCellManager.teethState)
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

    private void Start()
    {
        teethCellManager = GetComponent<CellManager>();
        toothGenerator = transform.parent.GetComponent<ToothManager>();

        mR = GetComponent<MeshRenderer>();

        activated = true;
        activated = false;
    }

    public void FixedUpdate()
    {
        if (Cleaned || activated)
            return;

        switch(teethCellManager.teethState)
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

    [Button("Clean Cell")]
    private void CleanCell()
    {
        Cleaned = true;
    }

    private void DecayBehavior()
    {
        throw new NotImplementedException();
    }

    private void TartarBehavior()
    {
        throw new NotImplementedException();
    }

    private void DirtyBehavior()
    {
        throw new NotImplementedException();
    }
}
