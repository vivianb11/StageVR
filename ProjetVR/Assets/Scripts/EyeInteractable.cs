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

    public InteractionType interactionType;

    [ShowIf("interactionType", InteractionType.StareTime)]
    public float stareTime = 2f;

    [ShowIf("interactionType", InteractionType.BlinkTime)]
    public float blinkTime = 2f;

    [ShowIf("interactionType", InteractionType.BlinkAmount)]
    public int blinkAmount = 2;
}
