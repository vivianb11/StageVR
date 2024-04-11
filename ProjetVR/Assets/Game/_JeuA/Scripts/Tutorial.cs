using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] bool enableTutorial;

    [SerializeField] int maxIndex;

    [SerializeField] int indexDoublon;

    [SerializeField] ToothManager toothManager;

    private void Start()
    {
        if (enableTutorial)
        {
            UpdateChildVisibility(0);
            toothManager.GenerationListIndex.AddListener(UpdateChildVisibility);
        }
    }

    private void UpdateChildVisibility(int index)
    {
        if (index > maxIndex)
            return;

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        transform.GetChild(index).gameObject.SetActive(true);
        if (index == indexDoublon)
            transform.GetChild(index + 1).gameObject.SetActive(true);
    }
}
