using SignalSystem;
using System;
using UnityEngine;

public class MusicEmiterSwitcher : MonoBehaviour
{
    [Space(10)]
    [SerializeField] MusicEmiter musicEmiter;

    [Space(10)]
    public MusicOnSignal[] musicOnSignals;

    public void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);

        if (musicEmiter == null) if (TryGetComponent(out musicEmiter)) Debug.Log("No Music Emiter Assigned or On Gameobject");
    }

    protected virtual void OnSignalReceived(string value)
    {
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
