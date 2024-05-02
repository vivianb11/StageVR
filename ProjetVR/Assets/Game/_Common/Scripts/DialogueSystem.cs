using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PanelBehavior))]
public class DialogueSystem : MonoBehaviour
{
    public bool UseButtonInteractable = false;
    [ShowIf("UseButtonInteractable")]

    public List<SO_Dialogs> dialogs;

    [HideInInspector]
    public int index;

    private PanelBehavior dialoguePanel;

    [Button]
    public void PlayDialogue(int dialogIndex, bool inTutorial)
    {
        SO_Dialogs dialog = dialogs[dialogIndex];

        if (inTutorial)
            dialoguePanel.SetDialogue(dialog.tutorialDialogue);
        else
            dialoguePanel.SetDialogue(dialog.content);
    }

    [Button]
    public void PlayDialogue(SO_Dialogs dialogue, bool inTutorial)
    {
        if (inTutorial)
            dialoguePanel.SetDialogue(dialogue.tutorialDialogue);
        else
            dialoguePanel.SetDialogue(dialogue.content);
    }

    private void Awake()
    {
        if (dialoguePanel == null)
            dialoguePanel = GetComponent<PanelBehavior>();
    }
}