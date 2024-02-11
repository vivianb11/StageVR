using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EyeInteractable : Interacable
{
    // temporaire a enlever
    public float blinkTime = 0.5f;

    private float timer = 0f;

    public override void Interact()
    {
        timer += Time.deltaTime;

        if (timer >+ blinkTime || timer >= blinkTime)
        {
            Select();
            timer = 0f;
        }
    }
}
