using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Events;

public class Mascotte : MonoBehaviour
{
    public enum MascotteState
    {
        IDLE, HELP_TARTAR, HELP_DECAY, HELP_DIRTY, HELP_SMELL, HAPPY
    }

    public MascotteState state;

    [SerializeField] ToothManager tooth;

    [SerializeField] float checkDelay;
    [SerializeField] float checkDelayAfterFullClean;

    [SerializeField]
    private float interactionDelay = 2f;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private AudioSource enterClean;

    [SerializeField]
    private AudioSource toothBrush;

    private bool canClean = true;

    public UnityEvent tartarHelpState;
    public UnityEvent dirtyHelpState;
    public UnityEvent decayHelpState;
    public UnityEvent smellHelpState;
    public UnityEvent happyState;

    private Coroutine checkCoroutine;

    private void Start()
    {
        meshRenderer.material.color = Color.green;

        tooth.CellCleaned.AddListener(() => PlayCheck(checkDelay));
        tooth.OnTeethCleaned.AddListener(() => SwitchState(MascotteState.HAPPY));

        PlayCheck(checkDelay);
    }

    public void SwitchState(MascotteState newState)
    {
        if (newState == state)
            return;

        state = newState;

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

                PlayCheck(checkDelayAfterFullClean);

                Debug.Log("GG !".SetColor(Color.green));
                break;
        }
    }

    public void SetTask(ToothManager toothManager)
    {
        if (!canClean)
            return;

        StartCoroutine(CleanTeeth(interactionDelay, toothManager));
    }

    private void PlayCheck(float delay)
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
    }

    private IEnumerator CleanTeeth(float delay, ToothManager toothManager)
    {
        enterClean.Play();
        yield return new WaitForSeconds(delay);

        toothManager.CleanTooth();
        toothBrush.Play();
    }

    private IEnumerator CheckDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckTooth();
    }
}
