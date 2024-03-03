using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using System.Linq;
using SignalSystem;
using UnityEngine.Events;
using UnityEngine.Rendering;
using System;

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

    public SO_TeethGeneration[] datas;
    private int dataIndex;

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
    [Foldout("Events")]
    public UnityEvent OnToothPreCleaned;

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

        dataIndex = Mathf.Clamp(dataIndex + 1, 0, datas.Length - 1);
    }

    public void RemoveSmellAmount()
    {
        if (!smells)
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

        foreach (var cell in teethCells)
        {
            cell.gameObject.SetActive(true);
            cell.SwitchTeethState(TeethState.Clean);
        }
        
        DisableGrab();

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
            EnableGrab();

            OnToothPreCleaned?.Invoke();
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

        Debug.Log(GetToothCleanPercent());
    }

    private void RandomCellSetup()
    {
        foreach (CellBehavior cell in teethCells)
        {
            TeethState teethState = (TeethState)Random.Range(0, 4);
            cell.SwitchTeethState(teethState);
        }

        SetSmell(Random.Range(0f, 1f) < 0.5f ? true : false);
    }

    private void SetupCells()
    {
        SO_TeethGeneration gP = datas[dataIndex];

        SetSmell(Random.Range(0f, 1f) < gP.smellSpawnChance);

        cellsState = new List<int>(teethCells.Count);
        settedCells = new List<bool>(teethCells.Count);

        List<int> highNeighbourCells = new List<int>();

        for (int i = 0; i < teethCells.Count; i++)
        {
            cellsState.Add(0);
            settedCells.Add(false);

            if (teethCells[i].neighbors.Count >= 5)
            {
                highNeighbourCells.Add(i);
            }
        }

        highNeighbourCells = highNeighbourCells.OrderBy(x => teethCells[x].neighbors.Count).ToList();

        List<CellBehavior> cleanedCells = new List<CellBehavior>();

        teethCells.ForEach((item) => cleanedCells.Add(item));

        foreach (Anomaly anomaly in gP.anomalies)
        {
            float weight = anomaly.curve.Evaluate(Random.Range(0f, 1f));
            
            int amount = ((int)Mathf.Lerp(anomaly.minMax.x, anomaly.minMax.y, weight));
            int count = Mathf.Min(cleanedCells.Count, amount);

            for (int i = 0; i < count; i++)
            {
                CellBehavior cellBehavior = cleanedCells.PickRandom();
                cleanedCells.Remove(cellBehavior);
                cellBehavior.SwitchTeethState(anomaly.teethState);
            }
        }

        if (OnlyDecayRemaining())
            EnableGrab();
    }
}