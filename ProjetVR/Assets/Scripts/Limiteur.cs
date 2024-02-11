using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Limiteur : MonoBehaviour
{
    public bool showVisual = true;

    public GameObject center;

    public float maxDistance;

    private void Update()
    {
        if (Vector3.Distance(center.transform.position, transform.position) > maxDistance)
        {
            transform.position = center.transform.position + (transform.position - center.transform.position).normalized * maxDistance;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (showVisual)
            Gizmos.DrawWireSphere(center.transform.position, maxDistance);
    }

}
