using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using System.Linq;
using SignalSystem;
using UnityEngine.Events;
using System.Threading.Tasks;

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
    public GameObject smellVFX;

    private bool smells;

    public int maxSmellAmount = 10;
    private int smellAmount = 10;

    [Foldout("Events")]
    public UnityEvent OnTeethCleaned;
    [Foldout("Events")]
    [InfoBox("Returns a float")]
    public UnityEvent<float> OnCleanAmountChange;

    private void Awake()
    {
        grabCollider = GetComponent<Collider>();
        grabIntractable = GetComponent<Interactable>();
        tweener = GetComponent<Tween>();

        smellAmount = maxSmellAmount;
    }

    void Start()
    {
        ResetTooth();

        Tooth.SetActive(false);

        for (int i = 0; i < teethCells.Count; i++)
        {
            teethCells[i].OnClean.AddListener(OnCellCleaned);
        }

        OnCleanAmountChange?.Invoke(GetToothCleanPercent());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetTooth();
        }
    }

    public void SetSmell(bool value)
    {
        smells = value;

        smellAmount = maxSmellAmount;

        if (value)
            smellVFX.SetActive(true);
        else
            smellVFX.SetActive(false);
    }

    [Button]
    public void CleanSmell()
    {
        SetSmell(false);

        OnCleanAmountChange?.Invoke(GetToothCleanPercent());

        CheckIfToothCleaned();
    }

    public void RemoveSmellAmount()
    {
        if (smells)
            return;

        smellAmount -= 1;

        if (smellAmount <= 0)
            CleanSmell();
    }

    public float GetToothCleanPercent()
    {
        float cleandCells = teethCells.FindAll(x => x.teethState == TeethState.Clean).Count;
        float totalCells = teethCells.Count;

        return (cleandCells / totalCells) - (smells ? 0.1f : 0f);
    }

    public bool IsToothCleaned()
    {
        return GetToothCleanPercent() == 1;
    }

    public bool OnlyDecayRemaining()
    {
        if (smells)
            return false;

        foreach (var teethCell in teethCells)
        {
            if (teethCell.teethState != TeethState.Decay && teethCell.teethState != TeethState.Clean)
                return false;
        }

        return true;
    }

    [Button]
    public void CleanTooth()
    {
        foreach (var cell in teethCells)
        {
            cell.gameObject.SetActive(false);
        }

        Tooth.SetActive(true);
        tweener.PlayTween("despawn");

        DisableGrab();

        OnCleanAmountChange?.Invoke(1);
        OnTeethCleaned?.Invoke();
    }

    public void ResetTooth()
    {
        transform.localPosition = Vector3.zero;
        SetSmell(false);

        Tooth.SetActive(false);
        tweener.PlayTween("spawn");

        DisableGrab();

        foreach (var cell in teethCells)
        {
            cell.gameObject.SetActive(true);
        }

        switch (generationMode)
        {
            case GenerationMode.Random:
                RandomCellSetup();
                break;
            case GenerationMode.Custom:
                SetupCells();
                break;
        }

        OnCleanAmountChange?.Invoke(GetToothCleanPercent());
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

    private void CheckIfToothCleaned()
    {
        if (IsToothCleaned())
        {
            CleanTooth();
        }
        else if (OnlyDecayRemaining())
        {
            EnableGrab();
        }
    }

    private void OnCellCleaned()
    {
        OnCleanAmountChange?.Invoke(GetToothCleanPercent());

        CheckIfToothCleaned();
    }

    private void RandomCellSetup()
    {
        foreach (CellBehavior cell in teethCells)
        {
            TeethState teethState = (TeethState)Random.Range(0, 4);
            cell.SwitchTeethState(teethState);
        }

        SetSmell(Random.Range(0, 1) < 0.5f ? true : false);
    }

    private void SetupCells()
    {
        SetSmell(Random.Range(0f, 1f) < generationParameter.smellSpawnChance);

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
            teethCells[i].SwitchTeethState((TeethState)cellsState[i]);
        }

        if (OnlyDecayRemaining())
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