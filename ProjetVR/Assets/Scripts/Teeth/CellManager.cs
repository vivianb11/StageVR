using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public SO_TeethVarient teethVarientData;

    public bool teethCleaned = false;

    private void Awake()
    {

    }

    public void SetCleanState(bool state)
    {
        teethCleaned = state;
    }
}
