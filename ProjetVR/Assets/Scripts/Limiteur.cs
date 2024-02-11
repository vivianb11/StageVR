using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using NaughtyAttributes;

public class Limiteur : MonoBehaviour
{
    [Header("Visual")]
    public bool showVisual = true;

    EyeRaycaster eyeRaycaster;

    private Interacable interacable;

    [Header("Limiteur Params")]
    public bool useObjectPosition = false;

    [ShowIf("useObjectPosition")]
    [Space(10)]
    public Transform objectCenter;

    private Vector3 center;

    [Space(10)]
    public float maxDistance = 1f;
    public float errorBuffer = 0.1f;

    private void Awake()
    {
        eyeRaycaster = EyeRaycaster.Instance;
        interacable = GetComponent<Interacable>();

        if (!useObjectPosition)
            center = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(center, transform.position) > maxDistance)
        {
            transform.position = center + (transform.position - center).normalized * maxDistance;
            interacable.rb.velocity = Vector3.zero;

        }
        if (Vector3.Distance(eyeRaycaster.GetGrabbedBodyDestination(),center) > maxDistance + errorBuffer)
        {
            interacable.DeSelect();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (showVisual)
        {
            if (!useObjectPosition)
                center = transform.position;

            Gizmos.DrawWireSphere(center, maxDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, maxDistance + errorBuffer);
        }
    }
}
