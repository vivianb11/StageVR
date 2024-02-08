using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRaycaster : MonoBehaviour
{
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 500, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {

            if (hit.collider.TryGetComponent(out Interacable component))
            {
                component.Select();
            }
        }
    }
}
