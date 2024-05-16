using JeuB;
using UnityEngine;

public class JeuBCommandListener : MonoBehaviour
{
    public string key;

    private void Awake()
    {
        if (JeuBCommands.listenersActiveStats.ContainsKey(key))
            gameObject.SetActive(JeuBCommands.listenersActiveStats[key]);

        JeuBCommands.AddKey(key, gameObject);
    }
}
