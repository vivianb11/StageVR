using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] MeshRenderer rend;
    [SerializeField] float fadeTransitionDuration = 2.0f;

    private bool loadRequested = false;

    private void Awake()
    {
        Instance = this;
        rend.sharedMaterial.SetFloat("_force", 0f);
    }

    public void LodScene(int  sceneId)
    {
        if (loadRequested)
            return;
        
        loadRequested = true;
        StartCoroutine(Fade(1f, fadeTransitionDuration));
        StartCoroutine(LoadDelay(fadeTransitionDuration, sceneId));
    }

    private IEnumerator LoadDelay(float delay, int sceneId)
    {
        yield return new WaitForSeconds(delay);
        loadRequested = false;
        StartCoroutine(Fade(0f, fadeTransitionDuration));
    }

    public IEnumerator Fade(float fadeTarget, float fadeDuration)
    {
        float time = 0;
        float startAlpha = rend.sharedMaterial.GetFloat("_force");

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            rend.sharedMaterial.SetFloat("_force", Mathf.Lerp(startAlpha, fadeTarget, time / fadeDuration));
            yield return null;
        }
    }
}
