using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using JetBrains.Annotations;

public class Interacable : MonoBehaviour
{
    [Header("Selection")]
    [Space(10)]
    public List<Conditions> selectConditions;
    public UnityEvent onSelected;
    [Header("Deselection")]
    [Space(10)]
    public List<Conditions> deselectConditions;
    public UnityEvent onDeselected;

    public UnityEvent onBlinked;

    public Rigidbody rb;

    private bool selected;

    private EyeManager eyeRaycaster;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < selectConditions.Count; i++)
        {
            var condition = selectConditions[i];
            
            if (condition.conditionType == ConditionsEye.EyeBlink)
                eyeRaycaster.blink.AddListener(CheckIfBlinked);
        }
    }

    private void Start()
    {
        eyeRaycaster = EyeManager.Instance;
    }

    public void CheckIfBlinked()
    {
        if (eyeRaycaster.interacable == this)
            onBlinked?.Invoke();
    }

    public void Interact() 
    {
        if (selected)
            return;

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

}

[Serializable]
public class Conditions
{
    public ConditionsEye conditionType;

    public ConditionAction conditionAction;

    public float conditionValue;

    [ShowIf("conditionAction", ConditionAction.Amount)]
    public float DetectionInterval = 1f;

    // Variable instance
    private float timer;
    private float distance;
    private int count;

    public Conditions()
    {
        conditionType = ConditionsEye.LookAt;
        conditionAction = ConditionAction.Time;
        conditionValue = 0;
    }

    public Conditions(ConditionsEye type , ConditionAction action, float value)
    {
        conditionType = type;
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
        switch (conditionType)
        {
            case ConditionsEye.LookAt:
                return LookAtCheck();

            case ConditionsEye.NotLooking:
                Debug.LogWarning("NotLooking not implemented yet");
                return true;

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
                return LookAtTimer(conditionValue);
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
                Debug.LogWarning("Time not implemented for NotLooking yet");
                return true;
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

    public bool LookAtTimer(float time)
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

public enum GrabType
{
    EYE, HAND
}

public enum ConditionAction
{
    Time, Amount, Distance
}