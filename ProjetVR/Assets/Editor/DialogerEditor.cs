using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialoger))]
public class DialogerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Dialoger dialoguer = (Dialoger)target;

        GUILayout.Label("Choose a Game Mode");
        dialoguer.index = Mathf.Clamp(EditorGUILayout.Popup(dialoguer.index, GetDialogs(dialoguer)), 0, dialoguer.dialogs.Count);

        if (Application.isPlaying)
        {

            if (GUILayout.Button("Play Dialog"))
            {
                dialoguer.PlayDiolog(dialoguer.index);
            }
        }
    }

    private string[] GetDialogs(Dialoger dialoguer)
    {
        string[] gameModes = new string[dialoguer.dialogs.Count];

        for (int i = 0; i < dialoguer.dialogs.Count; i++)
        {
            gameModes[i] = dialoguer.dialogs[i].name;
        }

        return gameModes;
    }

}
