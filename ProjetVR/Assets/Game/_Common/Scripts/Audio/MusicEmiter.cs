using System;
using UnityEngine;

public class MusicEmiter : MonoBehaviour
{
    public AudioClip[] musics;
    private SoundManager soundManager;

    void Start()
    {
        soundManager = SoundManager.instance;

        if (musics.Length > 0)
        {
            Array.Clear(soundManager.musics, 0, soundManager.musics.Length);

            soundManager.musics = new Sound[musics.Length];

            for (int i = 0; i < musics.Length; i++)
            {
                soundManager.musics[i] = new Sound(musics[i], SoundType.Music);
            }
        }
    }


    public void PlayMusic()
    {
        soundManager.PlayMusic();
    }

    public void PlayMusic(AudioClip music)
    {
        soundManager.PlayMusic(music);
    }

    public void PauseMusic()
    {
        soundManager.PauseMusic();
    }
}
