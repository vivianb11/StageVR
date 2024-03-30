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

        StartGameMode();
    }

    public void RestartSceneTimer()
    {
        Invoke("RestartScene", 5f);
    }

    public void ChangeGameMode(GameObject newGameMode)
    {
        DisableGameModes();

        nextGameMode = newGameMode;
    }

    public void ReloadGameMode(float delay)
    {
        nextGameMode = gameModes.Where(item => item.activeInHierarchy == true).ToArray()[0];
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

    private void DisableGameModes()
    {
        foreach (GameObject gameMode in gameModes)
        {
            gameMode.SetActive(false);
        }
    }

    private IEnumerator UnloadGameMode(float delay)
    {
        yield return new WaitForSeconds(delay);

        DisableGameModes();
        SceneLoader.Instance.FadeIn();
    }

    private void LoadGameMode()
    {
        nextGameMode.SetActive(true);
        nextGameMode = null;

        SceneLoader.Instance.FadeOut();
    }
}
