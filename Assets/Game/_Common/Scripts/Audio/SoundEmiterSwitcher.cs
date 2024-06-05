using SignalSystem;
using System;
using System.Linq;
using UnityEngine;

public class SoundEmiterSwitcher : MonoBehaviour
{
    [Space(10)]
    [SerializeField] SoundEmiter soundEmiter;

    [Space(10)]
    public SoundOnSignal[] soundOnSignals;

    public bool SkinCheck, ModeCheck;

    public void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);

        if (soundEmiter == null) if (TryGetComponent(out soundEmiter)) Debug.Log("No Sound Emiter Assigned or On Gameobject");

        if (soundEmiter == null) return;

        if (SkinCheck) OnSignalReceived(JeuB.JeuBCommands.lastSkinSignal.name);
        if (ModeCheck) OnSignalReceived(JeuB.JeuBCommands.lastModeSignal.name);
    }

    protected virtual void OnSignalReceived(string value)
    {
        // checks if in the soundOnSignals array a certain signal is present
        if (soundOnSignals.Where(x => x.signal.name == value).Count() < 1) value = JeuB.JeuBCommands.lastSkinSignal.name;

        if (JeuB.JeuBCommands.lastModeSignal.name == "SophoriqueMusic") return;

        foreach (SoundOnSignal soundOnSignal in soundOnSignals)
        {
            if (soundOnSignal.signal.name == value)
            {
                soundEmiter.playRandomSfx = soundOnSignal.randomize;

                soundEmiter.sound.soundType = soundOnSignal.soundType;

                if (soundOnSignal.randomize) soundEmiter.sfxs = soundOnSignal.sounds;
                else
                {
                    if (soundOnSignal.sounds == null || soundOnSignal.sounds.Length < 1) soundEmiter.sound = null;
                    else soundEmiter.sound.clip = soundOnSignal.sounds[0];

                    return;
                }
            }
        }
    }
}

[Serializable]
public class SoundOnSignal
{
    public SO_Signal signal;

    public bool randomize;
    public SoundType soundType;

    public AudioClip[] sounds;
}
