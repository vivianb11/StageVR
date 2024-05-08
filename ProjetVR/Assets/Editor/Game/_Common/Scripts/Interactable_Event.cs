using UnityEngine;

public class Interactable_Event : MonoBehaviour
{
    Interactable interactable;

    private void Awake()
    {
        gameObject.TryGetComponent(out interactable);

        if (interactable != null)
        {
            interactable.onSelected.AddListener(OnSelected);
            interactable.onDeselected.AddListener(OnDeselected);
            interactable.lookIn.AddListener(OnLookIn);
            interactable.lookOut.AddListener(OnLookOut);
            interactable.activeStateChanged.AddListener(ActiveStateChanged);
        }
    }

    public virtual void OnSelected()
    {

    }

    public virtual void OnDeselected()
    {

    }

    public virtual void OnLookIn()
    {

    }

    public virtual void OnLookOut()
    {

    }
     public virtual void ActiveStateChanged(bool value)
    {

    }
}
