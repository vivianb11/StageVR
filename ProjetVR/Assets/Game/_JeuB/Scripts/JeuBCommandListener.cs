using JeuB;
using UnityEngine;

public class JeuBCommandListener : MonoBehaviour
{
    public string key;

    [SerializeField] bool activeByDefault = true;

    private void Awake()
    {
        gameObject.SetActive(activeByDefault);
        if (JeuBCommands.listenersActiveStats.ContainsKey(key))
            gameObject.SetActive(JeuBCommands.listenersActiveStats[key]);

        JeuBCommands.AddKey(key, gameObject);
    }
}
