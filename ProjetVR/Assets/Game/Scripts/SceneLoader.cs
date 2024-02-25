using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LodScene(int  sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
