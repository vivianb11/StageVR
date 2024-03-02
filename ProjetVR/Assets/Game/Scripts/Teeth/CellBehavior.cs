using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using SignalSystem;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(SignalListener))]
public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;

    public SO_CellData cellData;

    public Transform[] foodPrefab;

    public Transform[] neighbors;

    public UnityEvent OnClean = new UnityEvent();

    private int toothPasteAmount;
    private ToothManager toothManager;
    private MeshRenderer mR;
    private Interactable interactable;
    private SignalListener signalListener;

    private void Start()
    {
        toothManager = transform.parent.GetComponent<ToothManager>();
        mR = GetComponent<MeshRenderer>();
        interactable = GetComponent<Interactable>();
        signalListener = GetComponent<SignalListener>();
        GetComponent<Rigidbody>().isKinematic = true;

        interactable.deSelectionCondition = Interactable.DeSelectionCondition.LOOK_OUT;

        signalListener.signalReceived.AddListener(() => interactable.SetActivateState(true));
        signalListener.signalLost.AddListener(() => interactable.SetActivateState(false));

        OnClean.AddListener(() => interactable.enabled = false);
    }

    public bool ToothPasteFull()
    {
        return toothPasteAmount == cellData.maxToothPasteCount;
    }

    public void IncreaseToothPasteAmount()
    {
        toothPasteAmount = Mathf.Clamp(toothPasteAmount + 1, 0, cellData.maxToothPasteCount);
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
        }

        SetMaterials(newTeethState);
        SetSignalListener(newTeethState);
    }

    [Button("Clean Cell")]
    public void CleanCell()
    {
        SwitchTeethState(TeethState.Clean);
        toothPasteAmount = 0;

        OnClean.Invoke();
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

    private void SpawnFood()
    {

    }
}
