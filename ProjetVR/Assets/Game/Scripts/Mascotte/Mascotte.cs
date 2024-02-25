using UnityEngine;
using System.Linq;
using System.Collections;

public class Mascotte : MonoBehaviour
{
    public enum MascotteState
    {
        IDLE, TRAVEL_SPAWN, TRAVEL_TEETH, CLEANING, PAUSE
    }

    public MascotteState state;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float range = 2f;

    [SerializeField]
    private float interactionDelay = 2f;

    [SerializeField]
    private float cleanCooldown = 5f;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private AudioSource enterClean;

    [SerializeField]
    private AudioSource enterPause;

    [SerializeField]
    private AudioSource exitPause;

    [SerializeField]
    private AudioSource toothBrush;

    private bool canClean = true;

    private Teeth[] teeths;
    private Teeth teethTarget;

    private Vector3 startPos;
    private Vector3 targetPosition;

    private Interactable interacable;

    private void Start()
    {
        teeths = FindObjectsOfType<Teeth>();
        startPos = transform.position;
        targetPosition = transform.position;

        //interacable = GetComponent<Interactable>();
        meshRenderer.material.color = Color.green;
    }

    //private void FixedUpdate()
    //{
    //    switch (state)
    //    {
    //        case MascotteState.TRAVEL_TEETH:
    //            if (TravelToDestination(targetPosition, range)) SwitchState(MascotteState.CLEANING);
    //            break;
    //        case MascotteState.TRAVEL_SPAWN:
    //            if (TravelToDestination(targetPosition, 0))
    //            { 
    //                SwitchState(MascotteState.PAUSE);
    //                StartCoroutine(CleanCooldown(cleanCooldown));
    //            } 
    //            break;
    //    }
    //}

    //private bool TravelToDestination(Vector3 destination, float destinationRange)
    //{
    //    if (Vector3.Distance(transform.position, destination) <= destinationRange)
    //        return true;

    //    transform.LookAt(destination);
    //    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    //    transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime);

    //    return false;
    //}

    //private IEnumerator InteractionDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    teethTarget.CleanTeeth();

    //    SwitchState(MascotteState.TRAVEL_SPAWN);
    //}

    //private IEnumerator CleanCooldown(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    SwitchState(MascotteState.IDLE);
    //}

    //private void SwitchState(MascotteState newState)
    //{
    //    state = newState;

    //    switch (state)
    //    {
    //        case MascotteState.IDLE:
    //            canClean = true;
    //            interacable.DeSelect();
    //            interacable.SetCanBeInteracted(true);
    //            exitPause.Play();
    //            animator.SetBool("walk", false);
    //            break;
    //        case MascotteState.TRAVEL_SPAWN:
    //            targetPosition = startPos;
    //            teethTarget = null;
    //            animator.SetBool("walk", true);
    //            break;
    //        case MascotteState.TRAVEL_TEETH:
    //            canClean = false;
    //            interacable.SetCanBeInteracted(false);
    //            enterClean.Play();
    //            animator.SetBool("walk", true);
    //            break;
    //        case MascotteState.CLEANING:
    //            toothBrush.Play();
    //            StartCoroutine(InteractionDelay(interactionDelay));
    //            animator.SetBool("walk", false);
    //            break;
    //        case MascotteState.PAUSE:
    //            canClean = false;
    //            enterPause.Play();
    //            animator.SetBool("walk", false);
    //            break;
    //    }
    //}

    //private bool GetClosestTeeth(out Teeth closestTeeth)
    //{
    //    closestTeeth = null;

    //    Teeth[] dirtyTeeth = teeths.Where(item => item.state != Teeth.TeethState.WHITE).ToArray();

    //    if (dirtyTeeth.Length == 0)
    //        return false;

    //    closestTeeth = dirtyTeeth[0];

    //    foreach (Teeth t in dirtyTeeth)
    //    {
    //        float closestTeethDistance = Vector3.Distance(closestTeeth.transform.position, transform.position);
    //        float currentTeethDistance = Vector3.Distance(t.transform.position, transform.position);

    //        if (((int)t.state) < ((int)closestTeeth.state))
    //            continue;

    //        if (currentTeethDistance < closestTeethDistance)
    //            closestTeeth = t;
    //    }

    //    return true;
    //}


    public void SetTask(ToothManager toothManager)
    {
        if (!canClean)
            return;

        StartCoroutine(CleanTeeth(interactionDelay, toothManager));
        
        //if (GetClosestTeeth(out teethTarget))
        //{
        //    targetPosition = teethTarget.transform.position;
        //    SwitchState(MascotteState.TRAVEL_TEETH);
        //}
        //else
        //    interacable.DeSelect();
    }

    private IEnumerator CleanTeeth(float delay, ToothManager toothManager)
    {
        yield return new WaitForSeconds(delay);

        toothManager.CleanTeeth();
    }
}
