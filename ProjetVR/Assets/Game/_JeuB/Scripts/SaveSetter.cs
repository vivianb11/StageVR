using JeuB;
using SignalSystem;
using UnityEngine;

public class SaveSetter : MonoBehaviour
{
    public void SetlastMode(SO_Signal signal)
    {
        JeuBCommands.lastModeSignal = signal;
    }

    public void SetlastSkin(SO_Signal signal)
    {
        JeuBCommands.lastSkinSignal = signal;
    }
}
