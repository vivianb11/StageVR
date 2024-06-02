using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SignalSystem
{
    public class SignalListener : MonoBehaviour
    {
        public List<SO_Signal> signal = new List<SO_Signal>();

        public UnityEvent signalReceived = new UnityEvent();
        public UnityEvent signalLost = new UnityEvent();

        private void Start()
        {
            SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);
        }

        protected virtual void OnSignalReceived(string value)
        {
            foreach (SO_Signal soSignal in signal)
            {
                if (soSignal.name == value)
                {
                    signalReceived?.Invoke();
                    return;
                }
            }

            signalLost?.Invoke();
        }
    }
}
