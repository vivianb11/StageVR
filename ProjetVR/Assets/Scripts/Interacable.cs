using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

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
    }

    private void Start()
    {
        eyeRaycaster = EyeManager.Instance;
        eyeRaycaster.blink.AddListener(CheckIfBlinked);
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
    public ConditionActor conditionType;

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
        conditionType = ConditionActor.LookAt;
        conditionAction = ConditionAction.Time;
        conditionValue = 0;
    }

    public Conditions(ConditionActor type , ConditionAction action, float value)
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
            case ConditionActor.LookAt:
                return LookAtCheck();
            case ConditionActor.EyeBlink:
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

public enum ConditionActor
{
    LookAt, Cursor, EyeBlink, EyeClosed, Grab, Pinch, Touch
}

public enum ConditionAction
{
    Time, Amount, Distance
}

public enum GrabType
{
    EYE, HAND
}