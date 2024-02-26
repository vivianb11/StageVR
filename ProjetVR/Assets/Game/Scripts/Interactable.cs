using UnityEngine;
using UnityEngine.Events;
using System;
using NaughtyAttributes;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    public enum SelectionCondition
    {
        LOOK_IN, LOOK_IN_TIME, NONE
    }

    public enum DeSelectionCondition
    {
        LOOK_OUT, LOOK_OUT_TIME, LOOK_DISTANCE, AUTO_TIME, NONE
    }

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

    [HideInInspector]
    public bool activated { get; set; } = true;
    private bool preActivated;

    [HideInInspector]
    public bool canBeInteracted { get; private set; } = true;

    public bool selected;

    [Header("Events")]
    public EventDelay[] selectedEventsDelay = new EventDelay[0];
    public EventDelay[] deSelectedEventsDelay = new EventDelay[0];
    [Space(10)]
    public UnityEvent onSelected = new UnityEvent();
    public UnityEvent onDeselected = new UnityEvent();
    [Space(10)]
    public UnityEvent lookIn = new UnityEvent();
    public UnityEvent lookOut = new UnityEvent();

    public UnityEvent<bool> activeStateChanged = new UnityEvent<bool>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        EyeManager eyeManager = EyeManager.Instance;
    }

    private void OnEnable()
    {
        activated = preActivated;
    }

    private void OnDisable()
    {
        activated = false;
    }

    public void SetActivateState(bool value)
    {
        if (activated != value)
            activeStateChanged?.Invoke(value);

        activated = value;
        preActivated = value;
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