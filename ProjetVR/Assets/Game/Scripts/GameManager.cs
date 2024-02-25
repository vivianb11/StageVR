using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool UseEyeTracking;
    public bool UseDynamicFoveatedRendering;

    public static GameManager Instance;
    public GameObject player { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found ! Please set the correct Tag on player");
        }

        if (OVRManager.eyeTrackedFoveatedRenderingSupported)
        {
            Debug.Log("Eye Tracking Available !");
            OVRManager.eyeTrackedFoveatedRenderingEnabled = UseEyeTracking;
            OVRManager.useDynamicFoveatedRendering = UseDynamicFoveatedRendering;
        }
    }
}
