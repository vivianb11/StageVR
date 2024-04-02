using UnityEngine;

namespace SignalSystem
{
    [CreateAssetMenu(fileName = "New Signal", menuName = "ScriptableObject/Signal")]
    public class SO_Signal : ScriptableObject
    {
        public void Emit()
        {
            SignalManager.Instance.EmitSignal(name);
        }
    }
}
