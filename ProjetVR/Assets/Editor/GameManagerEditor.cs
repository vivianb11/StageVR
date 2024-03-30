using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager gameManager = (GameManager)target;

        gameManager.currentSceneIndex = EditorGUILayout.Popup(gameManager.currentSceneIndex, GetGameModes(gameManager));
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
