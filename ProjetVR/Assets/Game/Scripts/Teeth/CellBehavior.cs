using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using SignalSystem;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(SignalListener))]
public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;
    public ToothManager toothManager { get; private set; }

    public SO_CellData cellData;

    public Transform[] foodPrefab;

    public List<Transform> neighbors;

    public UnityEvent OnClean = new UnityEvent();

    private int toothPasteAmount;
    private MeshRenderer mR;
    private Interactable interactable;
    private SignalListener signalListener;

    private void Awake()
    {
        mR = GetComponent<MeshRenderer>();
        toothManager = transform.parent.GetComponent<ToothManager>();
    }

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        signalListener = GetComponent<SignalListener>();

        GetComponent<Rigidbody>().isKinematic = true;

        ResetTooth();
        interactable.deSelectionCondition = DeSelectionCondition.LOOK_OUT;

        signalListener.signalReceived.AddListener(() => interactable.SetActivateState(true));
        signalListener.signalLost.AddListener(() => interactable.SetActivateState(false));

        OnClean.AddListener(() => interactable.enabled = false);
    }

    public void ResetTooth()
    {
        SwitchTeethState(TeethState.Clean);
        interactable.SetActivateState(false);
        interactable.SetCanBeInteracted(false);
    }

    public bool ToothPasteFull()
    {
        return toothPasteAmount == cellData.maxToothPasteCount;
    }

    public void IncreaseToothPasteAmount()
    {
        toothPasteAmount = Mathf.Clamp(toothPasteAmount + 1, 0, cellData.maxToothPasteCount);

        if (toothPasteAmount == cellData.maxToothPasteCount)
            interactable.SetCanBeInteracted(true);
    }

    private void SetMaterials(TeethState state)
    {
        switch (state)
        {
            case TeethState.Clean:
                mR.material = toothManager.cleanMat;
                break;
            case TeethState.Dirty:
                mR.material = toothManager.dirtyMat;
                break;
            case TeethState.Tartar:
                mR.material = toothManager.tartarMat;
                break;
            case TeethState.Decay:
                mR.material = toothManager.decayMat;
                break;
        }
    }

    private void SetSignalListener(TeethState state)
    {
        switch (state)
        {
            case TeethState.Clean:
                break;
            case TeethState.Dirty:
                signalListener.signal.Clear();
                signalListener.signal.Add(toothManager.brossetteSignal);
                interactable.onSelected.RemoveListener(TartarBehavior);
                interactable.onSelected.AddListener(DirtyBehavior);
                interactable.lookInTime = cellData.dirtryInteractionTime;
                break;
            case TeethState.Tartar:
                signalListener.signal.Clear();
                signalListener.signal.Add(toothManager.brushSignal);
                interactable.onSelected.RemoveListener(DirtyBehavior);
                interactable.onSelected.AddListener(TartarBehavior);
                interactable.lookInTime = cellData.tartarInteractionTime;
                interactable.resetValueOnExit = false;
                break;
        }
    }

    public void SwitchTeethState(TeethState newTeethState)
    {
        teethState = newTeethState;

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        if (teethState == TeethState.Dirty)
        {
            Transform food = Instantiate(foodPrefab.PickRandom(), transform);
            food.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f)));
            interactable.SetCanBeInteracted(true);
        }

        SetMaterials(newTeethState);
        SetSignalListener(newTeethState);
    }

    [Button("Clean Cell")]
    public void CleanCell()
    {
        SwitchTeethState(TeethState.Clean);
        toothPasteAmount = 0;

        OnClean?.Invoke();
        Debug.Log("Clean Cell");
    }

    public void DecayBehavior()
    {
        Debug.LogWarning("DecayBehavior Not Implemented!");
    }

    public void TartarBehavior()
    {
        if (!ToothPasteFull())
            return;

        CleanCell();
    }

    public void DirtyBehavior()
    {
        CleanCell();
    }

    [Button("Set Neighbours")]
    public void SetNeighboursButton()
    {
        StartCoroutine(SetNeighbours());
    }

    private IEnumerator SetNeighbours()
    {
        neighbors.Clear();

        MeshCollider mCol = GetComponent<MeshCollider>();
        mCol.isTrigger = true;

        gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        gameObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);

        mCol.isTrigger = false;

        //trie les cells par nom
        neighbors.Sort((x, y) => x.name.CompareTo(y.name));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cell"))
        {
            if (!neighbors.Contains(other.transform))
                neighbors.Add(other.transform);
        }
    }
}
