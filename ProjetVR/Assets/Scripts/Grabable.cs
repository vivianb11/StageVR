using UnityEngine;

[RequireComponent(typeof(Interacable))]
[RequireComponent(typeof(Rigidbody))]
public class Grabable : MonoBehaviour
{
    public GrabType grabType;

    private Interacable interacable;

    private void Awake()
    {
        interacable = GetComponent<Interacable>();
        interacable.onSelected.AddListener(OnSelected);
        interacable.onDeselected.AddListener(OnDeselected);
    }

    private void OnSelected()
    {
        interacable.rb.useGravity = false;

        switch (grabType)
        {
            case GrabType.EYE:
                EyeRaycaster.Instance.SetGrabbedBody(interacable.rb);
                break;
            case GrabType.HAND:
                EyeRaycaster.Instance.SetGrabbedBody(interacable.rb);
                break;
        }
    }

    private void OnDeselected()
    {
        interacable.rb.useGravity = true;
        EyeRaycaster.Instance.SetGrabbedBody(null);
    }
}
