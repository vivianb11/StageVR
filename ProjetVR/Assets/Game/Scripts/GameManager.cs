using SignalSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] SO_Signal startSignal;
    [SerializeField] PlayerInstance playerInstance;

    [SerializeField] float timeBeforeResetGame = 10f;

    public static GameManager Instance;

    public float startDelay = 1f;

    public UnityEvent gameStarted;

    public GameObject player { get; private set; }

    private DateTime timeOnUnfocus;

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
            player = Instantiate(playerInstance).gameObject;
        }

        if (player == null)
            Debug.LogError("Player not found ! Please set the correct Tag on player");

        StartCoroutine(StartDelay(startDelay));

        OVRManager.InputFocusAcquired += CheckUnfocusedTime;
        OVRManager.InputFocusLost += () => timeOnUnfocus = DateTime.Now;
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        startSignal.Emit();
        gameStarted?.Invoke();
    }

    private void CheckUnfocusedTime()
    {
        Debug.Log((DateTime.Now - timeOnUnfocus).ToString().SetColor(Color.cyan));

        if ((DateTime.Now - timeOnUnfocus).TotalSeconds > timeBeforeResetGame)
        {
            SceneLoader.Instance.LodScene(0);
        }

        OVRManager.display.RecenterPose();
    }
}
