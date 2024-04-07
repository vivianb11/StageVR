/**
 * @file SignalEmitter.cs
 * @author Raphael Daubelcour
 * @date 2023
 */

using System.Collections.Generic;
using UnityEngine;

namespace SignalSystem
{
    public class SignalEmitter : MonoBehaviour
    {
        [SerializeField] List<SO_Signal> signals = new List<SO_Signal>();

        /**
         * Request for every signal to be emitted by the Signal Manager.
         */

        public void RequestSignalCall()
        {
            foreach (SO_Signal signal in signals)
            {
                signal.Emit();
            }
        }
    }
}