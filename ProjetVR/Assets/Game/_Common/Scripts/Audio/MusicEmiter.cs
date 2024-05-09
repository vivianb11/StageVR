using System;
using UnityEngine;

public class MusicEmiter : MonoBehaviour
{
    [SerializeField] bool playMusicOnStart;

    public AudioClip[] musics;

    private SoundManager soundManager;

    void Start()
    {
        soundManager = SoundManager.Instance;

        if (musics.Length > 0)
        {
            Array.Clear(soundManager.musics, 0, soundManager.musics.Length);

            soundManager.musics = new Sound[musics.Length];

            for (int i = 0; i < musics.Length; i++)
            {
                soundManager.musics[i] = new Sound(musics[i], SoundType.Music);
            }
        }

        if (playMusicOnStart)
        {
            PlayMusic();
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
