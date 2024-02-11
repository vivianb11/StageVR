using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class EyeRaycaster : MonoBehaviour
{
    public static EyeRaycaster Instance;

    private Interacable interacable;

    private Rigidbody follow;
    private float distance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 500, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Interacable component))
            {
                if (!interacable)
                    interacable = component;

                if (interacable != component)
                {
                    interacable.DeInteract();
                    interacable = component;
                }

                if (interacable)
                    interacable.Interact();
            }
            else if (interacable)
            {
                interacable.DeInteract();
                interacable = null;
            }
        }
        else if (interacable)
        {
            interacable.DeInteract();
            interacable = null;
        }
    }

    private void FixedUpdate()
    {
        if (follow)
        {
            Vector3 direction = ((transform.position + transform.forward * distance) - follow.position).normalized;
            follow.AddForce(direction * 50);
        }
    }

    public void SetFollow(Rigidbody followTarget)
    {
        follow = followTarget;
        follow.useGravity = false;
        distance = Vector3.Distance(transform.position, follow.transform.position);
    }
}
