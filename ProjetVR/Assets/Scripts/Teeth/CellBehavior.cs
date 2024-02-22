using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CellManager))]
public class CellBehavior : MonoBehaviour
{
    public TeethState teethState;

    private CellManager teethCellManager;

    public UnityEvent OnClean;

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
