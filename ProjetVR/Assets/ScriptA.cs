using SignalSystem;
using UnityEngine;

public class ScriptA : MonoBehaviour
{
    [SerializeField] SO_Signal signal;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            signal.Emit();
        }
    }
}
