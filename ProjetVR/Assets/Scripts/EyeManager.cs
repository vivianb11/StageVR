using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EyeManager : MonoBehaviour
{
    public static EyeManager Instance;

    [SerializeField]
    private Transform cursor;

    public Slider slider;

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

    public enum ManagerState
    {
        SELECTION, SHOOT
    }

    public ManagerState managerState = ManagerState.SELECTION;

    public Interactable interactable { get; private set; }

    [Header("Grab")]
    [SerializeField]
    private float normalOffset= 0.5f;
    private float distance;
    private Grabable grabbedBody;

    private bool preBlink = false;

    public Vector3 hitPosition;

    [Header("Events")]
    public UnityEvent leftEyeClosed;
    public UnityEvent rightEyeClosed;

    public UnityEvent leftEyeOpened;
    public UnityEvent rightEyeOpened;

    public UnityEvent blink;

    public UnityEvent<ManagerState> stateChanged;

    private void Awake()
    {
        Instance = this;
        slider.value = 0;
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
                grabbedBody.MoveTo(hit.point + hit.normal * normalOffset);
            }
            else
            {
                Vector3 targetPos = transform.position + cursor.forward * distance;

                grabbedBody.MoveTo(targetPos);
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

    public void SwitchState(ManagerState newState)
    {
        managerState = newState;
        stateChanged?.Invoke(managerState);

        switch (managerState)
        {
            case ManagerState.SELECTION:
                break;
            case ManagerState.SHOOT:
                interactable.DeSelect();
                break;
        }
    }

    private void OnInteractableSelected()
    {
        slider.value = 0;
    }

    public Vector3 GetCursorForward()
    {
        return cursor.forward;
    }

    public bool RaycastForward(out RaycastHit hit)
    {
        Debug.DrawRay(transform.position, cursor.forward, Color.red);

        bool hitSuccessful = Physics.Raycast(transform.position, cursor.forward, out hit, Mathf.Infinity, LayerMask.NameToLayer("Ignore"));

        if (hitSuccessful)
            hitPosition = hit.point;

        return hitSuccessful;
    }

    private void RaycastInteractable()
    {
        if (RaycastForward(out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Interactable component))
            {
                if (!interactable)
                {
                    interactable = component;
                    interactable.LookIn();
                    interactable.onSelected.AddListener(OnInteractableSelected);
                }

                if (interactable != component && component.activated && component.canBeInteracted)
                {
                    slider.value = 0;
                    interactable.LookOut();
                    interactable.onSelected.RemoveListener(OnInteractableSelected);
                    interactable = component;
                    interactable.LookIn();
                    interactable.onSelected.AddListener(OnInteractableSelected);
                }

                if (interactable && !interactable.selected && interactable.canBeInteracted)
                {
                    interactable.LookStay();
                    slider.maxValue = interactable.lookInTime;
                    slider.value = interactable.currentLookInTime;
                }
            }
            else if (interactable)
            {
                slider.value = 0;
                interactable.LookOut();
                interactable.onSelected.RemoveListener(OnInteractableSelected);
                interactable = null;
            }
        }
        else if (interactable)
        {
            slider.value = 0;
            interactable.LookOut();
            interactable.onSelected.RemoveListener(OnInteractableSelected);
            interactable = null;
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
