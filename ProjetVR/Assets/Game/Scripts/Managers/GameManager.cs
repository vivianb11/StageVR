using SignalSystem;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] SO_Signal startSignal;
    [SerializeField] PlayerInstance playerInstance;

    [SerializeField] float timeBeforeResetGame = 10f;

    [SerializeField] GameObject[] gameModes;

    public static GameManager Instance;

    public float startDelay = 1f;

    public UnityEvent gameStarted;

    public GameObject player { get; private set; }

    private DateTime timeOnUnfocus;

    [SerializeField] int currentSceneIndex;

    private GameObject nextGameMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                player = Instantiate(playerInstance).gameObject;
            }

            if (player == null)
                Debug.LogError("Player not found ! Please set the correct Tag on player");
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        OVRManager.InputFocusAcquired += CheckUnfocusedTime;
        OVRManager.InputFocusLost += () => timeOnUnfocus = DateTime.Now;
        SceneLoader.Instance.fadeInCompleted.AddListener(LoadGameMode);
        SceneLoader.Instance.fadeOutCompleted.AddListener(StartGameMode);

        ChangeGameMode(currentSceneIndex);
    }

    public void RestartSceneTimer()
    {
        Invoke("RestartScene", 5f);
    }

    public void ChangeGameMode(int index)
    {
        DestroyGameModes();

        nextGameMode = gameModes[index];

        LoadGameMode();
    }

    public void ReloadGameMode(float delay)
    {
        nextGameMode = gameModes[currentSceneIndex];
        StartCoroutine(UnloadGameMode(delay));
    }

    private void StartGameMode()
    {
        StartCoroutine(StartDelay(startDelay));
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

    private void DestroyGameModes()
    {
        foreach (Transform t in transform.transform)
        {
            Destroy(t.gameObject);
        }
    }

    private IEnumerator UnloadGameMode(float delay)
    {
        yield return new WaitForSeconds(delay);

        DestroyGameModes();
        SceneLoader.Instance.FadeIn();
    }

    private void LoadGameMode()
    {
        Instantiate(nextGameMode, transform);
        nextGameMode = null;

        SceneLoader.Instance.FadeOut();
    }
}
