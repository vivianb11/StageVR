using UnityEngine;

namespace SignalSystem
{
    public class SignalEmitter : MonoBehaviour
    {
        [SerializeField] SO_Signal signal;

        public void RequestSignalCall()
        {
            SignalManager.Instance.EmitSignal(signal.signalName);
        }
    }
}