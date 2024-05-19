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

    public void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);

        if (soundEmiter == null) if (TryGetComponent(out soundEmiter)) Debug.Log("No Sound Emiter Assigned or On Gameobject");
    }

    protected virtual void OnSignalReceived(string value)
    {
        foreach (SoundOnSignal soundOnSignal in soundOnSignals)
        {
            if (soundOnSignal.signal.name == value)
            {
                soundEmiter.playRandomSfx = soundOnSignal.randomize;

                soundEmiter.sound.soundType = soundOnSignal.sounds[0].soundType;

                if (soundOnSignal.randomize) soundEmiter.sfxs = soundOnSignal.sounds.Select(sound => sound.clip).ToArray();
                else
                {
                    soundEmiter.sound = soundOnSignal.sounds[0];
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

    public Sound[] sounds;
}
