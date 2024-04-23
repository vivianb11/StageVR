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
    public float durationColor = 0.1f;

    private float FontSizeValue; //the variable that constantly changes up and down
    private bool increasing = true;
    private bool increasingColor = true;
    private float timer = 0f;
    private float timerColor = 0f;
    private float timerAppear = 0f;
    public bool colorActivated;

    public TextMesh textMesh;

    public bool appearAndDisappear;

    private void Start()
    {
        textMesh = gameObject.GetComponent<TextMesh>();
    }


    private void Update()
    {
        FontSizePulsating();

        if (colorActivated)
        {
            ColorPulsating();
        }

        if (appearAndDisappear)
        {
            AppearAndDisappear();
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

        gameObject.GetComponent<TextMesh>().characterSize = FontSizeValue;
    }

    public void ColorPulsating()
    {
        timerColor += Time.deltaTime;

        if (timerColor >= durationColor)
        {
            timerColor = 0f;
            increasingColor = !increasingColor;
        }

        Color newColor = textMesh.color; // Get the current color
        newColor.r = 1; // Modify the red component
        textMesh.color = newColor; // Assign the modified color back

        if (increasingColor)
        {
            gameObject.GetComponent<TextMesh>().color = Color.yellow;
        }
        else
        {
            gameObject.GetComponent<TextMesh>().color = Color.white;
        }
    }

    public void AppearAndDisappear()
    {
        timerColor += Time.deltaTime;

        if (timerColor >= durationColor)
        {
            timerColor = 0f;
            increasingColor = !increasingColor;
        }

        //1, 0.92, 0.016
        //1, 1, 1

        

        if (increasingColor)
        {
            gameObject.GetComponent<TextMesh>().color.r = 1;
        }
        else
        {
            gameObject.GetComponent<TextMesh>().color = Color.white;
        }
    }


}
