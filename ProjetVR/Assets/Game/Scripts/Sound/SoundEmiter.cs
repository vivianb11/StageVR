
using UnityEngine;

public class SoundEmiter : MonoBehaviour
{
    SoundManager soundManager;

    public SoundPlacing place = SoundPlacing.Local;
    public Sound sound = new Sound(default,SoundType.SFX);

    [HideInInspector] public int lastPlayedSound = 0;

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    public void PlaySound()
    {
        lastPlayedSound = soundManager.PlaySound(sound, place, transform);
    }

    public void ResumeSound(bool fade)
    {
        soundManager.ResumeSound(lastPlayedSound, fade);
    }

    public void PauseSound(bool fade)
    {
        soundManager.PauseSound(lastPlayedSound, fade);
    }

    public void StopSound(bool fade)
    {
        soundManager.StopSound(lastPlayedSound, fade);
    }


}
