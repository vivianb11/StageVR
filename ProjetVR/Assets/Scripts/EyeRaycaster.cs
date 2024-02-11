using UnityEngine;

public class EyeRaycaster : MonoBehaviour
{
    public static EyeRaycaster Instance;

    private Interacable interacable;

    public float force = 35f;
    private float distance;
    private Rigidbody grabbedBody;

    private void Awake()
    {
        Instance = this;

        //GetComponent<OVREyeGaze>().
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
        if (grabbedBody)
        {
            Vector3 direction = ((transform.position + transform.forward * distance) - grabbedBody.position).normalized;
            grabbedBody.AddForce(direction * force);
        }
    }

    public Vector3 GetGrabbedBodyDestination()
    {
        return (transform.position + transform.forward * distance) - grabbedBody.position;
    }

    public void SetFollow(Rigidbody followTarget)
    {
        grabbedBody = followTarget;
        grabbedBody.useGravity = false;
        distance = Vector3.Distance(transform.position, grabbedBody.transform.position);
    }
}
