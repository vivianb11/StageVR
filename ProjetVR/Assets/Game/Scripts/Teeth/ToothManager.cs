using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using System.Linq;
using SignalSystem;
using UnityEngine.Events;

public class ToothManager : MonoBehaviour
{
    public enum GenerationMode
    {
        Random,
        Custom
    }

    public GenerationMode generationMode = GenerationMode.Random;

    private List<int> cellsState;
    private List<bool> settedCells;

    public GameObject Tooth;
    public List<CellBehavior> teethCells = new List<CellBehavior>();

    public SO_TeethGrenration generationParameter;

    [Foldout("Materials")]
    public Material cleanMat;
    [Foldout("Materials")]
    public Material dirtyMat;
    [Foldout("Materials")]
    public Material tartarMat;
    [Foldout("Materials")]
    public Material decayMat;

    private Collider grabCollider;
    private Interactable grabIntractable;
    public SO_Signal brushSignal;
    public SO_Signal brossetteSignal;

    private Tween tweener;

    int dirtyCellsCount;
    int cleanedCell;

    public GameObject smellVFX;
    bool smells;

    public bool Smells
    {
        set
        {
            if(value)
                smellVFX.SetActive(true);
            else
                smellVFX.SetActive(false);

            smells = value;
        }
        get
        {
            return smells;
        }
    }

    [Foldout("Events")]
    public UnityEvent OnTeethCleaned;
    [Foldout("Events")]
    [InfoBox("Returns a float")]
    public UnityEvent<float> OnCleanAmountChange;
    
    public float CleanAmount
    {
        get
        {
            float cleandCells = teethCells.FindAll(x => x.GetComponent<CellBehavior>().teethState == TeethState.Clean).Count;
            float totalCells = teethCells.Count;

            if (smells)
                cleandCells--;

            return cleandCells / totalCells;
        }
        private set
        {
            Debug.LogWarning("CleanAmount is read only");
        }
    }

    private void Awake()
    {
        grabCollider = GetComponent<Collider>();
        grabIntractable = GetComponent<Interactable>();
        tweener = GetComponent<Tween>();
    }

    void Start()
    {
        ResetTeeth();

        Tooth.SetActive(false);

        for (int i = 0; i < teethCells.Count; i++)
        {
            teethCells[i].OnClean.AddListener(OnCellCleaned);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetTeeth();
        }
    }

    [Button]
    public void CleanTeeth()
    {
        grabCollider.enabled = true;

        foreach (var cell in teethCells)
        {
            cell.CleanCell();
            cell.gameObject.SetActive(false);
        }

        DisableGrab();
        Tooth.SetActive(true);

        OnTeethCleaned?.Invoke();

        tweener.PlayTween("despawn");
    }

    public void ResetTeeth()
    {
        tweener.PlayTween("spawn");
        transform.localPosition = Vector3.zero;
        dirtyCellsCount = 0;
        cleanedCell = 0;
        smells = false;

        DisableGrab();

        foreach (var cell in teethCells)
        {
            cell.gameObject.SetActive(true);
        }

        Tooth.SetActive(false);

        switch (generationMode)
        {
            case GenerationMode.Random:
                RandomCellSetup();
                break;
            case GenerationMode.Custom:
                SetupCells();
                break;
        }

        OnCleanAmountChange?.Invoke(CleanAmount);
    }

    private void RandomCellSetup()
    {
        foreach (CellBehavior cell in teethCells)
        {
            TeethState teethState = (TeethState)Random.Range(0, 4);
            dirtyCellsCount += teethState != TeethState.Clean && teethState != TeethState.Decay ? 1 : 0;
            cell.SwitchTeethState(teethState);
        }

        smells = Random.Range(0, 1) < 0.5f ? true : false;
    }

