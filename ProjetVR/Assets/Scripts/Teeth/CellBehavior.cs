using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;

    private TeethCellManager teethCellManager;

    // Start is called before the first frame update
    void Start()
    {
        teethCellManager = transform.parent.GetComponent<TeethCellManager>();

        teethState = teethCellManager.teethVarientData.teethState;
    }

    // Update is called once per frame
    void Update()
    {

        if (teethState == TeethState.Clean)
            teethCellManager.SetCleanState(true);
        else
            teethCellManager.SetCleanState(false);
    }
}
