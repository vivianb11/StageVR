using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EyeInteractable : Interacable
{
    public enum InteractionType
    {
        StareTime,BlinkTime,BlinkAmount
    }  

    public InteractionType interactionEnterType;
    public InteractionType interactionQuitType;

    [ShowIf("interactionType", InteractionType.StareTime)]
    public float stareTime = 2f;

    [ShowIf("interactionType", InteractionType.BlinkTime)]
    public float blinkTime = 2f;

    [ShowIf("interactionType", InteractionType.BlinkAmount)]
    public int blinkAmount = 2;

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
