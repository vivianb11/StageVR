using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CellManager : MonoBehaviour
{
    [HideInInspector]
    public ToothManager toothGenerator;

    private TeethState localTeethState;
    public TeethState teethState
    {
        get
        {
            return localTeethState;
        }
        set
        {
            localTeethState = value;
            
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
}
