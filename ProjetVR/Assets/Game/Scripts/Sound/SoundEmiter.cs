using NaughtyAttributes;
using UnityEngine;

public class SoundEmiter : MonoBehaviour
{
    SoundManager soundManager;

    public SoundPlacing type = SoundPlacing.Local;
    public Sound sound = new Sound(default,SoundType.SFX);

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    [Button("Play Sound")]
    public void PlaySound()
    {
        soundManager.PlaySound(sound, type, transform);
    }
}
