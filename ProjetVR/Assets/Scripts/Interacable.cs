using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

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

    
    public virtual void Interact() { }

    private void Start()
    {
        selectConditions.Add(new Conditions(Conditions.ConditionActor.LookAt, Conditions.ConditionAction.Time, 1f));
    }

    public void Select()
    {
        onSelected?.Invoke();
    }

    public void DeSelect()
    {
        onDeselected?.Invoke();
    }

}

[Serializable]
public class Conditions
{
    public enum ConditionActor
    {
        LookAt, Cursor, Blink, Grab, Pinch, Touch
    }

    public enum ConditionAction
    {
        Time, Amount, Distance
    }

    public ConditionActor conditionType;

    public ConditionAction conditionAction;

    public float conditionValue;

    public Conditions(ConditionActor type , ConditionAction action, float value)
    {
        conditionType = type;
        conditionAction = action;
        conditionValue = value;
    }


    public bool CheckCondition()
    {
        switch (conditionType)
        {
            case ConditionActor.LookAt:
                return true;
            case ConditionActor.Blink:
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
                return true;
            case ConditionAction.Amount:
                return true;
            case ConditionAction.Distance:
                return true;
            default:
                return false;
        }
    }
}