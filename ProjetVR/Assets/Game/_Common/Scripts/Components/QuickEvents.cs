using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class QuickEvents : MonoBehaviour
{
    bool AutoDisableAfterEvent = false;
    [ShowIf("AutoDisableAfterEvent")]
    bool DestroyComponentAfterEvent = false;

    [Header("On object :")]
    public bool enable;
    public bool disable, destroy, applicationQuit, applicationPause, applicationFocus;

    [Header("Events :")]
    [ShowIf("enable")]
    public UnityEvent OnEnableEvent;
    [ShowIf("disable")]
    public UnityEvent OnDisableEvent;
    [ShowIf("destroy")]
    public UnityEvent OnDestroyEvent;
    [ShowIf("applicationQuit")]
    public UnityEvent OnApplicationQuitEvent;
    [ShowIf("applicationPause")]
    public UnityEvent OnApplicationPauseEvent;
    [ShowIf("applicationFocus")]
    public UnityEvent OnApplicationFocusEvent;

    private void OnEnable()
    {
        if (!enable) return;

        OnEnableEvent?.Invoke();

        if (AutoDisableAfterEvent)
            enabled = false;

        if (CheckIfAllEventsCalled())
            Destroy(this);
    }

    private void OnDisable()
    {
        if (!disable) return;

        OnDisableEvent?.Invoke();

        if (AutoDisableAfterEvent)
            enabled = false;

        if (CheckIfAllEventsCalled())
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (!destroy) return;

        OnDestroyEvent?.Invoke();

        if (AutoDisableAfterEvent)
            enabled = false;

        if (CheckIfAllEventsCalled())
            Destroy(this);
    }

    private void OnApplicationQuit()
    {
        if (applicationQuit)
            OnApplicationQuitEvent.Invoke();

        if (AutoDisableAfterEvent) applicationQuit = false;

        if (CheckIfAllEventsCalled())
            Destroy(this);
    }

    private void OnApplicationPause(bool pause)
    {
        if (applicationPause)
            OnApplicationPauseEvent.Invoke();

        if (AutoDisableAfterEvent) applicationPause = false;

        if (CheckIfAllEventsCalled())
            Destroy(this);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (applicationFocus)
            OnApplicationFocusEvent.Invoke();

        if (AutoDisableAfterEvent) applicationFocus = false;

        if (CheckIfAllEventsCalled())
            Destroy(this);
    }

    public bool CheckIfAllEventsCalled()
    {
        if (enable && disable && destroy && applicationQuit && applicationPause && applicationFocus)
            return DestroyComponentAfterEvent;
        else
            return false;
    }
}
