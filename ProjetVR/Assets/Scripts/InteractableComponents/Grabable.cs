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

    [ShowIf("alignToNormal")]
    public AlignDirection alignDirection = AlignDirection.UP;

    [ShowIf("alignToNormal")]
    public float distanceToPoint = 0.5f;

    public enum MovementType
    {
        NORMAL, CHAIN
    }

    public MovementType movementType = MovementType.NORMAL;

    [ShowIf("movementType", MovementType.CHAIN)]
    [SerializeField]
    private float chainLength = 1f;

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

        switch (movementType)
        {
            case MovementType.NORMAL:
                if (alignToNormal)
                    AlignToNormal(normal);

                interacable.rb.velocity = Vector3.Lerp(interacable.rb.velocity, directionToTarget * moveSpeed, 1f);
                break;
            case MovementType.CHAIN:
                if (Vector3.Distance(transform.position, targetPos) > chainLength)
                    interacable.rb.velocity = Vector3.Lerp(interacable.rb.velocity, directionToTarget * moveSpeed, 1f);
                else
                    interacable.rb.velocity = Vector3.Lerp(interacable.rb.velocity, Vector3.zero, 0.5f);
                break;
        }
    }

    private void OnSelected()
    {
        interacable.rb.useGravity = false;
        interacable.rb.isKinematic = false;

        gameObject.layer = LayerMask.NameToLayer("Ignore");

        EyeManager.Instance.SetGrabbedBody(this);
    }

    private void OnDeselected()
    {
        interacable.rb.useGravity = true;

        gameObject.layer = 0;

        EyeManager.Instance.SetGrabbedBody(null);
    }
}
