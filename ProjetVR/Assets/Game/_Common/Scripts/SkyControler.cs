using System.Collections;
using UnityEngine;

public class SkyControler : MonoBehaviour
{
    private Material skyMaterial;
    private HeadMotionTracker headMotionTracker;

    public float tiltAmount = 35f;
    public float blendSpeed = 1f;

    private void Awake()
    {
        skyMaterial = RenderSettings.skybox;
    }

    private void OnDisable()
    {
        skyMaterial.SetFloat("_Blend", 0);
    }

    private void Start()
    {
        headMotionTracker = HeadMotionTracker.Instance;
        InvokeRepeating("ChangeSky", 0, 1);
    }

    private void ChangeSky()
    {
        if(headMotionTracker.GetTilt > tiltAmount)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeSkyTexture(true));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ChangeSkyTexture(false));
        }
    }

    IEnumerator ChangeSkyTexture(bool blend)
    {
        float t = skyMaterial.GetFloat("_Blend");

        if( blend )
        {
            while(t < 1)
            {
                t += Time.deltaTime * blendSpeed;
                skyMaterial.SetFloat("_Blend", t);
                yield return null;
            }
        }
        else
        {
            while(t > 0)
            {
                t -= Time.deltaTime * blendSpeed;
                skyMaterial.SetFloat("_Blend", t);
                yield return null;
            }
        }
    }
}
