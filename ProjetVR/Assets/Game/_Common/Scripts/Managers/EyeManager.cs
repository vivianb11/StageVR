using SignalSystem;
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

    public Vector2 eyeOffset = new Vector2(0,0);

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
    }
}
