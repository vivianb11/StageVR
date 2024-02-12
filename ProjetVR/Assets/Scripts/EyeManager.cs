using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EyeManager : MonoBehaviour
{
    public enum EyeState
    {
        OPENED, CLOSED
    }

    public static EyeManager Instance;

    public Interacable interacable { get; private set; }

    public float force = 35f;
    private float distance;
    private Rigidbody grabbedBody;

    private bool preBlink = false;

    [Header("Data")]
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

    [Header("Events")]
    public UnityEvent leftEyeClosed;
    public UnityEvent rightEyeClosed;

    public UnityEvent leftEyeOpened;
    public UnityEvent rightEyeOpened;

    public UnityEvent blink;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
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

        HandleEyesStats();

        RaycastInteractable();
    }

    private void FixedUpdate()
    {
        if (grabbedBody)
        {
            Vector3 direction = ((transform.position + transform.forward * distance) - grabbedBody.position).normalized;
            grabbedBody.AddForce(direction * force);
        }
    }

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

    private void RaycastInteractable()
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

    public Vector3 GetGrabbedBodyDestination()
    {
        return (transform.position + transform.forward * distance) - grabbedBody.position;
    }

    public Rigidbody GetGrabbedBody()
    {
        return grabbedBody;
    }

    public void SetGrabbedBody(Rigidbody followTarget)
    {
        grabbedBody = followTarget;

        if (!grabbedBody)
            return;
        distance = Vector3.Distance(transform.position, grabbedBody.transform.position);
    }
}
