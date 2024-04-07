/**
 * @file SignalListener.cs
 * @author Raphael Daubelcour
 * @date 2023
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SignalSystem
{
    public class SignalListener : MonoBehaviour
    {
        /**
         * @category Test
         */

        /**
         * List of signals the listener will react as successfull
         */
        public List<SO_Signal> signal = new List<SO_Signal>();

        /**
         * Event invoke when the signal received is in the signal variable
         */
        public UnityEvent signalReceived = new UnityEvent();

        /**
         * Event invoke when the signal received is not in the signal variable
         */
        public UnityEvent signalLost = new UnityEvent();

        private void Start()
        {
            SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);
        }

        /**
        * Check if the signalName is in the signal variable
        */
        protected void OnSignalReceived(string signalName)
        {
            foreach (SO_Signal soSignal in signal)
            {
                if (soSignal.name == signalName)
                {
                    signalReceived?.Invoke();
                    return;
                }
            }

            signalLost?.Invoke();
        }
    }
}
