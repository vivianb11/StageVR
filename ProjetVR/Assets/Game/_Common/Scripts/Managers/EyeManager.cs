using SignalSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EyeManager : MonoBehaviour
{
    public static EyeManager Instance;

    [SerializeField]
    private Transform cursor;

    [SerializeField] LayerMask ignoreMask;

    public float maxValue;
    public float value;

    public Interactable interactable { get; private set; }

    [Header("Grab")]
    [SerializeField]
    private float distance;
    private Grabable grabbedBody;

    private bool preBlink = false;

    public bool hitSuccessful;
    public Vector3 hitPosition;
    public Object hitCollider;

    public SO_Signal grabActivation;
    public SO_Signal grabDeactivation;

    [Header("Events")]
    public UnityEvent leftEyeClosed;
    public UnityEvent rightEyeClosed;

    public UnityEvent leftEyeOpened;
    public UnityEvent rightEyeOpened;

    public UnityEvent blink;

    public UnityEvent lookInSelectable;
    public UnityEvent lookOutSelectable;
    public UnityEvent selectableSelected;

    private void Awake()
    {
        Instance = this;
        value = 0;
    }

    private void Update()
    {
        RaycastInteractable();
    }

    private void FixedUpdate()
    {
        if (grabbedBody)
        {
            if (RaycastForward(out RaycastHit hit))
            {
                grabbedBody.MoveTo(hit.point, hit.normal);
            }
            else
            {
                Vector3 targetPos = transform.position + cursor.forward * distance;

                grabbedBody.MoveTo(targetPos, hit.normal);
            }

        }
    }

    private void OnInteractableSelected()
    {
        selectableSelected?.Invoke();
        value = 0;
    }

    public Vector3 GetCursorForward()
    {
        return cursor.forward;
    }

    public bool RaycastForward(out RaycastHit hit)
    {
        Vector3 targetForward = EyeTrackingData.eyeOffset != Vector3.zero ? Quaternion.Euler(EyeTrackingData.eyeOffset) * cursor.forward : cursor.forward;

        Debug.DrawRay(transform.position, cursor.forward * 50f, Color.red);
        Debug.DrawRay(transform.position, targetForward * 50f, Color.green);

        hitSuccessful = Physics.Raycast(transform.position, targetForward, out hit, Mathf.Infinity, ignoreMask);

        if (hitSuccessful)
        {
            hitPosition = hit.point;
            hitCollider = hit.collider;
        }
        else
            hitPosition = transform.position + cursor.forward * 1000f;

        return hitSuccessful;
    }

    private void RaycastInteractable()
    {
        if (RaycastForward(out RaycastHit hit) && hit.collider.TryGetComponent(out Interactable component) && component.activated && component.enabled)
        {
            if (!interactable && component.activated)
            {
                lookInSelectable?.Invoke();

                interactable = component;
                interactable.LookIn();
                interactable.onSelected.AddListener(OnInteractableSelected);
            }

            if (interactable != component && component.activated && component.canBeInteracted)
            {
                lookInSelectable?.Invoke();

                value = 0;
                interactable.LookOut();
                interactable.onSelected.RemoveListener(OnInteractableSelected);
                interactable = component;
                interactable.LookIn();
                interactable.onSelected.AddListener(OnInteractableSelected);
            }

            if (interactable && !interactable.selected && interactable.canBeInteracted)
            {
                interactable.LookStay();
                maxValue = interactable.lookInTime;
                value = interactable.currentLookInTime;
            }
        }
        else if (interactable)
        {
            lookOutSelectable?.Invoke();

            value = 0;
            interactable.LookOut();
            interactable.onSelected.RemoveListener(OnInteractableSelected);
            interactable = null;
        }

        if (Physics.Raycast(transform.position, cursor.forward, out hit, Mathf.Infinity, LayerMask.NameToLayer("Ignore")) && hit.collider.TryGetComponent(out TestImageGenerator generator))
        {
            generator.SetPixel(hit.textureCoord, Color.red, 0);
        }
    }

    public Vector3 GetGrabbedBodyDestination()
    {
        return (transform.position + cursor.forward * distance) - grabbedBody.transform.position;
    }

    public Grabable GetGrabbedBody()
    {
        return grabbedBody;
    }

    public void SetGrabbedBody(Grabable followTarget)
    {
        if (followTarget == null)
            grabActivation.Emit();
        else
            grabDeactivation.Emit();

        grabbedBody = followTarget;

        if (!grabbedBody)
            return;
        distance = Vector3.Distance(transform.position, grabbedBody.transform.position);
    }
}
