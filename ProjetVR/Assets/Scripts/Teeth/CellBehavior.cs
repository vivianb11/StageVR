using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CellManager))]
public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;

    private CellManager teethCellManager;

    // Start is called before the first frame update
    void Start()
    {
        teethCellManager = transform.GetComponent<CellManager>();

        teethState = teethCellManager.teethVarientData.RandomState();

        if (teethState == TeethState.Clean)
            teethCellManager.SetCleanState(true);
        else
            teethCellManager.SetCleanState(false);
    }
}
