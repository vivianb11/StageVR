using JeuB;
using TMPro;
using UnityEngine;

public class ToggleUpdater : MonoBehaviour
{
    public string toogleName;
    public bool invertToggle;

    public string firstDescription;
    public string secondDescription;

    private void OnEnable()
    {
        //chacks if the listener exists
        if (!JeuBCommands.listenersActiveStats.ContainsKey(toogleName))
        {
            Debug.LogError("The listener " + toogleName + " does not exist");
            return;
        }

        if (JeuBCommands.listenersActiveStats[toogleName])
        {
            GetComponent<Switch_2Events>().StartAsSwitched = invertToggle ? true : false;
            transform.GetChild(0).GetComponent<TextMeshPro>().text = firstDescription;
        }
        else
        {
            GetComponent<Switch_2Events>().StartAsSwitched = invertToggle ? false : true;
            transform.GetChild(0).GetComponent<TextMeshPro>().text = secondDescription;
        }
    }
}
