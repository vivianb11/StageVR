using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SignalSystem
{
    public class SignalListener : MonoBehaviour
    {
        public List<SO_Signal> signal = new List<SO_Signal>();

        public UnityEvent signalReceived = new UnityEvent();

        private void Start()
        {
            SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);
        }

        private void OnSignalReceived(string value)
        {
            foreach (SO_Signal soSignal in signal)
            {
                if (soSignal.signalName == value)
                {
                    signalReceived?.Invoke();
                    return;
                }
            }
        }
    }
}
