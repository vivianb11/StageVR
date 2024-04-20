using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Events;

public class Mascotte : MonoBehaviour
{
    public enum MascotteState
    {
        IDLE, HELP_TARTAR, HELP_DECAY, HELP_DIRTY, HELP_SMELL, HAPPY, CONFUSE
    }

    public MascotteState state;

    [SerializeField] ToothManager tooth;

    [SerializeField] float checkDelay;

    [SerializeField]
    private float interactionDelay = 2f;

    private bool canClean = true;

    [Space(10)]

    public UnityEvent<MascotteState> stateSwitched;

    public UnityEvent tartarHelpState;
    public UnityEvent dirtyHelpState;
    public UnityEvent decayHelpState;
    public UnityEvent smellHelpState;
    public UnityEvent happyState;

    private Coroutine checkCoroutine;

    private HeadMotionTracker headMotionTracker;

    private Collider bodyCollider;

    private void Start()
    {
        bodyCollider = GetComponent<Collider>();
        bodyCollider.enabled = false;

        tooth.respawned.AddListener(() => StartToothCheckingState(checkDelay));
        tooth.CellCleaned.AddListener(() => StartToothCheckingState(checkDelay));
        tooth.OnTeethCleaned.AddListener(() => SwitchState(MascotteState.HAPPY));

        tooth.decayOnly.AddListener(() => bodyCollider.enabled = true);

        headMotionTracker = FindAnyObjectByType<HeadMotionTracker>();

        headMotionTracker.Excited.AddListener(() => SwitchState(MascotteState.CONFUSE));
        headMotionTracker.Normal.AddListener(() => SwitchState(MascotteState.IDLE));
        headMotionTracker.Calme.AddListener(() => SwitchState(MascotteState.IDLE));

        StartToothCheckingState(checkDelay);
    }

    private void OnDisable()
    {
        headMotionTracker.Excited.RemoveListener(() => SwitchState(MascotteState.CONFUSE));
        headMotionTracker.Normal.RemoveListener(() => SwitchState(MascotteState.IDLE));
        headMotionTracker.Calme.RemoveListener(() => SwitchState(MascotteState.IDLE));
    }

    public void CleanTooth()
    {
        tooth.CleanTooth();
        bodyCollider.enabled = false;
    }

    public void SwitchState(MascotteState newState)
    {
        if (newState == state)
            return;

        state = newState;
        stateSwitched?.Invoke(state);

        switch (newState)
        {
            case MascotteState.HELP_TARTAR:
                tartarHelpState?.Invoke();
                Debug.Log("Say tartar stuff".SetColor(Color.green));
                break;
            case MascotteState.HELP_DECAY:
                decayHelpState?.Invoke();
                Debug.Log("Say decay stuff".SetColor(Color.green));
                break;
            case MascotteState.HELP_DIRTY:
                dirtyHelpState?.Invoke();
                Debug.Log("Say dirty stuff".SetColor(Color.green));
                break;
            case MascotteState.HELP_SMELL:
                smellHelpState?.Invoke();
                Debug.Log("Say smell stuff".SetColor(Color.green));
                break;
            case MascotteState.HAPPY:
                happyState?.Invoke();
                Debug.Log("GG !".SetColor(Color.green));
                break;
            case MascotteState.IDLE:
                StartToothCheckingState(checkDelay);
                break;
            case MascotteState.CONFUSE:
                break;
        }
    }

    public void SetTask(ToothManager toothManager)
    {
        if (!canClean)
            return;

        StartCoroutine(CleanTeeth(interactionDelay, toothManager));
    }

    private void StartToothCheckingState(float delay)
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(CheckDelay(delay));
    }

    private void CheckTooth()
    {
        int tartarCount = tooth.teethCells.Where(item => item.teethState == TeethState.Tartar).Count();
        int dirtyCount = tooth.teethCells.Where(item => item.teethState == TeethState.Dirty).Count();
        int decayCount = tooth.teethCells.Where(item => item.teethState == TeethState.Decay).Count();

        if (tartarCount > 0)
        {
            SwitchState(MascotteState.HELP_TARTAR);
            return;
        }

        if (dirtyCount > 0)
        {
            SwitchState(MascotteState.HELP_DIRTY);
            return;
        }

        if (decayCount > 0)
        {
            SwitchState(MascotteState.HELP_DECAY);
            return;
        }

        if (tooth.smells)
        {
            SwitchState(MascotteState.HELP_SMELL);
            return;
        }

        StartToothCheckingState(1f);
    }

    private IEnumerator CleanTeeth(float delay, ToothManager toothManager)
    {
        yield return new WaitForSeconds(delay);

        toothManager.CleanTooth();
    }

    private IEnumerator CheckDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckTooth();
    }
}
