using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PanelBehavior))]
public class Dialoger : MonoBehaviour
{
    public bool UseButtonInteractable = false;
    [ShowIf("UseButtonInteractable")]

    public List<SO_Dialogs> dialogs;

    [HideInInspector]
    public int index;

    private PanelBehavior dialogPanel;

    [Button]
    public void PlayDiolog(int dialogIndex)
    {
        SO_Dialogs dialog = dialogs[dialogIndex];

        dialogPanel.SetDialog(dialog);
    }

    [Button]
    public void PlayDiolog(SO_Dialogs dialogue)
    {
        SO_Dialogs dialog = dialogue;

        dialogPanel.SetDialog(dialog);
    }

    private void Awake()
    {
        if (dialogPanel == null)
            dialogPanel = GetComponent<PanelBehavior>();
    }
}