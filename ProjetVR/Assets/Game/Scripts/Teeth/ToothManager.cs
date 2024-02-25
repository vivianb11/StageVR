using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using SignalSystem;

public class ToothManager : MonoBehaviour
{
    public GameObject Tooth;

    [Foldout("Materials")]
    public Material cleanMaterial;
    [Foldout("Materials")]
    public Material dirtyMaterial;
    [Foldout("Materials")]
    public Material tartarMaterial;
    [Foldout("Materials")]
    public Material decayMaterial;

    public SO_Signal brushSignal;

    private List<GameObject> teethCells = new List<GameObject>();

    public float CleanAmount
    {
        get
        {
            float cleandCells = teethCells.FindAll(x => x.GetComponent<CellBehavior>().teethState == TeethState.Clean).Count;
            float totalCells = teethCells.Count;

            return cleandCells / totalCells;
        }
        private set
        {
            Debug.LogWarning("CleanAmount is read only");
        }
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == Tooth)
                continue;

            teethCells.Add(child.gameObject);
        }

        SetupCells();

        Tooth.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetTeeth();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(CleanAmount);
        }
    }

    private void ResetTeeth()
    {
        SetupCells();
    }

    private void SetupCells()
    {
        foreach (var cell in teethCells)
        {
            var cellBehaviour = cell.GetComponent<CellBehavior>();
            cellBehaviour.SwitchTeethState((TeethState)Random.Range(0, Enum.GetValues(typeof (TeethState)).Length));
        }
    }
}
