using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PanelBehavior : MonoBehaviour
{
    public AudioSource dialogueAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip textAudio;

    public Coroutine textCoroutine;

    public TextMesh text;

    private void Awake()
    {
        dialogueAudioSource.Stop();
        sfxAudioSource.Stop();
    }

    public void SetText(string newText)
    {
        ResetText();

        text.text = newText;
    }

    public void SetText(string newText, float time)
    {
        ResetText();

        textCoroutine = StartCoroutine(SetTextWithTime(newText, time));
    }

    IEnumerator SetTextWithTime(string newText, float time)
    {
        foreach (var letter in newText)
        {
            text.text += letter;
            
            if (textAudio != null)
                PlaySFX(textAudio);

            yield return new WaitForSeconds((time / newText.Length));
        }
        
        textCoroutine = null;
    }

    public void ShakeText() 
    {
        InvokeRepeating("ShakeTextCoroutine", 0, 0.01f);
    }

    IEnumerator ShakeTextCoroutine(float fortime)
    {
        Vector3 originalPos = text.transform.position;
        float time = 0;
        while (time < fortime)
        {
            text.transform.position = originalPos + Random.insideUnitSphere * 0.1f;
            time += Time.deltaTime;
            yield return null;
        }
        text.transform.position = originalPos;
    }

    public void ResetText()
    {
        if (textCoroutine != null)
            StopCoroutine(textCoroutine);
        else
            text.text = "";
    }

    public void PlaySFX(AudioClip audio)
    {
        sfxAudioSource.clip = audio;
        sfxAudioSource.Play();
    }

    public void PlayAudio(AudioClip audio)
    {
        if (dialogueAudioSource.isPlaying)
            ForceStopAudio();

        dialogueAudioSource.clip = audio;
        dialogueAudioSource.Play();
    }

    public void StopAudio()
    {
        StartCoroutine(AudioFade(dialogueAudioSource));
    }

    public void StopSFX()
    {
        StartCoroutine(AudioFade(sfxAudioSource));
    }

    public void StopAllAudio()
    {
        StartCoroutine(AudioFade(dialogueAudioSource));
        StartCoroutine(AudioFade(sfxAudioSource));
    }

    public void ForceStopAudio()
    {
        dialogueAudioSource.volume = 0;
    }

    public void ForceStopSFX()
    {
        sfxAudioSource.volume = 0;
    }

    public void ForceStopAllAudio()
    {
        dialogueAudioSource.volume = 0;
        sfxAudioSource.volume = 0;
    }

    IEnumerator AudioFade(AudioSource source)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }
}
