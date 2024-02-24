using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;

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

    private List<GameObject> teethCells = new List<GameObject>();

    public float CleanAmount
    {
        get
        {
            float cleandCells = teethCells.FindAll(x => x.GetComponent<CellManager>().teethState == TeethState.Clean).Count;
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

    private void SetupCells()
    {
        foreach (var cell in teethCells)
        {
            var cellManager = cell.GetComponent<CellManager>();
            cellManager.toothGenerator = this;
            cellManager.teethState = (TeethState)Random.Range(0, Enum.GetValues(typeof (TeethState)).Length);
            cellManager.GetComponent<CellBehavior>().activated = true;
        }
    }

    private void ResetTeeth()
    {
        SetupCells();
    }
}
