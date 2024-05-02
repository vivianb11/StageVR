using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections.Generic;

namespace JeuA
{

    public class Mascotte : MonoBehaviour
    {
        public enum MascotteState
        {
            IDLE, HELP_TARTAR, HELP_DECAY, HELP_DIRTY, HELP_SMELL, HAPPY, CONFUSE
        }

        public MascotteState state;

        [SerializeField] float checkDelay;

        [SerializeField] float interactionDelay = 2f;

        [SerializeField] ToothManager tooth;

        [SerializeField] DialogueSystem dialogueSystem;

        public SO_Dialogs tartarDialogue;

        public SO_Dialogs dirtyDialogue;

        public SO_Dialogs decayDialogue;

        public SO_Dialogs smellDialogue;

        public SO_Dialogs happyDialogue;

        private bool canClean = true;

        [Space(10)]
        [Foldout("Events")]
        public UnityEvent<MascotteState> stateSwitched;
        [Foldout("Events")]
        public UnityEvent tartarHelpState;
        [Foldout("Events")]
        public UnityEvent dirtyHelpState;
        [Foldout("Events")]
        public UnityEvent decayHelpState;
        [Foldout("Events")]
        public UnityEvent smellHelpState;
        [Foldout("Events")]
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

            bool inTutorial = Tutorial.inTutorial;

            switch (newState)
            {
                case MascotteState.HELP_TARTAR:
                    tartarHelpState?.Invoke();
                    dialogueSystem.PlayDialogue(tartarDialogue, inTutorial);
                    break;
                case MascotteState.HELP_DECAY:
                    decayHelpState?.Invoke();
                    dialogueSystem.PlayDialogue(decayDialogue, inTutorial);
                    break;
                case MascotteState.HELP_DIRTY:
                    dirtyHelpState?.Invoke();
                    dialogueSystem.PlayDialogue(dirtyDialogue, inTutorial);
                    break;
                case MascotteState.HELP_SMELL:
                    smellHelpState?.Invoke();
                    dialogueSystem.PlayDialogue(smellDialogue, inTutorial);
                    break;
                case MascotteState.HAPPY:
                    happyState?.Invoke();
                    dialogueSystem.PlayDialogue(happyDialogue, inTutorial);
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
            int decayCount = tooth.teethCells.Where(item => item.teethState == TeethState.Decay).Count();

            if (tartarCount > 0)
            {
                SwitchState(MascotteState.HELP_TARTAR);
                return;
            }

            if (tooth.dirtyTooth)
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

}