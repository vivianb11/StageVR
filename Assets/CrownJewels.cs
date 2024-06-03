using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownJewels : MonoBehaviour
{

    public int childIndex = 0;

    public void JewelsDelete()
    {
        // Get the child transform by index
        Transform topJewels = transform.GetChild(2);
        Transform childTransform = topJewels.GetChild(childIndex);


        if (childTransform != null)
        {
            // Deactivate the child GameObject
            childTransform.gameObject.SetActive(false);
            childIndex++;
        }
    }
}
