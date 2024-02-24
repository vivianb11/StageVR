using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CellManager : MonoBehaviour
{
    [HideInInspector]
    public ToothGenerator toothGenerator;

    public TeethState teethState
    {
        get
        {
            return teethState;
        }
        set
        {
            teethState = value;
            
            if (value == TeethState.Clean)
            {
                OnClean.Invoke();
                teethCleaned = true;
            }
        }
    }

    [HideInInspector]
    public bool teethCleaned = false;

    public UnityEvent OnClean;

    private void Awake()
    {
        toothGenerator = this.transform.parent.GetComponent<ToothGenerator>();

        teethState = (TeethState)Random.Range(0, (Enum.GetValues(typeof (TeethState)).Length));
    }
}
