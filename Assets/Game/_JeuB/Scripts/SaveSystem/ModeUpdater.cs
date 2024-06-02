using JeuB;
using SignalSystem;
using TMPro;
using UnityEngine;

public class ModeUpdater : MonoBehaviour
{
    public SO_Signal signal1;
    public SO_Signal signal2;

    private void OnEnable()
    {
        if (JeuBCommands.lastModeSignal == signal1)
        {
            GetComponent<Switch_2Events>().StartAsSwitched = false;
            transform.GetChild(0).GetComponent<TextMeshPro>().text = "Basic";
        }
        else if (JeuBCommands.lastModeSignal == signal2)
        {
            GetComponent<Switch_2Events>().StartAsSwitched = true;
            transform.GetChild(0).GetComponent<TextMeshPro>().text = "Sophorique";
        }
    }
}
