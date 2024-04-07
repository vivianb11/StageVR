using UnityEngine;
using UnityEngine.Events;

namespace SignalSystem
{
    public class SignalManager : MonoBehaviour
    {
        public static SignalManager Instance { get; private set; }

        public UnityEvent<string> signalCalled = new UnityEvent<string>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /**
        * Emit a signal to all the listeners
        * @code{.cs} SignalListener.OnSignalReceived(string signalName) @endcode
        */
        public void EmitSignal(string signalName)
        {
            signalCalled?.Invoke(value);
        }
    }
}