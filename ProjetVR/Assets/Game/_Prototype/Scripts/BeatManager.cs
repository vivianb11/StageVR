using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public AudioClip hbClip;
    private AudioSource audioSource;

    public float playSpeed = 1;

    public List<GameObject> visualFeedback = new();
    public float amplification = 50;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hbClip;
        audioSource.loop = true;

        audioSource.pitch = playSpeed;
    }

    [Button]
    public void ApplyChanges()
    {
        audioSource.pitch = playSpeed;
    }

    private void Update()
    {
        if (visualFeedback.Count > 0)
        {
            if (audioSource.isPlaying)
            {
                foreach (GameObject go in visualFeedback)
                {
                    // analyze the sound being played
                    float[] spectrum = new float[256];
                    audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
                    float sum = 0;
                    for (int i = 0; i < spectrum.Length; i++)
                    {
                        sum += spectrum[i];
                    }
                    float average = sum / spectrum.Length;

                    go.transform.localScale = Vector3.one + Vector3.one * average * 10 * amplification;
                }
            }
            else
            {
                foreach (GameObject go in visualFeedback)
                {
                    go.transform.localScale = Vector3.one;
                }
            }
        }
    }

    [Button]
    public void PlayHeartbeat()
    {
        audioSource.Play();
    }

    [Button]
    public void StopHeartbeat()
    {
        audioSource.Stop();
    }
}
