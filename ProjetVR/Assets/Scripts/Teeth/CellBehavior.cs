using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CellManager))]
public class CellBehavior : MonoBehaviour
{
    private CellManager teethCellManager;

    private ToothGenerator toothGenerator;

    public bool Cleaned
    {
        get
        {
            return teethCellManager.teethCleaned;
        }
        set
        {
            teethCellManager.teethState = TeethState.Clean;
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
            if (value)
            {
                SetMaterials();
            }
        }
    }

    private void SetMaterials()
    {
        switch (teethCellManager.teethState)
        {
            case TeethState.Dirty:
                this.GetComponent<MeshRenderer>().material = toothGenerator.dirtyMaterial;
                break;
            case TeethState.Tartar:
                this.GetComponent<MeshRenderer>().material = toothGenerator.tartarMaterial;
                break;
            case TeethState.Decay:
                this.GetComponent<MeshRenderer>().material = toothGenerator.decayMaterial;
                break;
        }
    }

    private void Awake()
    {
        _localActivated = false;
    }

    void Start()
    {
        teethCellManager = transform.GetComponent<CellManager>();
        toothGenerator = teethCellManager.toothGenerator;
    }

    public void FixedUpdate()
    {
        if (!_localActivated || Cleaned)
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
