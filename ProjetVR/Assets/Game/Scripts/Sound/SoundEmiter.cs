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

    public void ResumeSound(bool fade = false)
    {
        soundManager.ResumeSound(lastPlayedSound, fade);
    }

    public void PauseSound(bool fade = false)
    {
        soundManager.PauseSound(lastPlayedSound, fade);
    }

    public void StopSound(bool fade = false)
    {
        if (lastPlayedSound < 0)
        {
            soundManager.SkipMusic();
            return;
        }

        soundManager.StopSound(lastPlayedSound, fade);
    }
}
