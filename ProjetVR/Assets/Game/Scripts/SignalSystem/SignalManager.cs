using UnityEngine;
using UnityEngine.Events;

namespace SignalSystem
{
    public class SignalManager : MonoBehaviour
    {
        public static SignalManager Instance { get; private set; }

        public UnityEvent<string> signalCalled;

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

        public void EmitSignal(string value)
        {
            signalCalled?.Invoke(value);
        }
    }
}