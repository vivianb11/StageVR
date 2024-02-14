using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Grabable : MonoBehaviour
{
    public SelectType grabType;

    private Interactable interacable;

    private void Awake()
    {
        interacable = GetComponent<Interactable>();
        interacable.onSelected.AddListener(OnSelected);
        interacable.onDeselected.AddListener(OnDeselected);

        interacable.rb.isKinematic = true;
    }

    private void OnSelected()
    {
        interacable.rb.useGravity = false;

        switch (grabType)
        {
            case SelectType.EYE:
                EyeManager.Instance.SetGrabbedBody(interacable.rb);
                break;
            case SelectType.HAND:
                EyeManager.Instance.SetGrabbedBody(interacable.rb);
                break;
        }
    }

    private void OnDeselected()
    {
        interacable.rb.useGravity = true;
        EyeManager.Instance.SetGrabbedBody(null);
    }
}
