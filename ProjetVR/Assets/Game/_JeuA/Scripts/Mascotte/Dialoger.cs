using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialoger : MonoBehaviour
{
    public bool UseButtonInteractable = false;
    [ShowIf("UseButtonInteractable")]
    public Interactable buttonInteractable;
    private Interactable panelInteractable;
    private Interactable interactable;
    private PanelBehavior dialogPanel;
    public List<Dialogs> diologs;

    [Header("Play Dialog")]
    public string dialogName;

    [Button]
    public void PlayDiolog()
    {
        var dialog = diologs.Find(x => x.DialogName == dialogName);

        if (!UseButtonInteractable)
        { 
            interactable = panelInteractable;
        }
        else
        {
            interactable = buttonInteractable;
        }
        
        if (dialog.interactionType == InteractionType.Blink)
        {
            Debug.Log("Blink Not Implemented yet");
        }
        else if (dialog.interactionType == InteractionType.LookOut)
        {
            interactable.selectionCondition = SelectionCondition.LOOK_IN;
            interactable.deSelectionCondition = DeSelectionCondition.LOOK_OUT_TIME;
            interactable.lookOutTime = 1f;
        }
        else if (dialog.interactionType == InteractionType.LookAt)
        {
            interactable.selectionCondition = SelectionCondition.LOOK_IN_TIME;
            interactable.lookInTime = 1f;
            interactable.deSelectionCondition = DeSelectionCondition.AUTO_TIME;
            interactable.autoTime = 0.5f;
        }

        dialogPanel.SetText("");

        StartCoroutine(PlayDialog(dialog));
    }

    private void Awake()
    {
        if (panelInteractable == null)
            panelInteractable = GetComponent<Interactable>();

        if (dialogPanel == null)
            dialogPanel = GetComponent<PanelBehavior>();
    }

    IEnumerator PlayDialog(Dialogs dialog)
    {
        foreach (var content in dialog.dialogContents)
        {
            if (content.audio != null && !content.continueAudio)
            {
                dialogPanel.PlayAudio(content.audio);
            }
            else if (!content.continueAudio)
            {
                dialogPanel.StopAudio();
            }

            if (content.displayEffect == DisplayEffect.None)
            {
                dialogPanel.SetText(content.text);
            }
            else if (content.displayEffect == DisplayEffect.Blink)
            {
                dialogPanel.SetText(content.text, content.duration * content.EffectTime);
            }

            if (dialog.interactionType == InteractionType.Auto)
            {
                yield return new WaitForSeconds(content.duration);
            }
            else
            {
                yield return new WaitUntil(() => interactable.selected);
            }
            
            dialogPanel.SetText("");

            if (dialog.switchSound != null)
            {
                dialogPanel.PlaySFX(dialog.switchSound);
            }
        }

        dialogPanel.StopAudio();

        yield return null;
    }
}

[Serializable]
public class Dialogs
{
    public string DialogName;
    public InteractionType interactionType;

    public AudioClip switchSound;

    public List<DialogContent> dialogContents;
}

[Serializable]
public class DialogContent
{
    public DisplayEffect displayEffect = DisplayEffect.None;
    [Range(0f,1f)]
    public float EffectTime = 0.75f;
    [TextArea(3, 10)]
    public string text;

    public bool continueAudio;
    public AudioClip audio;

    public float duration = 1f;
}

public enum DisplayEffect
{
    None,
    Blink,
    Shake
}

public enum InteractionType
{
    Auto,
    Blink,
    LookAt,
    LookOut
}