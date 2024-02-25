using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VivDebuger : MonoBehaviour
{
   public void EventTextDebuger(string text)
   {
       Debug.Log(text);
   }

    public void EventChangeColor_red(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.red;
    }

    public void EventChangeColor_Yellow(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void EventChangeColor_green(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.green;
    }
}
