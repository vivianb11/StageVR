using JeuB;
using UnityEngine;

public class SaveReloader : MonoBehaviour
{

    public JeuBCommands jeuBCommands;

    void Start()
    {
        GameManager.Instance.gameStart.AddListener(() =>
        {
            jeuBCommands.lastModeSignal.Emit();
        });
    }
}
