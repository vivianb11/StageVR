using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace SignalSystem
{
    public class SignalEmitter : MonoBehaviour
    {
        [SerializeField] List<SO_Signal> signals = new List<SO_Signal>();

        [Button]
        public void RequestSignalCall()
        {
            foreach (SO_Signal signal in signals)
            {
                signal.Emit();
            }
        }
    }
}