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
    [Header("Selection")]
    [Space(10)]
    public List<Conditions> selectConditions;
    [Header("Deselection")]
    [Space(10)]
    public List<Conditions> deselectConditions;

    public Rigidbody rb;

    private bool canBeInteracted = true;

    public bool selected;

    private EyeManager eyeRaycaster;

    public float interactionTime;

    [Header("Events")]
    public UnityEvent onSelected;
    public UnityEvent onDeselected;
    [Space(10)]
    public UnityEvent onInteracted;
    public UnityEvent onDeInteracted;
    [Space(10)]
    public UnityEvent onBlinked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < selectConditions.Count; i++)
        {
            var condition = selectConditions[i];
            
            if (condition.conditionEye == ConditionsEye.EyeBlink)
                eyeRaycaster.blink.AddListener(CheckIfBlinked);
        }
    }

    private void Start()
    {
        eyeRaycaster = EyeManager.Instance;
    }

    public void CheckIfBlinked()
    {
        if (eyeRaycaster.interactable == this)
            onBlinked?.Invoke();
    }

    public void Interact(Slider slider) 
    {
        if (selected || !canBeInteracted)
            return;

        if (selectConditions.Where(item => item.conditionEye == ConditionsEye.LookAt).ToArray().Length > 0)
        {
            slider.maxValue = selectConditions.Where(item => item.conditionEye == ConditionsEye.LookAt).ToArray()[0].conditionValue;
            slider.value += Time.deltaTime;
        }

        onInteracted?.Invoke();

        int temp = 0;

        for (int i = 0; i < selectConditions.Count; i++)
        {
            temp += selectConditions[i].CheckCondition() ? 1 : 0;
        }

        if (temp == selectConditions.Count)
            Select();
    }

    public void DeInteract() 
    {
        onDeInteracted?.Invoke();

        int temp = 0;

        for (int i = 0; i < deselectConditions.Count; i++)
        {
            temp += deselectConditions[i].CheckCondition() ? 1 : 0;
        }

        if (temp == deselectConditions.Count && deselectConditions.Count > 0)
            DeSelect();

        for (int i = 0; i < selectConditions.Count; i++)
        {
            selectConditions[i].Reset();
        }
    }

    public void Select()
    {
        for(int i = 0;i < selectConditions.Count;i++)
        {
            selectConditions[i].Reset();
        }

        selected = true;
        onSelected?.Invoke();
    }

    public void DeSelect()
    {
        selected = false;
        onDeselected?.Invoke();
    }

    public void SetCanBeInteracted(bool value)
    {
        canBeInteracted = value;
    }
}

[Serializable]
public class Conditions
{
    public SelectType selectType;

    [ShowIf("selectType", SelectType.EYE)]
    [Space(10)]
    public ConditionsEye conditionEye;

    [ShowIf("selectType", SelectType.HAND)]
    public ConditionsHand conditionHand;

    [Space(10)]
    public ConditionAction conditionAction;

    [Space(10)]
    public float conditionValue;

    [ShowIf("conditionAction", ConditionAction.Amount)]
    public float DetectionInterval = 1f;

    // Variable instance
    private float timer;
    private float distance;
    private int count;

    public Conditions()
    {
        conditionEye = ConditionsEye.LookAt;
        conditionAction = ConditionAction.Time;
        conditionValue = 0;
    }

    public Conditions(ConditionsEye type , ConditionAction action, float value)
    {
        conditionEye = type;
        conditionAction = action;
        conditionValue = value;
    }

    public void Reset()
    {
        timer = 0;
        count = 0;
        distance = 0;
    }

    public bool CheckCondition()
    {
        switch (conditionEye)
        {
            case ConditionsEye.LookAt:
                return LookAtCheck();

            case ConditionsEye.NotLooking:
                return NotLookingCheck();

            case ConditionsEye.EyeBlink:
                Debug.LogWarning("EyeBlink not implemented yet");
                return true;

            case ConditionsEye.EyeClosed:
                Debug.LogWarning("EyeClosed not implemented yet");
                return true;

            case ConditionsEye.Cursor:
                Debug.LogWarning("Cursor not implemented yet");
                return true;

            default:
                return false;
        }
    }

    private bool LookAtCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                return Timer(conditionValue);
            case ConditionAction.Amount:
                return true;
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for LookAt yet");
                return true;
            default:
                return false;
        }
    }

    private bool NotLookingCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                return Timer(conditionValue);
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for NotLooking yet");
                return true;
            default:
                return false;
        }
    }

    private bool EyeBlinkCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                Debug.LogWarning("Time not implemented for EyeBlink yet");
                return true;
            case ConditionAction.Amount:
                Debug.LogWarning("Amount not implemented for EyeBlink yet");
                return true;
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for EyeBlink yet");
                return true;
            default:
                return false;
        }
    }

    private bool EyeClosedCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                Debug.LogWarning("Time not implemented for EyeClosed yet");
                return true;
            case ConditionAction.Amount:
                Debug.LogWarning("Amount not implemented for EyeClosed yet");
                return true;
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for EyeClosed yet");
                return true;
            default:
                return false;
        }
    }

    private bool CursorCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                Debug.LogWarning("Time not implemented for Cursor yet");
                return true;
            case ConditionAction.Amount:
                Debug.LogWarning("Amount not implemented for Cursor yet");
                return true;
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for Cursor yet");
                return true;
            default:
                return false;
        }
    }

    private bool GrabCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                Debug.LogWarning("Time not implemented for Grab yet");
                return true;
            case ConditionAction.Amount:
                Debug.LogWarning("Amount not implemented for Grab yet");
                return true;
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for Grab yet");
                return true;
            default:
                return false;
        }
    }

    private bool PinchCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                Debug.LogWarning("Time not implemented for Pinch yet");
                return true;
            case ConditionAction.Amount:
                Debug.LogWarning("Amount not implemented for Pinch yet");
                return true;
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for Pinch yet");
                return true;
            default:
                return false;
        }
    }

    private bool TouchCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                Debug.LogWarning("Time not implemented for Touch yet");
                return true;
            case ConditionAction.Amount:
                Debug.LogWarning("Amount not implemented for Touch yet");
                return true;
            case ConditionAction.Distance:
                Debug.LogWarning("Distance not implemented for Touch yet");
                return true;
            default:
                return false;
        }
    }

    public bool Timer(float time)
    {
        timer += Time.deltaTime;

        return timer >= time;
    }
}

public enum ConditionsEye
{
    LookAt, NotLooking, Cursor, EyeBlink, EyeClosed
}

public enum ConditionsHand
{
    Grab, Pinch, Touch
}

public enum SelectType
{
    EYE, HAND
}

public enum ConditionAction
{
    Time, Amount, Distance
}