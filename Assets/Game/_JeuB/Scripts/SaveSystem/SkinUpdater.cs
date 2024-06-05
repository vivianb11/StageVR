using JeuB;
using SignalSystem;
using TMPro;
using UnityEngine;

public class SkinUpdater : MonoBehaviour
{
    public SO_Signal signal1;
    public SO_Signal signal2;

    private void OnEnable()
    {
        if (JeuBCommands.lastSkinSignal == signal1)
        {
            GetComponent<Switch_2Events>().StartAsSwitched = false;
            transform.GetChild(0).GetComponent<TextMeshPro>().text = "Dental";
        }
        else if (JeuBCommands.lastSkinSignal == signal2)
        {
            GetComponent<Switch_2Events>().StartAsSwitched = true;
            transform.GetChild(0).GetComponent<TextMeshPro>().text = "Medieval";
        }
    }
}
