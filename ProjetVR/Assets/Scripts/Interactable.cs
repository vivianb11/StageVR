using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    public enum SelectionCondition
    {
        LOOK_IN, LOOK_IN_TIME
    }

    public enum DeSelectionCondition
    {
        LOOK_OUT, LOOK_OUT_TIME, LOOK_DISTANCE, AUTO_TIME, NONE
    }

    private EyeManager eyeManager;

    [HideInInspector]
    public Rigidbody rb;

    [Header("Selection Condition")]
    public SelectionCondition selectionCondition = SelectionCondition.LOOK_IN_TIME;

    [ShowIf("selectionCondition", SelectionCondition.LOOK_IN_TIME)]
    public float lookInTime = 1f;
    [HideInInspector]
    public float currentLookInTime;

    [Header("Deselection Condition")]
    public DeSelectionCondition deSelectionCondition = DeSelectionCondition.LOOK_OUT_TIME;
    [ShowIf("deSelectionCondition", DeSelectionCondition.LOOK_OUT_TIME)]
    public float lookOutTime = 1;
    private Coroutine lookOutCoroutine;

    [ShowIf("deSelectionCondition", DeSelectionCondition.LOOK_DISTANCE)]
    public float lookOutDistance;

    [ShowIf("deSelectionCondition", DeSelectionCondition.AUTO_TIME)]
    public float autoTime;

    [Header("Active In State")]
    public List<EyeManager.ManagerState> activeStats = new List<EyeManager.ManagerState> { EyeManager.ManagerState.SELECTION };

    [Header("General Parameters")]
    public bool canAlwaysInteract;

    [HideInInspector]
    public bool activated { get; private set; } = true;
    [HideInInspector]
    public bool canBeInteracted { get; private set; } = true;

    public bool selected;


    [Header("Events")]
    public EventDelay[] selectedEventsDelay;
    public EventDelay[] deSelectedEventsDelay;
    [Space(10)]
    public UnityEvent onSelected;
    public UnityEvent onDeselected;
    [Space(10)]
    public UnityEvent lookIn;
    public UnityEvent lookOut;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        eyeManager = EyeManager.Instance;

        eyeManager.stateChanged.AddListener(OnManagerStateChanged);
    }

    private void OnManagerStateChanged(EyeManager.ManagerState managerState)
    {
        activated = activeStats.Contains(managerState) || canAlwaysInteract;
    }

    public void LookIn() 
    {
        lookIn?.Invoke();

        if (lookOutCoroutine != null)
        {
            StopCoroutine(lookOutCoroutine);
            lookOutCoroutine = null;
        }

        if (selectionCondition == SelectionCondition.LOOK_IN)
            Select();
    }

    public void LookStay()
    {
        if (selectionCondition == SelectionCondition.LOOK_IN_TIME)
        {
            currentLookInTime = Mathf.Clamp(currentLookInTime + Time.deltaTime, 0, lookInTime);

            if (currentLookInTime == lookInTime)
                Select();
        }
    }

    public void LookOut()
    {
        lookOut?.Invoke();

        ResetSelectionValues();

        if (deSelectionCondition == DeSelectionCondition.LOOK_OUT)
            DeSelect();
        else if (deSelectionCondition == DeSelectionCondition.LOOK_OUT_TIME)
            lookOutCoroutine = StartCoroutine(DeselectionTimer(lookOutTime));
    }

    public void Select()
    {
        if (selected)
            return;

        foreach (EventDelay eventDelay in selectedEventsDelay)
        {
            StartCoroutine(CallEventWithDelay(eventDelay));
        }

        ResetSelectionValues();

        if (deSelectionCondition == DeSelectionCondition.AUTO_TIME)
            StartCoroutine(DeselectionTimer(autoTime));

        selected = true;
        onSelected?.Invoke();
    }

    public void DeSelect()
    {
        StopAllCoroutines();

        foreach (EventDelay eventDelay in deSelectedEventsDelay)
        {
            StartCoroutine(CallEventWithDelay(eventDelay));
        }

        selected = false;
        onDeselected?.Invoke();
    }

    private void ResetSelectionValues()
    {
        currentLookInTime = 0;
    }

    public void SetCanBeInteracted(bool value)
    {
        canBeInteracted = value;
    }

    private IEnumerator DeselectionTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeSelect();
    }

    private IEnumerator CallEventWithDelay(EventDelay eventDelay)
    {
        yield return new WaitForSeconds(eventDelay.delay);

        eventDelay.unityEvent?.Invoke();
    }
}

[Serializable]
public struct EventDelay
{
    public float delay;
    public UnityEvent unityEvent;
}