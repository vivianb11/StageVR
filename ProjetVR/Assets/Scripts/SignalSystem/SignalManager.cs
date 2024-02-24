using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalManager : MonoBehaviour
{
    public static SignalManager Instance { get; private set; }

    public UnityEvent<string> signalCalled;

    private void Awake()
    {
        Instance = this;
    }

    public void EmitSignal(string value)
    {
        signalCalled?.Invoke(value);
    }
}
