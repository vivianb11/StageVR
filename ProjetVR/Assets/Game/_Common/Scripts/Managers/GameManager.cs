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

    [SerializeField] SO_Signal startSignal;

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

    private bool isReloading = false;

    private bool _gamePaused = false;
    public bool gamePaused
    {
        get => _gamePaused;
        set
        {
            _gamePaused = value;
            Time.timeScale = value ? 0f : 1f;
        }
    }

    private float _holdTimeReset;
    private float _holdTimeMaxReset = 1f;
    private bool _resetHolded;

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

    private void Update()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
            _resetHolded = false;

        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger) || Input.GetKey(KeyCode.R))
        {
            if (!_resetHolded)
            {
                _holdTimeReset += Time.deltaTime;

                if (_holdTimeReset >= _holdTimeMaxReset)
                {
                    _holdTimeReset = 0;
                    _resetHolded = true;
                    Commands.ResetGameTransform();
                }
            }
        }
    }

    public void ChangeGameMode(int index)
    {
        nextGameMode = gameModes[index];

        StartCoroutine(UnloadGameMode(3f, 3f));
        StartCoroutine(ChangeGameModeCoroutine());
    }

    public void ReloadGameMode(float delay)
    {
        if (isReloading)
            return;

        isReloading = true;
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
        yield return new WaitForSecondsRealtime(3);

        LoadNextGameMode();
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        startSignal.Emit();
        gameStarted?.Invoke();
        gamePaused = false;
    }

    private void CheckUnfocusedTime()
    {
        Debug.Log((DateTime.Now - timeOnUnfocus).ToString().SetColor(Color.cyan));

        if ((DateTime.Now - timeOnUnfocus).TotalSeconds > timeBeforeResetGame)
        {
            gameStopped?.Invoke();
            ReloadGameMode(0);
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
        yield return new WaitForSecondsRealtime(delay);

        SceneLoader.Instance.FadeIn(fadeDuration);
        yield return new WaitForSecondsRealtime(fadeDuration);
        DestroyGameModes();
        LoadNextGameMode();
    }

    private void LoadNextGameMode()
    {
        Instantiate(nextGameMode, transform);
        nextGameMode = null;

        SceneLoader.Instance.FadeOut(3);
        isReloading = false;
    }
}
