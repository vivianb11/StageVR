using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialChanger : MonoBehaviour
{
    private Renderer rend;
    private SpriteRenderer spriteRend;

    public enum ChangeType 
    {
        Material,
        Sprite
    }

    [SerializeField] ChangeType changeType;

    [ShowIf("changeType", ChangeType.Material)]
    [SerializeField] Material[] materials = new Material[1];

    [ShowIf("changeType", ChangeType.Sprite)]
    [SerializeField] Sprite[] spritesArray = new Sprite[1];

    private void Start()
    {
        if (changeType is ChangeType.Material) rend = GetComponent<Renderer>();
        else if (changeType is ChangeType.Sprite) spriteRend = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        // TryGetRenderers();
        ChangeInEditor();
    }

    // private void TryGetRenderers()
    // {
    //     if (rend is not null && spriteRend is not null) return;
        
    //     if (changeType is ChangeType.Sprite) TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRend);
    //     else if (changeType is ChangeType.Material) TryGetComponent<Renderer>(out Renderer rend);
    // }

    private void ChangeInEditor()
    {
        if (!Application.isEditor && Application.isPlaying) return;
        
        if (changeType is ChangeType.Sprite) spriteRend.sprite = spritesArray[0];
        else if (changeType is ChangeType.Material) rend.material = materials[0];
    }

    public void ChangeMaterial(int index)
    {
        if (index > materials.Length) return;
        
        rend.material = materials[index];
    }

    public void ChangeSprite(int index)
    {
        if (index > spritesArray.Length)
        
        spriteRend.sprite = spritesArray[index];
    }
}
