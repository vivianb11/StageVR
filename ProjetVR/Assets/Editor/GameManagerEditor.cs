using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager gameManager = (GameManager)target;

        GUILayout.Label("Choose a Game Mode");
        gameManager.currentSceneIndex = Mathf.Clamp(EditorGUILayout.Popup(gameManager.currentSceneIndex, GetGameModes(gameManager)), 0, gameManager.gameModes.Length);
        gameManager.SkipCalibration = EditorGUILayout.Toggle("Skip Calibration", gameManager.SkipCalibration);

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Switch Game Mode"))
            {
                gameManager.ChangeGameMode(gameManager.currentSceneIndex);
            }

            if (GUILayout.Button("Reload Current Game Mode"))
            {
                gameManager.ReloadGameMode(3);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(gameManager);
        }
    }

    private string[] GetGameModes(GameManager gameManager)
    {
        string[] gameModes = new string[gameManager.gameModes.Length];

        for (int i = 0; i < gameManager.gameModes.Length; i++)
        {
            gameModes[i] = gameManager.gameModes[i].name;
        }

        return gameModes;
    }
}

#endif
