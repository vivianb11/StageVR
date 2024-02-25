using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using SignalSystem;

public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;

    public UnityEvent OnClean = new UnityEvent();

    private int maxToothPasteAmount = 10;
    public int toothPasteAmount;

    private bool selected;

    private ToothManager toothManager;
    private MeshRenderer mR;

    private void Start()
    {
        toothManager = transform.parent.GetComponent<ToothManager>();
        mR = GetComponent<MeshRenderer>();

        Interactable interactable = gameObject.AddComponent<Interactable>();
        SignalListener signalListener = gameObject.AddComponent<SignalListener>();

        interactable.onSelected.AddListener(TartarBehavior);
        interactable.deSelectionCondition = Interactable.DeSelectionCondition.LOOK_OUT;

        interactable.lookIn.AddListener(() => SetSelected(true));
        interactable.lookOut.AddListener(() => SetSelected(false));

        signalListener.signal.Add(toothManager.brushSignal);
        signalListener.signalReceived.AddListener(() => interactable.SetActivateState(true));
        signalListener.signalLost.AddListener(() => interactable.SetActivateState(false));

        OnClean.AddListener(() => interactable.enabled = false);
    }

    public void FixedUpdate()
    {
        if (!selected)
            return;

        switch(teethState)
        {
            case TeethState.Dirty:
                DirtyBehavior();
                break;
            case TeethState.Tartar:
                TartarBehavior();
                break;
            case TeethState.Decay:
                DecayBehavior();
                break;
        }
    }

    public void SetSelected(bool value)
    {
        selected = value;
    }

    public bool ToothPasteFull()
    {
        return toothPasteAmount == maxToothPasteAmount;
    }

    public void IncreaseToothPasteAmount()
    {
        toothPasteAmount = Mathf.Clamp(toothPasteAmount + 1, 0, maxToothPasteAmount);
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

    public void SwitchTeethState(TeethState newTeethState)
    {
        teethState = newTeethState;

        SetMaterials(teethState);
    }

    [Button("Clean Cell")]
    private void CleanCell()
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
        Debug.LogWarning("DirtyBehavior Not Implemented!");
    }
}
