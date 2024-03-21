using SignalSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] SO_Signal startSignal;

    public static GameManager Instance;

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

        StartCoroutine(StartDelay(startDelay));

        //OVRManager.InputFocusAcquired += OVRManager.display.RecenterPose;
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        startSignal.Emit();
        gameStarted?.Invoke();
    }
}
