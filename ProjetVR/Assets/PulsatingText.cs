using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class PulsatingText : MonoBehaviour
{
    public float minValueFontSize = 6f;
    public float maxValueFontSize = 12f;
    public float duration = 1f;

    private float FontSizeValue; //the variable that constantly changes up and down
    private bool increasing = true;
    private float timer = 0f;


    private void Update()
    {
        FontSizePulsating();
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

        gameObject.GetComponent<TextMesh>().characterSize = FontSizeValue;
    }
}
