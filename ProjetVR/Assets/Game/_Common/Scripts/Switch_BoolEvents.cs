using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class Switch_BoolEvents : MonoBehaviour
{
    public UnityEvent Event;
    bool state = false;

    public void SwitchEvent()
    {
        ExecuteEvent(state);

        state = !state;
    }

    public void ExecuteEvent(bool invered)
    {
        if (invered)
        {
            // getts all listeners of the event
            var listeners = Event.GetPersistentEventCount();

            // iterate over all listeners
            for (int i = 0; i < listeners; i++)
            {
                // get the target object of the listener
                UnityEngine.Object target = Event.GetPersistentTarget(i);

                // gets the method info of the listener
                MethodInfo method = target.GetType().GetMethod(Event.GetPersistentMethodName(i));

                // looks if the parameter is a bool
                if (method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(bool))
                {
                    throw new NotImplementedException();

                    // invoke the method with the opposite value as it should be
                }
                else
                {
                    // invoke the method with the normal value
                    method.Invoke(target, new object[] { state });
                }
            }
        }
        else
        {
            Event?.Invoke();
        }
    }
}
