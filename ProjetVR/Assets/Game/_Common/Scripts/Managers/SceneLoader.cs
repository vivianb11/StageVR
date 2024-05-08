using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] MeshRenderer rend;
    [SerializeField] float fadeTransitionDuration = 2.0f;

    private bool loadRequested = false;

    public UnityEvent fadeInCompleted;
    public UnityEvent fadeOutCompleted;

    private void Awake()
    {
        Instance = this;
        rend.material.SetFloat("_force", 1f);
    }

    public void LodScene(int  sceneId)
    {
        if (loadRequested)
            return;
        
        loadRequested = true;
        //StartCoroutine(Fade(1f, fadeTransitionDuration, true));
        StartCoroutine(LoadDelay(fadeTransitionDuration, sceneId));
    }

    public void FadeIn(float fadeDuration)
    {
        StartCoroutine(Fade(1f, fadeDuration, true));
    }

    public void FadeOut(float fadeDuration)
    {
        StartCoroutine(Fade(0f, fadeDuration, false));
    }

    private IEnumerator LoadDelay(float delay, int sceneId)
    {
        yield return new WaitForSeconds(delay);
        loadRequested = false;
        StartCoroutine(Fade(0f, fadeTransitionDuration, true));
    }

    public IEnumerator Fade(float fadeTarget, float fadeDuration, bool invokeFadeInEvent)
    {
        float time = 0;
        float startAlpha = rend.material.GetFloat("_force");

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            rend.material.SetFloat("_force", Mathf.Lerp(startAlpha, fadeTarget, time / fadeDuration));
            yield return null;
        }

        if (invokeFadeInEvent)
            fadeInCompleted?.Invoke();
        else
            fadeOutCompleted?.Invoke();
    }
}
