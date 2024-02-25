using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using System.Linq;
using SignalSystem;

public class ToothManager : MonoBehaviour
{
    public GameObject Tooth;

    public SO_TeethGrenration generationParameter;

    [Foldout("Materials")]
    public Material cleanMat;
    [Foldout("Materials")]
    public Material dirtyMat;
    [Foldout("Materials")]
    public Material tartarMat;
    [Foldout("Materials")]
    public Material decayMat;

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
        List<int> cellsState = new List<int>(teethCells.Count);
        List<bool> settedCells = new List<bool>(teethCells.Count);

        for (int i = 0; i < teethCells.Count; i++)
        {
            List<int> unChangedCells = new List<int>(settedCells.Where(x => x == false).ToList().Count);

            for (int j = 0; j < teethCells.Count; j++)
            {
                if (settedCells[j] == false)
                {
                    unChangedCells.Add(j);
                }
            }

            int cellIndex = unChangedCells[Random.Range(0, unChangedCells.Count)];

            settedCells[cellIndex] = true;

            if (cellsState.Where(cellsState => cellsState == (int)TeethState.Clean).Count() < generationParameter.minClean)
            {
                cellsState[cellIndex] = (int)TeethState.Clean;
            }
            else
            {
                int randomState = (int)Random.Range(0, 101);

                if (randomState < generationParameter.unCleanChance.Evaluate((float)cellsState.Where(cellsState => cellsState == (int)TeethState.Clean).Count() / generationParameter.numberOfPeices))
                {
                    cellsState[cellIndex] = (int)TeethState.Clean;
                }
                else
                {
                    //chooses a random anomalie depending if it has it or not
                    List<bool> activeAnomalies = new List<bool> { generationParameter.hasDirty, generationParameter.hasTartar, generationParameter.hasDecay };
                    int randomAnomalie = Random.Range(0, activeAnomalies.Where(x => x == true).Count());

                    if (randomAnomalie == 0)
                    {
                        cellsState[cellIndex] = DirtyGeneration();
                    }
                    else if (randomAnomalie == 1)
                    {
                        cellsState[cellIndex] = TartarGeneration();
                    }
                    else if (randomAnomalie == 2)
                    {
                        cellsState[cellIndex] = DecayGeneration();
                    }
                }
            }
        }

        for (int i = 0; i < teethCells.Count; i++)
        {
            teethCells[i].GetComponent<CellBehavior>().SwitchTeethState((TeethState)cellsState[i]);
        }
    }

    private int DirtyGeneration()
    {
        return Random.Range(0, 101) < generationParameter.dirtyChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Dirty : (int)TeethState.Clean;
    }

    private int TartarGeneration()
    {
        return Random.Range(0, 101) < generationParameter.tartarChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Tartar : (int)TeethState.Clean;
    }

    private int DecayGeneration()
    {
        return Random.Range(0, 101) < generationParameter.decayChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Decay : (int)TeethState.Clean;
    }
}
