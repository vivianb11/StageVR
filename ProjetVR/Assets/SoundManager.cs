using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [Header("General")]
    public static SoundManager instance;
    [SerializeField] AudioFilterMode audioFilterMode;

    [SerializeField] float fadeDuration = 1f;

    [SerializeField] int maxAudioSources = 10;

    [Header("Music")]
    [SerializeField] bool playMusicOnStart;
    [SerializeField] Sound[] musics;
    private AudioSource _musicSource;

    // The key is the audio source and the value is the sound that is playing on it
    private Dictionary<AudioSource,Sound> _audioSources;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        if (this.TryGetComponent<AudioSource>(out _musicSource) == false)
            _musicSource = this.gameObject.AddComponent<AudioSource>();

        _audioSources = new Dictionary<AudioSource, Sound>();

        for (int i = 0; i < maxAudioSources; i++)
        {
            var go = Instantiate(new GameObject("AudioSource "+i));
            go.transform.SetParent(transform);

            var aS = go.AddComponent<AudioSource>();
            
            _audioSources.Add(aS, null);
        }

        if (playMusicOnStart)
        {
            PlayMusic();
        }
    }

    public int PlaySound(Sound sound, SoundPlacing soundPlacing = SoundPlacing.Global, Transform location = null)
    {
        if (sound.soundType == SoundType.Music)
        {
            PlayMusic(sound.clip);
            return 0;
        }

        AudioSource audioSource = null;
        float clossestPercentage = 0;

        // Finds the audio source to play the sound
        foreach (var audio in _audioSources)
        {
            var aS = audio.Key;
            var s = audio.Value;

            if (!aS.isPlaying && s is null || s.isPaused && s.timePaused >= AudioData.expirationTime[s.soundType])
            {
                audioSource = aS;
                break;
            }
            else
            {
                if (audioFilterMode == AudioFilterMode.ClossestToFinishByType && audio.Value.soundType != sound.soundType)
                    continue;

                var percentage = aS.time / aS.clip.length;

                if (percentage > clossestPercentage)
                {
                    clossestPercentage = percentage;
                    audioSource = aS;
                }
            }
        }

        if (audioSource is null)
            Debug.LogWarning("No audio source available to play the sound");

        _audioSources[audioSource] = sound;

        switch (soundPlacing)
        {
            case SoundPlacing.Global:
                audioSource.transform.position = Vector3.zero;
                audioSource.spatialBlend = 0;
                break;
            case SoundPlacing.Local:
                audioSource.transform.position = location.position;
                audioSource.spatialBlend = 1;
                break;
        }

        audioSource.volume = AudioData.volumes[sound.soundType];
        audioSource.clip = sound.clip;
        audioSource.Play();

        return Array.IndexOf(_audioSources.Keys.ToArray(), audioSource);
    }

    [Button("Play Music")]
    public void PlayMusic()
    {
        // sorts the musics by the times played
        Array.Sort(musics, (x, y) => x.timePaused.CompareTo(y.timePaused));

        int smallestTimesPlayed = musics[0].timePaused;

        // selects a random music from the musics with the smallest times played
        List<Sound> smallestTimesPlayedMusics = new List<Sound>();
        foreach (var music in musics)
        {
            if (music.timePaused == smallestTimesPlayed)
                smallestTimesPlayedMusics.Add(music);
        }

        int randomIndex = Random.Range(0, smallestTimesPlayedMusics.Count);
        Sound selectedMusic = smallestTimesPlayedMusics[randomIndex];
        musics[randomIndex].timePaused++;

        StartCoroutine(PlayMusicCo(selectedMusic.clip));
    }
    public void PlayMusic(AudioClip music)
    {
        StartCoroutine(PlayMusicCo(music));
    }
    [Button("Resume Music")]
    public void ResumeMusic()
    {
        _musicSource.Play();
    }
    [Button("Pause Music")]
    public void PauseMusic()
    {
        _musicSource.Pause();
    }
    [Button("Skip Music")]
    public void SkipMusic()
    {
        _musicSource.time = _musicSource.clip.length - 0.01f;
    }

    public void PauseSound(int index, bool withFade = false)
    {
        var aS = _audioSources.Keys.ToArray()[index];

        if (withFade && aS.isPlaying)
        {
            StartCoroutine(FadeOutSound(aS, fadeDuration));
            _audioSources[aS].isPaused = true;
        }
        else if (aS.isPlaying)
        {
            aS.Pause();
            _audioSources[aS].isPaused = true;
        }
    }

    public void ResumeSound(int index, bool withFade = false)
    {
        var aS = _audioSources.Keys.ToArray()[index];

        if (withFade && !aS.isPlaying)
        {
            StartCoroutine(FadeInSound(aS, fadeDuration, AudioData.volumes[_audioSources[aS].soundType]));
            _audioSources[aS].isPaused = false;
        }
        else if (!aS.isPlaying)
        {
            aS.Play();
            _audioSources[aS].isPaused = false;
        }
    }

    public void StopSound(int index, bool withFade = false)
    {
        var aS = _audioSources.Keys.ToArray()[index];

        if (withFade && aS.isPlaying)
        {
            StartCoroutine(FadeOutSound(aS, fadeDuration));
            _audioSources[aS] = null;
        }
        else if (aS.isPlaying)
        {
            aS.Stop();
            _audioSources[aS] = null;
        }
    }

    public void PauseSound(SoundType soundType, bool withFade = false)
    {
        foreach (var audio in _audioSources)
        {
            var aS = audio.Key;
            var sound = audio.Value;

            if (sound is not null && sound.soundType == soundType)
            {
                if (withFade && aS.isPlaying)
                {
                    StartCoroutine(FadeOutSound(aS, fadeDuration));
                    sound.isPaused = true;
                }
                else if (aS.isPlaying)
                {
                    aS.Pause();
                    sound.isPaused = true;
                }
            }
        }
    }

    public void ResumeSound(SoundType soundType, bool withFade = false)
    {
        foreach (var audio in _audioSources)
        {
            var aS = audio.Key;
            var sound = audio.Value;

            if (sound is not null && sound.soundType == soundType)
            {
                if (withFade && !aS.isPlaying)
                {
                    StartCoroutine(FadeInSound(aS, fadeDuration, AudioData.volumes[soundType]));
                    sound.isPaused = false;
                }
                else if (!aS.isPlaying)
                {
                    aS.Play();
                    sound.isPaused = false;
                }
            }
        }
    }

    public void StopSound(SoundType soundType, bool withFade = false)
    {
        foreach(var audio in _audioSources)
        {
            var aS = audio.Key;
            var sound = audio.Value;

            if (sound is not null && sound.soundType == soundType)
            {
                if (withFade && aS.isPlaying)
                {
                    StartCoroutine(FadeOutSound(aS, fadeDuration));
                    _audioSources[aS] = null;
                }
                else if (aS.isPlaying)
                {
                    aS.Stop();
                    _audioSources[aS] = null;
                }
            }
        }
    }

    // Needs to be called by the UI when the sound settings are changed or when the sound is changed
    public void SoundChanged()
    {
        foreach (var audio in _audioSources)
        {
            var aS = audio.Key;
            var sound = audio.Value;

            if (sound != null)
            {
                aS.volume = AudioData.volumes[sound.soundType];
            }
        }

        _musicSource.volume = AudioData.volumes[SoundType.Music];
    }

    public IEnumerator PlayMusicCo(AudioClip music)
    {
        if (_musicSource.isPlaying)
            yield return FadeOutSound(_musicSource, fadeDuration);

        _musicSource.clip = music;
        _musicSource.Play();

        yield return new WaitWhile(() => _musicSource.time < _musicSource.clip.length);

        PlayMusic();
    }
    public IEnumerator FadeInSound(AudioSource source, float duration, float targetVolume)
    {
        source.volume = 0;
        source.Play();

        while (source.volume < targetVolume)
        {
            source.volume += Time.deltaTime / duration;

            yield return null;
        }

        source.volume = targetVolume;
    }
    public IEnumerator FadeOutSound(AudioSource source,float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;

            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }
}

[Serializable]
public class Sound
{
    public SoundType soundType;

    public AudioClip clip;

    [HideInInspector] public int timePaused;

    private bool _isPaused;
    public bool isPaused
    {
        get => _isPaused;
        set
        {
            _isPaused = value;

            if (value)
                TimePaused();
        }
    }
    [HideInInspector] public float pausedTime;

    public Sound(AudioClip sound, SoundType type = SoundType.None)
    {
        clip = sound;
        soundType = type;
        timePaused = 0;
    }
    public void ResetTimesPlayed()
    {
        timePaused = 0;
    }
    public IEnumerator TimePaused()
    {
        pausedTime = 0;

        while (isPaused && pausedTime < AudioData.expirationTime[soundType])
        {
            pausedTime += Time.deltaTime;

            yield return null;
        }
    }
}

public enum SoundPlacing
{
    Global,
    Local
}

public enum SoundType
{
    None,
    Music,
    SFX,
    Voice
}

public enum AudioFilterMode
{
    ClossestToFinish,
    ClossestToFinishByType
}