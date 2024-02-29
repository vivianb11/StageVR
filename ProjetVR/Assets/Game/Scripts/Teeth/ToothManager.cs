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

    private Collider grabCollider;
    private Interactable grabIntractable;
    public SO_Signal brushSignal;
    public SO_Signal brossetteSignal;

    public Tween tweener;
    private List<CellBehavior> teethCells = new List<CellBehavior>();

    [SerializeField] int dirtyCellsCount;
    [SerializeField] int cleanedCell;

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
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == Tooth)
                continue;

            teethCells.Add(child.GetComponent<CellBehavior>());
        }

        ResetTeeth();

        Tooth.SetActive(false);

        for (int i = 0; i < teethCells.Count; i++)
        {
            teethCells[i].SwitchTeethState((TeethState)cellsState[i]);
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
        GetComponent<Collider>().enabled = true;

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
            cell.SwitchTeethState((TeethState)Random.Range(0, 4));
        }
    }

    private void EnableGrab()
    {
        grabCollider.enabled = true;

        foreach (Transform child in transform)
        {
            child.GetComponent<Collider>().enabled = false;
        }
    }

    private void DisableGrab()
    {
        grabCollider.enabled = false;
        
        if (grabIntractable.selected)
            grabIntractable.DeSelect();

        foreach (Transform child in transform)
        {
            child.GetComponent<Collider>().enabled = true;
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

        if (dirtyCellsCount == 0)
            EnableGrab();
    }

    private void ChooseAnomaly(int cellIndex, bool withClean = false)
    {
        List<TeethState> activeAnomalies = generationParameter.GetActives(withClean);

        TeethState teethState = activeAnomalies[Random.Range(0, activeAnomalies.Count)];

        switch (teethState)
        {
            case TeethState.Dirty:
                cellsState[cellIndex] = DirtyGeneration(cellIndex, withClean);
                dirtyCellsCount++;
                break;
            case TeethState.Tartar:
                cellsState[cellIndex] = TartarGeneration(cellIndex, withClean);
                dirtyCellsCount++;
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
