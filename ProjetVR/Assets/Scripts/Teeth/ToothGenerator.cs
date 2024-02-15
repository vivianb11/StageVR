using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothGenerator : MonoBehaviour
{
    public GameObject Tooth;

    private List<GameObject> teethCellsPosition = new List<GameObject>();

    private List<GameObject> teethCells = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == Tooth)
                continue;

            teethCellsPosition.Add(child.gameObject);
        }

        SetupCells();

        Tooth.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetTeeth();
        }
    }


    private void SetupCells()
    {
        foreach (GameObject cellPos in teethCellsPosition)
        {
            var cellData = cellPos.GetComponent<TeethCellManager>().teethVarientData;
            
            cellData.RandomState();

            GameObject go = Instantiate(cellData.GetTeeth(), cellPos.transform.position, cellPos.transform.rotation, cellPos.transform);

            teethCells.Add(go);

            //Destroy(cellPos);
        }
    }

    private void ResetTeeth()
    {
        foreach (GameObject cell in teethCells)
        {
            Destroy(cell);
        }

        teethCells.Clear();

        SetupCells();
    }
}
