using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueSystem))]
public class DialoguerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueSystem dialoguer = (DialogueSystem)target;

        GUILayout.Label("Choose a Game Mode");
        dialoguer.index = Mathf.Clamp(EditorGUILayout.Popup(dialoguer.index, GetDialogs(dialoguer)), 0, dialoguer.dialogs.Count);

        if (Application.isPlaying)
        {

            if (GUILayout.Button("Play Dialog"))
            {
                dialoguer.PlayDialogue(dialoguer.index, false);
            }
        }
    }

    private string[] GetDialogs(DialogueSystem dialoguer)
    {
        string[] gameModes = new string[dialoguer.dialogs.Count];

        for (int i = 0; i < dialoguer.dialogs.Count; i++)
        {
            gameModes[i] = dialoguer.dialogs[i].name;
        }

        return gameModes;
    }

}
