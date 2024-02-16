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
        LookIn, LookInTime
    }

    public enum DeSelectionCondition
    {
        LookOut, LookOutTime, LookDistance, AutoTime
    }

    private EyeManager eyeManager;

    [HideInInspector]
    public Rigidbody rb;

    [Header("Selection Condition")]
    public SelectionCondition selectionCondition = SelectionCondition.LookInTime;

    [ShowIf("selectionCondition", SelectionCondition.LookInTime)]
    public float lookInTime = 1f;
    [HideInInspector]
    public float currentLookInTime;

    [Header("Deselection Condition")]
    public DeSelectionCondition deSelectionCondition = DeSelectionCondition.LookOutTime;
    [ShowIf("deSelectionCondition", DeSelectionCondition.LookOutTime)]
    public float lookOutTime = 1;
    private Coroutine lookOutCoroutine;

    [ShowIf("deSelectionCondition", DeSelectionCondition.LookDistance)]
    public float lookOutDistance;

    [ShowIf("deSelectionCondition", DeSelectionCondition.AutoTime)]
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

        if (selectionCondition == SelectionCondition.LookIn)
            Select();
    }

    public void LookStay()
    {
        if (selectionCondition == SelectionCondition.LookInTime)
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

        if (deSelectionCondition == DeSelectionCondition.LookOut)
            DeSelect();
        else if (deSelectionCondition == DeSelectionCondition.LookOutTime)
            lookOutCoroutine = StartCoroutine(DeselectionTimer(lookOutTime));
    }

    public void Select()
    {
        if (selected)
            return;

        ResetSelectionValues();

        if (deSelectionCondition == DeSelectionCondition.AutoTime)
            StartCoroutine(DeselectionTimer(autoTime));

        selected = true;
        onSelected?.Invoke();
    }

    public void DeSelect()
    {
        StopAllCoroutines();

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
}