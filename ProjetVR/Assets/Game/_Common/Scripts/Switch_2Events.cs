using UnityEngine;
using UnityEngine.Events;

public class Switch_2Events : MonoBehaviour
{
    public UnityEvent Event1, Event2;
    bool state = false;

    public bool StartAsSwitched
    {
        get => state;

        set => state = !state;
    }


    public void SwitchEvent()
    {
        if (state)
        {
            Event1?.Invoke();
        }
        else
        {
            Event2?.Invoke();
        }

        state = !state;
    }
}
