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
    }

    private void OnSelected()
    {
        switch (grabType)
        {
            case GrabType.EYE:
                EyeRaycaster.Instance.SetFollow(interacable.rb);
                break;
            case GrabType.HAND:
                EyeRaycaster.Instance.SetFollow(interacable.rb);
                break;
        }
    }
}
