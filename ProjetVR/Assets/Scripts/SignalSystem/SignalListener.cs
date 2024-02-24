using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public SO_Signal[] signal;

    public UnityEvent signalReceived;

    public UnityEvent signalLost;

    private void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);
    }

    private void OnSignalReceived(string value)
    {
        foreach (SO_Signal eventName in signal)
        {
            if (eventName.signalName == value)
            {
                signalReceived?.Invoke();
                return;
            }
        }

        signalLost?.Invoke();
    }
}
