using UnityEngine;

[CreateAssetMenu(fileName = "New Command", menuName = "ScriptableObject/Command")]
public class Commands : ScriptableObject
{
    public static void ResetGameTransform()
    {
        GameRemoteTransform.Instance.target.position = Vector3.zero;
        GameRemoteTransform.Instance.target.eulerAngles = Vector3.zero;
    }

    public void ReloadGameMode(float time)
    {
        GameManager.Instance.ReloadGameMode(time);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
