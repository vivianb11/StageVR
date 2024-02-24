using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothGenerator : MonoBehaviour
{
    public GameObject Tooth;

    [Foldout("Materials")]
    public Material cleanMaterial;
    [Foldout("Materials")]
    public Material dirtyMaterial;
    [Foldout("Materials")]
    public Material tartarMaterial;
    [Foldout("Materials")]
    public Material decayMaterial;

    private List<GameObject> teethCells = new List<GameObject>();

    public int CleanAmount = 0;

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == Tooth)
                continue;

            teethCells.Add(child.gameObject);
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
        throw new System.NotImplementedException();
    }

    private void ResetTeeth()
    {
        SetupCells();
    }
}
