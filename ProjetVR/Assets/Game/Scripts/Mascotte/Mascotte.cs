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
    private float interactionDelay = 2f;

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

    private Vector3 startPos;
    private Vector3 targetPosition;

    private Interactable interacable;

    private void Start()
    {
        startPos = transform.position;
        targetPosition = transform.position;

        //interacable = GetComponent<Interactable>();
        meshRenderer.material.color = Color.green;
    }

    public void SetTask(ToothManager toothManager)
    {
        if (!canClean)
            return;

        StartCoroutine(CleanTeeth(interactionDelay, toothManager));
    }

    private IEnumerator CleanTeeth(float delay, ToothManager toothManager)
    {
        enterClean.Play();
        yield return new WaitForSeconds(delay);

        toothManager.CleanTooth();
        toothBrush.Play();
    }
}
