using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using NaughtyAttributes;

public class Limiteur : MonoBehaviour
{
    [Header("Visual")]
    public bool showVisual = true;

    EyeManager eyeRaycaster;

    private Interacable interacable;

    [Header("Limiteur Params")]
    public bool useTargetPosition = false;

    [ShowIf("useTargetPosition")]
    [Space(10)]
    public Transform objectCenter;

    private Vector3 center;

    [Space(10)]
    public float maxDistance = 1f;
    public float errorBuffer = 0.1f;

    private void Awake()
    {
        interacable = GetComponent<Interacable>();
    }

    private void Start()
    {
        eyeRaycaster = EyeManager.Instance;

        if (useTargetPosition && objectCenter)
            center = objectCenter.transform.position;
        else
            center = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(center, transform.position) > maxDistance)
        {
            transform.position = center + (transform.position - center).normalized * maxDistance;
            interacable.rb.velocity = Vector3.zero;
        }

        if (eyeRaycaster.GetGrabbedBody())
        {
            if (Vector3.Distance(eyeRaycaster.GetGrabbedBodyDestination(),center) > maxDistance + errorBuffer)
            {
                interacable.DeSelect();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (showVisual)
        {
            Gizmos.DrawWireSphere(center, maxDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, maxDistance + errorBuffer);
        }
    }
}
