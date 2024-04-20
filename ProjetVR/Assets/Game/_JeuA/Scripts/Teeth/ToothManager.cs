using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using SignalSystem;
using UnityEngine.Events;
using System;

public class ToothManager : MonoBehaviour
{
    public enum GenerationMode
    {
        Random,
        Custom
    }

    public GenerationMode generationMode = GenerationMode.Random;

    public GameObject Tooth;
    public List<CellBehavior> teethCells = new List<CellBehavior>();

    [SerializeField] List<CellList> notNeighborsCells;

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
    [Foldout("Materials")]
    public Material toothPasteMat;

    public SO_Signal brushSignal;
    public SO_Signal brossetteSignal;

    private Tween tweener;
    public ParticleSystem smellVFX;

    public bool smells { get; private set; }

    public int maxSmellAmount = 10;
    private int smellAmount = 10;

    [Foldout("Events")]
    public UnityEvent OnTeethCleaned;
    [Foldout("Events")]
    [InfoBox("Returns a float")]
    public UnityEvent<float> OnCleanAmountChange;
    [Foldout("Events")]
    public UnityEvent OnToothPreCleaned, OnToothPreCleanedWithDecay;
    [Foldout("Events")]
    public UnityEvent CellCleaned;
    [Foldout("Events")]
    public UnityEvent<int> GenerationListIndex;
    [Foldout("Events")]
    public UnityEvent decayOnly;
    [Foldout("Events")]
    public UnityEvent respawned;

    [Foldout("Playtest Only")]
    public bool toothPasteColorChange;

    private void Awake()
    {
        tweener = GetComponent<Tween>();

        smellAmount = maxSmellAmount;

        smellVFX.gameObject.SetActive(true);
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

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetTooth();
        }
    }
#endif

    public void SetSmell(bool value)
    {
        smells = value;

        smellAmount = maxSmellAmount;

        if (value)
            smellVFX.Play();
        else
            smellVFX.Stop();
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

        OnToothPreCleanedWithDecay?.Invoke();

        return true;
    }

    [Button]
    public void CleanTooth()
    {
        foreach (var cell in teethCells)
        {
            cell.gameObject.SetActive(false);
        }

        SetSmell(false);

        Tooth.SetActive(true);
        tweener.PlayTween("despawn");

        dataIndex = Mathf.Clamp(dataIndex + 1, 0, datas.Length - 1);

        OnCleanAmountChange?.Invoke(1);
        OnTeethCleaned?.Invoke();
    }

    public void ResetTooth()
    {
        respawned?.Invoke();

        transform.localPosition = Vector3.zero;
        SetSmell(false);

        Tooth.SetActive(false);
        tweener.PlayTween("spawn");

        GenerationListIndex?.Invoke(dataIndex);

        foreach (var cell in teethCells)
        {
            cell.gameObject.SetActive(true);
            cell.ResetTooth();
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

    private void CheckIfToothCleaned()
    {
        if (IsToothCleaned())
        {
            OnToothPreCleaned?.Invoke();
            CleanTooth();
        }
        else if (OnlyDecayRemaining())
        {
            decayOnly?.Invoke();
        }
    }

    private void OnCellCleaned()
    {
        OnCleanAmountChange?.Invoke(GetToothCleanPercent());
        CellCleaned?.Invoke();

        CheckIfToothCleaned();
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

        List<CellBehavior> cleanedCells = new List<CellBehavior>();
        List<TeethState> teethStates = new List<TeethState>();

        teethCells.ForEach(item => cleanedCells.Add(item));

        foreach (Anomaly anomaly in gP.anomalies)
        {
            float weight = anomaly.curve.Evaluate(Random.Range(0f, 1f));
            
            int amount = ((int)Mathf.Lerp(anomaly.minMax.x, anomaly.minMax.y, weight));
            int count = Mathf.Min(cleanedCells.Count, amount);

            for (int i = 0; i < count; i++)
            {
                CellBehavior cellBehavior = cleanedCells.PickRandom();
                cleanedCells.Remove(cellBehavior);

                teethStates.Add(anomaly.teethState);
            }
        }

        teethCells.Shuffle();

        if (teethStates.Count > 3)
        {
            for (int i = 0; i < teethStates.Count; i++)
                teethCells[i].SwitchTeethState(teethStates[i]);
        }
        else
        {
            CellList cellList = notNeighborsCells.PickRandom();
            Debug.Log("Selected list index: " + notNeighborsCells.IndexOf(cellList));

            for (int i = 0; i < teethStates.Count; i++)
            {
                cellList.cells[i].SwitchTeethState(teethStates[i]);
            }
        }

        if (OnlyDecayRemaining())
        {
            decayOnly?.Invoke();
        }
    }
}

[Serializable]
public struct CellList
{
    public List<CellBehavior> cells;
}