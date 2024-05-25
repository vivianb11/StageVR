using JeuB;
using SignalSystem;
using UnityEngine;

public class SaveSetter : MonoBehaviour
{
    JeuBCommands jeuBCommands;
    public void SetlastMode(SO_Signal signal)
    {
        jeuBCommands.lastModeSignal = signal;
    }

    public void SetlastSkin(SO_Signal signal)
    {
        jeuBCommands.lastSkinSignal = signal;
    }
}
