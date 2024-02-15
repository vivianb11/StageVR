using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Grabable : MonoBehaviour
{
    public SelectType grabType;

    private Interactable interacable;

    public float moveSpeed = 2f;

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

    public void MoveTo(Vector3 targetPos)
    {
        Vector3 directionToTarget = targetPos - transform.position;

        switch (movementType)
        {
            case MovementType.NORMAL:
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

        gameObject.layer = LayerMask.NameToLayer("Ignore");

        switch (grabType)
        {
            case SelectType.EYE:
                EyeManager.Instance.SetGrabbedBody(this);
                break;
            case SelectType.HAND:
                EyeManager.Instance.SetGrabbedBody(this);
                break;
        }
    }

    private void OnDeselected()
    {
        interacable.rb.useGravity = true;
        gameObject.layer = 0;

        EyeManager.Instance.SetGrabbedBody(null);
    }
}
