using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    private bool activated;

    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            transform.LookAt(EyeManager.Instance.hitPosition);
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(EyeManager.Instance.hitPosition));
        }
    }

    public void ToogleActivation()
    {
        activated = !activated;

        interactable.selected = false;
    }
}
