using JeuB;
using UnityEngine;

public class SaveReloader : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.gameStart.AddListener(() =>
        {
            JeuBCommands.lastModeSignal.Emit();
            JeuBCommands.lastSkinSignal.Emit();
        });
    }
}
