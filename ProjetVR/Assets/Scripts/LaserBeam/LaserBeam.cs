using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private bool activated;

    [SerializeField] 
    private float laserThickness;

    [SerializeField]
    private Gradient laserColor;

    [SerializeField]
    private float laserLifeTime = 0.5f;

    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();

        lineRenderer.enabled = false;
        lineRenderer.widthMultiplier = laserThickness;
        lineRenderer.colorGradient = laserColor;
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

    public void Shoot()
    {
        StartCoroutine(ShootDelay());
    }

    private IEnumerator ShootDelay()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserLifeTime);

        lineRenderer.enabled = false;
    }
}
