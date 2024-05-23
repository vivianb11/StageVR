using UnityEngine;

[CreateAssetMenu(fileName = "New Command", menuName = "ScriptableObject/Command")]
public class Commands : ScriptableObject
{
    public static void ResetGameTransform()
    {
        GameRemoteTransform.Instance.target.position = Vector3.zero;
        Vector3 playerRot = EyeManager.Instance.transform.eulerAngles;
        playerRot.z = 0;
        GameRemoteTransform.Instance.target.eulerAngles = playerRot;
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
