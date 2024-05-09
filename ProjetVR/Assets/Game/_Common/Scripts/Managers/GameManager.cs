using SignalSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] float timeBeforeResetGame = 10f;

    [SerializeField] GameObject calibration;

    [SerializeField] SO_Signal startSignal;
    [SerializeField] PlayerInstance playerInstance;

    public GameObject[] gameModes;

    public float startDelay = 1f;

    [HideInInspector]
    public bool SkipCalibration;

    //[HideInInspector]
    public int currentSceneIndex;

    public UnityEvent gameStarted;

    public UnityEvent gameStopped;

    private DateTime timeOnUnfocus;

    private GameObject nextGameMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        OVRManager.InputFocusAcquired += CheckUnfocusedTime;
        OVRManager.InputFocusLost += () => timeOnUnfocus = DateTime.Now;
        OVRManager.InputFocusLost += () => Time.timeScale = 0;

        SceneLoader.Instance.fadeOutCompleted.AddListener(StartGameMode);

        if (SkipCalibration)
        {
            nextGameMode = gameModes[currentSceneIndex];
            LoadNextGameMode();
        }
        else
            ChangeGameMode(currentSceneIndex);
    }

    public void OnCalibrationFinished()
    {
        StartCoroutine(UnloadGameMode(0, 3));
    }

    public void ChangeGameMode(int index)
    {
        nextGameMode = gameModes[index];

        StartCoroutine(ChangeGameModeCoroutine());
    }

    public void ReloadGameMode(float delay)
    {
        nextGameMode = gameModes[currentSceneIndex];
        StartCoroutine(UnloadGameMode(delay, 3));
    }

    private void StartGameMode()
    {
        StartCoroutine(StartDelay(startDelay));
    }

    private IEnumerator ChangeGameModeCoroutine()
    {
        SceneLoader.Instance.FadeIn(3);
        yield return new WaitForSeconds(3);

        LoadCalibrationMode();
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
            gameStopped?.Invoke();
            ChangeGameMode(currentSceneIndex);
        }

        Time.timeScale = 1;
        OVRManager.display.RecenterPose();
    }

    private void DestroyGameModes()
    {
        foreach (Transform t in transform.transform)
        {
            DestroyImmediate(t.gameObject);
        }
    }

    private IEnumerator UnloadGameMode(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        SceneLoader.Instance.FadeIn(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        DestroyGameModes();
        LoadNextGameMode();
    }

    private void LoadNextGameMode()
    {
        Instantiate(nextGameMode, transform);
        nextGameMode = null;

        SceneLoader.Instance.FadeOut(3);
    }

    private void LoadCalibrationMode()
    {
        DestroyGameModes();
        calibration.SetActive(true);
        SceneLoader.Instance.FadeOut(3);
    }
}
