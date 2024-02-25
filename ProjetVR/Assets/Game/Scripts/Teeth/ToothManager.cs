using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using System.Linq;
using SignalSystem;

public class ToothManager : MonoBehaviour
{
    public enum GenerationMode
    {
        Random,
        Custom
    }

    public GenerationMode generationMode = GenerationMode.Random;

    List<int> cellsState;
    private List<bool> settedCells;

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
    public SO_Signal brossetteSignal;

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

        ResetTeeth();

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
        switch (generationMode)
        {
            case GenerationMode.Random:
                RandomCellSetup();
                break;
            case GenerationMode.Custom:
                SetupCells();
                break;
        }
    }

    private void RandomCellSetup()
    {
        foreach (GameObject cell in teethCells)
        {
            cell.GetComponent<CellBehavior>().SwitchTeethState((TeethState)Random.Range(0, 4));
        }
    }

    private void SetupCells()
    {
        cellsState = new List<int>(teethCells.Count);
        settedCells = new List<bool>(teethCells.Count);

        for (int i = 0; i < teethCells.Count; i++)
        {
            cellsState.Add(0);
            settedCells.Add(false);
        }

        for (int i = 0; i < teethCells.Count; i++)
        {
            List<int> unChangedCells = new List<int>(settedCells.Where(x => x == false).Count());

            for (int j = 0; j < settedCells.Count; j++)
            {
                if (settedCells[j] == false)
                {
                    unChangedCells.Add(j);
                }
            }

            int y = Random.Range(0, unChangedCells.Count);

            int cellIndex = unChangedCells[y];

            settedCells[cellIndex] = true;

            if (cellsState.Where(cellsState => cellsState == (int)TeethState.Clean).Count() < generationParameter.minMaxClean.x)
            {
                cellsState[cellIndex] = (int)TeethState.Clean;
            }
            else if (cellsState.Where(cellsState => cellsState == (int)TeethState.Clean).Count() > generationParameter.minMaxClean.y)
            {
                ChooseAnomaly(cellIndex, true);
            }
            else
            {
                ChooseAnomaly(cellIndex);
            }
        }

        for (int i = 0; i < teethCells.Count; i++)
        {
            teethCells[i].GetComponent<CellBehavior>().SwitchTeethState((TeethState)cellsState[i]);
        }
    }

    private void ChooseAnomaly(int cellIndex)
    {
        List<TeethState> activeAnomalies = generationParameter.GetActives(false);

        TeethState teethState = activeAnomalies[Random.Range(0, activeAnomalies.Count)];

        switch (teethState)
        {
            case TeethState.Dirty:
                cellsState[cellIndex] = DirtyGeneration(cellIndex, false);
                break;
            case TeethState.Tartar:
                cellsState[cellIndex] = TartarGeneration(cellIndex, false);
                break;
            case TeethState.Decay:
                cellsState[cellIndex] = DecayGeneration(cellIndex, false);
                break;
        }
    }

    private void ChooseAnomaly(int cellIndex, bool withClean)
    {
        List<TeethState> activeAnomalies = generationParameter.GetActives(withClean);

        TeethState teethState = activeAnomalies[Random.Range(0, activeAnomalies.Count)];

        switch (teethState)
        {
            case TeethState.Dirty:
                cellsState[cellIndex] = DirtyGeneration(cellIndex, withClean);
                break;
            case TeethState.Tartar:
                cellsState[cellIndex] = TartarGeneration(cellIndex, withClean);
                break;
            case TeethState.Decay:
                cellsState[cellIndex] = DecayGeneration(cellIndex, withClean);
                break;
            case TeethState.Clean:
                cellsState[cellIndex] = CleanGeneration(cellIndex, withClean);
                break;
        }
    }

    private int DirtyGeneration(int cellIndex, bool withClean)
    {
        int x = Random.Range(0, 1) < generationParameter.dirtyChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Dirty : (int)TeethState.Clean;

        if (x == (int)TeethState.Clean)
        {
            CleanGeneration(cellIndex, withClean);
        }
        return x;
    }

    private int TartarGeneration(int cellIndex, bool withClean)
    {
        int x = Random.Range(0, 1) < generationParameter.tartarChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Tartar : (int)TeethState.Clean;

        if (x == (int)TeethState.Clean)
        {
            CleanGeneration(cellIndex, withClean);
        }
        return x;
    }

    private int DecayGeneration(int cellIndex, bool WithClean)
    {
        int x = Random.Range(0, 1) < generationParameter.decayChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Decay : (int)TeethState.Clean;

        if (x == (int)TeethState.Clean)
        {
            CleanGeneration(cellIndex, WithClean);
        }
        return x;
    }

    private int CleanGeneration(int cellIndex, bool withClean)
    {
        int x = Random.Range(0, 1) < generationParameter.weightClean ? (int)TeethState.Clean : (int)TeethState.Dirty;

        if (x != (int)TeethState.Clean)
        {
            DirtyGeneration(cellIndex, withClean);
        }
        return x;
    }
}
