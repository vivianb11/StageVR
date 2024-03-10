using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PanelBehavior : MonoBehaviour
{
    public AudioSource dialogueAudioSource;
    public AudioSource sfxAudioSource;
    
    public TextMesh text;

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void SetText(string newText, float time)
    {
        StartCoroutine(SetTextWithTime(newText, time));
    }

    IEnumerator SetTextWithTime(string newText, float time)
    {
        foreach (var letter in newText)
        {
            text.text += letter;
            yield return new WaitForSeconds((time / newText.Length));
        }
    }

    public void PlaySFX(AudioClip audio)
    {
        sfxAudioSource.clip = audio;
        sfxAudioSource.Play();
    }

    public void PlayAudio(AudioClip audio)
    {
        dialogueAudioSource.clip = audio;
        dialogueAudioSource.Play();
    }

    public void StopAudio()
    {
        dialogueAudioSource.Stop();
    }

    public void StopAllAudio()
    {
        dialogueAudioSource.Stop();
        sfxAudioSource.Stop();
    }
}
