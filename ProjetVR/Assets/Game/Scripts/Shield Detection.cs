using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ShieldDetection : MonoBehaviour
{
    [SerializeField] [ReadOnly] private GameObject currentActiveShield;

    void Start()
    {
        currentActiveShield = null;
    }

    public void Activate(GameObject obj)
    {
        DeactivateCurrentShield();

        obj.SetActive(true);

        currentActiveShield = obj;
    }

    private void DeactivateCurrentShield()
    {
        if (currentActiveShield != null && currentActiveShield.activeSelf)
        {
            currentActiveShield.SetActive(false);
        }
    }
}
