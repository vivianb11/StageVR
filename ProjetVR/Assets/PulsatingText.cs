using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PulsatingText : MonoBehaviour
{
    public float minValueFontSize = 6f;
    public float maxValueFontSize = 12f;

    public float minValueOpacity = 0f;
    public float maxValueOpacity = 1f;

    public float duration = 1f;
    public float durationColor = 0.1f;
    public float durationAppear = 3f;

    private float FontSizeValue; //the variable that constantly changes up and down
    private bool increasing = true;
    private bool increasingColor = true;
    private float timer = 0f;
    private float timerColor = 0f;
    public bool colorActivated;

    public TextMesh textMesh;

    public bool disappear;

    public UnityEvent onAppear = new UnityEvent();

    private void Start()
    {
        textMesh = gameObject.GetComponent<TextMesh>();
    }


    private void Update()
    {
        FontSizePulsating();


        if (disappear)
        {

            Disappear();
        }




    }

    public void FontSizePulsating()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            timer = 0f;
            increasing = !increasing;
        }

        if (increasing)
        {
            FontSizeValue = Mathf.Lerp(minValueFontSize, maxValueFontSize, timer / duration);
        }
        else
        {
            FontSizeValue = Mathf.Lerp(maxValueFontSize, minValueFontSize, timer / duration);
        }

        textMesh.characterSize = FontSizeValue;
    }

    public void ColorPulsating()
    {
        timerColor += Time.deltaTime;

        if (timerColor >= durationColor)
        {
            timerColor = 0f;
            increasingColor = !increasingColor;
        }


        if (increasingColor)
        {
            Color newColor = textMesh.color;
            newColor.r = 1;
            newColor.g = 0.92f;
            newColor.b = 0.016f;
            textMesh.color = newColor;
        }
        else
        {
            Color newColor = textMesh.color;
            newColor.r = 1;
            newColor.g = 1;
            newColor.b = 1;
            textMesh.color = newColor;
        }
    }

    public void Disappear()
    {
        Color newColor = textMesh.color;
        newColor.a = 1;
        textMesh.color = newColor;

        onAppear.Invoke();

        StartCoroutine(FadeOut());
        disappear = false;

    }

    IEnumerator FadeOut()
    {
        Color originalColor = textMesh.color;
        float timer = 0f;

        while (timer < 3f)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, timer / 3f); // Interpolate alpha from current alpha to 0
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            textMesh.color = newColor;
            timer += Time.deltaTime;
            ColorPulsating();
            yield return null;
        }

        // Ensure alpha is exactly 0 when the loop finishes
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    //public void Disappear()
    //{

    //    Color newColor = textMesh.color;
    //    newColor.a = OpacitySizeValue;
    //    textMesh.color = newColor;
    //}


}
