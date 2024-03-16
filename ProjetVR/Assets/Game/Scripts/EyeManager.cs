using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EyeManager : MonoBehaviour
{
    public static EyeManager Instance;

    [SerializeField]
    private Transform cursor;

    [SerializeField] LayerMask ignoreMask;

    public float maxValue;
    public float value;

    #region Eyes
    public enum EyeState
    {
        OPENED, CLOSED
    }

    [Header("Eyes Datas")]
    public float leftEyeOpeningPercent = 1f;
    public float rightEyeOpeningPercent = 1f;
    [SerializeField]
    private float closedTime;
    [SerializeField]
    private float blinkBuffer = 1;

    [SerializeField]
    private EyeState leftEyeState = EyeState.OPENED;
    [SerializeField]
    private EyeState rightEyeState = EyeState.OPENED;
    #endregion

    public Interactable interactable { get; private set; }

    [Header("Grab")]
    [SerializeField]
    private float distance;
    private Grabable grabbedBody;

    private bool preBlink = false;

    public bool hitSuccessful;
    public Vector3 hitPosition;

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
        #region Debug
        if (Input.GetKey(KeyCode.UpArrow))
        {
            leftEyeOpeningPercent = 1;
            rightEyeOpeningPercent = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            leftEyeOpeningPercent = 0;
            rightEyeOpeningPercent = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (leftEyeOpeningPercent == 0)
            {
                leftEyeOpeningPercent = 1;
                rightEyeOpeningPercent = 1;
            }
            else
            {
                leftEyeOpeningPercent = 0;
                rightEyeOpeningPercent = 0;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
            leftEyeOpeningPercent = Mathf.Clamp(leftEyeOpeningPercent + Input.mouseScrollDelta.y * 0.1f, 0f, 1f);
        if (Input.GetKey(KeyCode.RightArrow))
            rightEyeOpeningPercent = Mathf.Clamp(rightEyeOpeningPercent + Input.mouseScrollDelta.y * 0.1f, 0f, 1f);
        #endregion

        HandleEyesStats();

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

    #region Eyes Methods
    private IEnumerator BlinkBuffer(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (leftEyeOpeningPercent > 0 && rightEyeOpeningPercent > 0)
            blink?.Invoke();
    }

    private void HandleEyesStats()
    {
        //Left Eye
        if (leftEyeOpeningPercent == 0f && leftEyeState == EyeState.OPENED)
        {
            leftEyeState = EyeState.CLOSED;
            leftEyeClosed?.Invoke();
        }
        else if (leftEyeOpeningPercent > 0f && leftEyeState == EyeState.CLOSED)
        {
            leftEyeState = EyeState.OPENED;
            leftEyeOpened?.Invoke();
            closedTime = 0f;
        }

        //Right Eye
        if (rightEyeOpeningPercent == 0f && rightEyeState == EyeState.OPENED)
        {
            rightEyeState = EyeState.CLOSED;
            rightEyeClosed?.Invoke();
        }
        else if (rightEyeOpeningPercent > 0f && rightEyeState == EyeState.CLOSED)
        {
            rightEyeState = EyeState.OPENED;
            rightEyeOpened?.Invoke();
            closedTime = 0f;
        }

        if (leftEyeState == EyeState.CLOSED && rightEyeState == EyeState.CLOSED)
        {
            closedTime += Time.deltaTime;
        }

        if (rightEyeState == EyeState.OPENED || rightEyeState == EyeState.OPENED)
        {
            preBlink = false;
        }

        if (!preBlink && leftEyeOpeningPercent == 0 && rightEyeOpeningPercent == 0)
        {
            preBlink = true;
            StartCoroutine(BlinkBuffer(blinkBuffer));
        }
    }
    #endregion

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
        hitSuccessful = Physics.Raycast(transform.position, cursor.forward, out hit, Mathf.Infinity, ignoreMask);

        if (hitSuccessful)
            hitPosition = hit.point;
        else
            hitPosition = transform.position + cursor.forward * 1000f;

        return hitSuccessful;
    }

    private void RaycastInteractable()
    {
        if (RaycastForward(out RaycastHit hit) && hit.collider.TryGetComponent(out Interactable component) && component.activated)
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
        grabbedBody = followTarget;

        if (!grabbedBody)
            return;
        distance = Vector3.Distance(transform.position, grabbedBody.transform.position);
    }
}
