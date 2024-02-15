using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;

    // Start is called before the first frame update
    void Start()
    {
        if(teethState == TeethState.Clean)
            transform.parent.GetComponent<TeethCellManager>().SetCleanState(true);
        else
            transform.parent.GetComponent<TeethCellManager>().SetCleanState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
