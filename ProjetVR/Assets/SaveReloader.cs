using JeuB;
using UnityEngine;

public class SaveReloader : MonoBehaviour
{

    public JeuBCommands jeuBCommands;

    void Start()
    {
        GameManager.Instance.OnGameReload.AddListener(() =>
        {
            jeuBCommands.lastModeSignal.Emit();
        });
    }
}
