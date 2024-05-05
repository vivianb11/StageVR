using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuickEvents : MonoBehaviour
{
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
        if (enable)
            OnEnableEvent.Invoke();
    }

    private void OnDisable()
    {
        if (disable)
            OnDisableEvent.Invoke();
    }

    private void OnDestroy()
    {
        if (destroy)
            OnDestroyEvent.Invoke();
    }

    private void OnApplicationQuit()
    {
        if (applicationQuit)
            OnApplicationQuitEvent.Invoke();
    }

    private void OnApplicationPause(bool pause)
    {
        if (applicationPause)
            OnApplicationPauseEvent.Invoke();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (applicationFocus)
            OnApplicationFocusEvent.Invoke();
    }
}
