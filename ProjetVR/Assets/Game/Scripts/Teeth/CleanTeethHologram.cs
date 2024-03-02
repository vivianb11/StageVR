using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CleanTeethHologram : MonoBehaviour
{
    [SerializeField] ToothManager toothManager;

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        print(toothManager.CleanAmount);
    }
}
