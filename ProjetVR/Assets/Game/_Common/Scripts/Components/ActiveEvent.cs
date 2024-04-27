using UnityEngine;
using UnityEngine.Events;

public class ActiveEvent : MonoBehaviour
{
    public UnityEvent OnActivated;

    public UnityEvent OnDeactivated;

    private void OnEnable()
    {
        OnActivated?.Invoke();
    }

    private void OnDisable()
    {
        OnDeactivated?.Invoke();
    }
}