    private void EnableGrab()
    {
        grabCollider.enabled = true;

        foreach (CellBehavior cell in teethCells)
        {
            cell.gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void DisableGrab()
    {
        grabCollider.enabled = false;
        
        if (grabIntractable.selected)
            grabIntractable.DeSelect();

        foreach (CellBehavior cell in teethCells)
        {
            cell.gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    private void OnCellCleaned()
    {
        cleanedCell++;

        if (cleanedCell == dirtyCellsCount)
        {
            foreach (var cell in teethCells)
            {
                if (cell.teethState != TeethState.Clean)
                {
                    EnableGrab();

                    return;
                }
            }

            tweener.PlayTween("despawn");
        }

        OnCleanAmountChange?.Invoke(CleanAmount);
    }

    private void SetupCells()
    {
        Smells = Random.Range(0f,1f) < generationParameter.smellSpawnChance;

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

            if (cellsState.Where(cellsState => cellsState == (int)TeethState.Clean).Count() <= generationParameter.minMaxClean.x)
            {
                cellsState[cellIndex] = (int)TeethState.Clean;
            }
            else if (cellsState.Where(cellsState => cellsState == (int)TeethState.Clean).Count() > generationParameter.minMaxClean.y)
            {
                ChooseAnomaly(cellIndex);
            }
            else
            {
                ChooseAnomaly(cellIndex, true);
            }
        }

        for (int i = 0; i < teethCells.Count; i++)
        {
            dirtyCellsCount += (TeethState)cellsState[i] != TeethState.Clean && (TeethState)cellsState[i] != TeethState.Decay ? 1 : 0;
            teethCells[i].SwitchTeethState((TeethState)cellsState[i]);
        }

        if (dirtyCellsCount == 0)
            EnableGrab();
    }

    private void ChooseAnomaly(int cellIndex, bool withClean = false)
    {
        List<TeethState> activeAnomalies = generationParameter.GetActives(withClean);

        int maxwheight = 0;

        foreach (var anomaly in activeAnomalies)
        {
            if (anomaly == TeethState.Clean)
            {
                maxwheight += generationParameter.weightClean;
            }
            else if (anomaly == TeethState.Dirty)
            {
                maxwheight += generationParameter.weightDirty;
            }
            else if (anomaly == TeethState.Tartar)
            {
                maxwheight += generationParameter.weightTartar;
            }
            else if (anomaly == TeethState.Decay)
            {
                maxwheight += generationParameter.weightDecay;
            }
        }

        int random = Random.Range(0, maxwheight);

        foreach (var anomaly in activeAnomalies)
        {
            if (random < generationParameter.weightClean)
            {
                cellsState[cellIndex] = (int)TeethState.Clean;
                return;
            }
            else if (random < generationParameter.weightClean + generationParameter.weightDirty)
            {
                cellsState[cellIndex] = DirtyGeneration(cellIndex, withClean);
                return;
            }
            else if (random < generationParameter.weightClean + generationParameter.weightDirty + generationParameter.weightTartar)
            {
                cellsState[cellIndex] = TartarGeneration(cellIndex, withClean);
                return;
            }
            else if (random < generationParameter.weightClean + generationParameter.weightDirty + generationParameter.weightTartar + generationParameter.weightDecay)
            {
                cellsState[cellIndex] = DecayGeneration(cellIndex, withClean);
                return;
            }
        }
    }

    private int DirtyGeneration(int cellIndex, bool withClean)
    {
        int x = Random.Range(0, 1) < generationParameter.dirtyChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Dirty : (int)TeethState.Clean;

        if (x == (int)TeethState.Clean && !withClean)
        {
            ChooseAnomaly(cellIndex, withClean);
        }
        return x;
    }

    private int TartarGeneration(int cellIndex, bool withClean)
    {
        int x = Random.Range(0, 1) < generationParameter.tartarChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Tartar : (int)TeethState.Clean;

        if (x == (int)TeethState.Clean && !withClean)
        {
            ChooseAnomaly(cellIndex, withClean);
        }
        return x;
    }

    private int DecayGeneration(int cellIndex, bool withClean)
    {
        int x = Random.Range(0, 1) < generationParameter.decayChance.Evaluate(Random.Range(0, 1)) ? (int)TeethState.Decay : (int)TeethState.Clean;

        if (x == (int)TeethState.Clean && !withClean)
        {
            ChooseAnomaly(cellIndex, withClean);
        }
        return x;
    }

    private int CleanGeneration(int cellIndex, bool withClean)
    {
        int x = Random.Range(0, 1) < generationParameter.weightClean ? (int)TeethState.Clean : (int)TeethState.Dirty;

        if (x != (int)TeethState.Clean)
        {
            ChooseAnomaly(cellIndex, withClean);
        }
        return x;
    }

    public void SetCellState(int index, TeethState teethState)
    {
        teethCells[index].SwitchTeethState(teethState);
    }

    public void SetCellState(int index, int teethState)
    {
        teethCells[index].SwitchTeethState((TeethState)teethState);
    }

    public void SetCellState(CellBehavior cell, int teethState)
    {
        cell.SwitchTeethState((TeethState)teethState);
    }

    public void SetCellState(CellBehavior cell, TeethState teethState)
    {
        cell.SwitchTeethState(teethState);
    }
}
