using JeuB;
using SignalSystem;
using System.Collections;
using UnityEngine;

public class SaveReloader : MonoBehaviour
{
    public SO_Signal BaseMode;
    public SO_Signal BaseSkin;

    void Start()
    {
        JeuBCommands.lastModeSignal = BaseMode;
        JeuBCommands.lastSkinSignal = BaseSkin;

        GameManager.Instance.gameStart.AddListener(() => StartCoroutine(EmitSignals()));
    }

    private IEnumerator EmitSignals()
    {
        yield return new WaitForEndOfFrame();

        JeuBCommands.lastModeSignal.Emit();
        JeuBCommands.lastSkinSignal.Emit();
    }
}
