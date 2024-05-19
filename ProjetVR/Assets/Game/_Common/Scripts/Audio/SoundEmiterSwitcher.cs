using SignalSystem;
using System;
using System.Linq;
using UnityEngine;

public class SoundEmiterSwitcher : MonoBehaviour
{
    [Space(10)]
    [SerializeField] SoundEmiter SoundEmiter;

    [Space(10)]
    public SoundOnSignal[] soundOnSignals;

    public void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);

        if (SoundEmiter == null) if (TryGetComponent(out SoundEmiter)) Debug.Log("No Sound Emiter Assigned or On Gameobject");
    }

    protected virtual void OnSignalReceived(string value)
    {
        foreach (SoundOnSignal soundOnSignal in soundOnSignals)
        {
            if (soundOnSignal.signal.name == value)
            {
                SoundEmiter.playRandomSfx = soundOnSignal.randomize;

                SoundEmiter.sound.soundType = soundOnSignal.sounds[0].soundType;

                if (soundOnSignal.randomize) SoundEmiter.sfxs = soundOnSignal.sounds.Select(sound => sound.clip).ToArray();
                else
                {
                    SoundEmiter.sound = soundOnSignal.sounds[0];
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
