using SignalSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] SignalEmitter signalEmitter;

    public static GameManager Instance;

    public bool UseEyeTracking;
    public bool UseDynamicFoveatedRendering;

    public float startDelay = 1f;

    public UnityEvent gameStarted;

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
            Debug.LogError("Player not found ! Please set the correct Tag on player");

        if (OVRManager.eyeTrackedFoveatedRenderingSupported)
        {
            Debug.Log("Eye Tracking Available !");
            OVRManager.eyeTrackedFoveatedRenderingEnabled = UseEyeTracking;
            OVRManager.useDynamicFoveatedRendering = UseDynamicFoveatedRendering;
        }

        StartCoroutine(StartDelay(startDelay));

        //OVRManager.InputFocusAcquired += OVRManager.display.RecenterPose;
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        signalEmitter.RequestSignalCall();
        gameStarted?.Invoke();
    }
}
