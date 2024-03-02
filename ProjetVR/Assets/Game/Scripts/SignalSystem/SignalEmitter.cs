using System.Collections.Generic;
using UnityEngine;

namespace SignalSystem
{
    public class SignalEmitter : MonoBehaviour
    {
        [SerializeField] List<SO_Signal> signal = new List<SO_Signal>();

        public void RequestSignalCall()
        {
            foreach (SO_Signal signal in signal)
            {
                SignalManager.Instance.EmitSignal(signal.signalName);
            }
        }
    }
}