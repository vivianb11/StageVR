using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackOpacity : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Start()
    {
        Interactable interactable = GetComponent<Interactable>();

        interactable.activeStateChanged.AddListener(OnActiveStateChanged);
    }

    private void OnActiveStateChanged(bool value)
    {
        Color color = meshRenderer.material.color;

        color.a = value ? 1f : 0.5f;
        
        meshRenderer.material.color = value ?  Color.green : Color.red;
    }
}
