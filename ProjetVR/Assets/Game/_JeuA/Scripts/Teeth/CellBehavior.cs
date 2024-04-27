using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using SignalSystem;
using System.Collections;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(SignalListener))]
public class CellBehavior : MonoBehaviour
{
    [SerializeField] GameObject BrushParticle;

    [SerializeField] FeedbackScale _feedbackScale;

    public TeethState teethState;
    public ToothManager toothManager { get; private set; }

    public SO_CellData cellData;

    public Transform[] foodPrefab;

    public UnityEvent OnClean = new UnityEvent();

    private int toothPasteAmount;
    private MeshRenderer mR;
    private Interactable interactable;
    private SignalListener signalListener;

    private void Awake()
    {
        mR = GetComponent<MeshRenderer>();
        toothManager = transform.parent.GetComponent<ToothManager>();
        interactable = GetComponent<Interactable>();
        signalListener = GetComponent<SignalListener>();

        _feedbackScale = GetComponent<FeedbackScale>();
    }

    private void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;

        interactable.deSelectionCondition = DeSelectionCondition.LOOK_OUT;

        signalListener.signalReceived.AddListener(() => interactable.SetActivateState(true));
        signalListener.signalLost.AddListener(() => interactable.SetActivateState(false));

        OnClean.AddListener(() => interactable.SetActivateState(false));
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
        {
            _feedbackScale.SetActive(true);

            StartCoroutine(StartTPFeedback());

            if (toothManager.toothPasteColorChange) mR.material = toothManager.toothPasteMat;
            interactable.SetCanBeInteracted(true);
        }
    }

    public IEnumerator StartTPFeedback()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ToothPasteProjectile toothPaste))
                toothPaste.StartFeedback();
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void SetMaterials(TeethState state)
    {
        switch (state)
        {
            case TeethState.Clean:
                mR.material = toothManager.cleanMat;
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
                signalListener.signal.Clear();
                interactable.onSelected.RemoveAllListeners();
                
                _feedbackScale.SetActive(false);
                break;
            case TeethState.Tartar:
                signalListener.signal.Clear();
                signalListener.signal.Add(toothManager.brushSignal);
                interactable.onSelected.AddListener(TartarBehavior);
                interactable.lookInTime = cellData.tartarInteractionTime;
                interactable.resetValueOnExit = false;

                _feedbackScale.SetActive(false);
                break;
            default:
                signalListener.signal.Clear();
                interactable.onSelected.RemoveAllListeners();
                
                _feedbackScale.SetActive(false);
                break;
        }
    }

    public void SwitchTeethState(TeethState newTeethState)
    {
        if (newTeethState == TeethState.Dirty) newTeethState = TeethState.Clean;

        if (teethState == TeethState.Tartar && newTeethState != TeethState.Tartar)
            foreach (Transform child in transform)
                Destroy(child.gameObject);

        teethState = newTeethState;

        SetMaterials(newTeethState);
        SetSignalListener(newTeethState);
    }

    [Button("Clean Cell")]
    public void CleanCell()
    {
        SwitchTeethState(TeethState.Clean);
        toothPasteAmount = 0;

        OnClean?.Invoke();
    }

    public void TartarBehavior()
    {
        if (!ToothPasteFull())
            return;

        CleanCell();
    }
}
