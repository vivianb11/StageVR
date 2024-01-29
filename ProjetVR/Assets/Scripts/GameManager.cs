using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool UseEyeTracking;
    public bool UseDynamicFoveatedRendering;

    void Start()
    {
        if (OVRManager.eyeTrackedFoveatedRenderingSupported)
        {
            Debug.Log("Eye Tracking Available !");
            OVRManager.eyeTrackedFoveatedRenderingEnabled = UseEyeTracking;
            OVRManager.useDynamicFoveatedRendering = UseDynamicFoveatedRendering;
        }
    }
}
