using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Grabable : MonoBehaviour
{
    public enum AlignDirection
    {
        UP, DOWN, LEFT, RIGHT, FORWARD, BACKWARD
    }

    private Interactable interacable;

    public float moveSpeed = 2f;

    public bool alignToNormal;

    public bool lookAtCamera;

    [ShowIf("alignToNormal")]
    public AlignDirection alignDirection = AlignDirection.UP;

    [ShowIf("alignToNormal")]
    public float distanceToPoint = 0.5f;

    private bool preUseGravity;
    private bool preIsKinematic;

    private void Awake()
    {
        interacable = GetComponent<Interactable>();
        interacable.onSelected.AddListener(OnSelected);
        interacable.onDeselected.AddListener(OnDeselected);
    }

    private void Start()
    {
        interacable.rb.angularDrag = 1f;
    }

    public void AlignToNormal(Vector3 normal)
    {
        switch (alignDirection)
        {
            case AlignDirection.UP:
                transform.up = normal;
                break;
            case AlignDirection.DOWN:
                transform.up = -normal;
                break;
            case AlignDirection.LEFT:
                transform.right = -normal;
                break;
            case AlignDirection.RIGHT:
                transform.right = normal;
                break;
            case AlignDirection.FORWARD:
                transform.forward = normal;
                break;
            case AlignDirection.BACKWARD:
                transform.forward = -normal;
                break;
        }
    }

    public void MoveTo(Vector3 targetPos, Vector3 normal)
    {
        Vector3 targetNormalPos = targetPos + normal * distanceToPoint;
        Vector3 directionToTarget = targetNormalPos - transform.position;

        if (alignToNormal)
            AlignToNormal(normal);

        if (lookAtCamera)
            transform.LookAt(Camera.main.transform.position, normal);

        interacable.rb.velocity = Vector3.Lerp(interacable.rb.velocity, directionToTarget * moveSpeed, 1f);
    }

    private void OnSelected()
    {
        preUseGravity = interacable.rb.useGravity;
        preIsKinematic = interacable.rb.isKinematic;

        interacable.rb.useGravity = false;
        interacable.rb.isKinematic = false;

        gameObject.layer = LayerMask.NameToLayer("Ignore");

        EyeManager.Instance.SetGrabbedBody(this);
    }

    private void OnDeselected()
    {
        interacable.rb.useGravity = preUseGravity;
        interacable.rb.isKinematic = preIsKinematic;

        gameObject.layer = 0;

        EyeManager.Instance.SetGrabbedBody(null);
    }
}
