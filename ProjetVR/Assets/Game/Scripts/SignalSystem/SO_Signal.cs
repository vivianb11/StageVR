using UnityEngine;
using UnityEngine.Events;

namespace SignalSystem
{
    [CreateAssetMenu(fileName = "New Signal", menuName = "ScriptableObject/Signal")]
    public class SO_Signal : ScriptableObject
    {
        public string signalName;

        public void Emit()
        {
            SignalManager.Instance.EmitSignal(signalName);
        }
    }
}
