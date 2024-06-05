using SignalSystem;
using System;
using System.Linq;
using UnityEngine;

public class MusicEmiterSwitcher : MonoBehaviour
{
    [Space(10)]
    [SerializeField] MusicEmiter musicEmiter;

    [Space(10)]
    public MusicOnSignal[] musicOnSignals;

    public bool SkinCheck, ModeCheck;

    public void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);

        if (musicEmiter == null) if (TryGetComponent(out musicEmiter)) Debug.Log("No Music Emiter Assigned or On Gameobject");

        if (SkinCheck) OnSignalReceived(JeuB.JeuBCommands.lastSkinSignal.name);
        if (ModeCheck) OnSignalReceived(JeuB.JeuBCommands.lastModeSignal.name);
    }

    protected virtual void OnSignalReceived(string value)
    {
        if (musicOnSignals.Where(x => x.signal.name == value).Count() < 1) value = JeuB.JeuBCommands.lastSkinSignal.name;

        if (JeuB.JeuBCommands.lastModeSignal.name == "SophoriqueMusic") return;

        foreach (MusicOnSignal musicOnSignal in musicOnSignals)
        {
            if (musicOnSignal.signal.name == value)
            {
                musicEmiter.musics = musicOnSignal.musics;

                musicEmiter.ResetMusics();
                musicEmiter.PlayMusic();

                return;
            }
        }
    }
}

[Serializable]
public class MusicOnSignal
{
    public SO_Signal signal;

    public AudioClip[] musics;
}
