using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JeuA
{

    public class FoodBehavior : MonoBehaviour
    {
        private Rigidbody rb;
        private Interactable interactable;
        private ToothManager ToothMan;
        public ToothManager toothManager { get { return ToothMan; } set { ToothMan = value; } }

        public float interactionTime = 1f;

        public float delayBeforeDestroayable = 1f;
        private bool isDestroyable = false;

        public float force = 200f;

        private bool ejected = false;

        [SerializeField] GameObject Explosion;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            interactable = GetComponent<Interactable>();
            interactable.SetActivateState(false);

            interactable.selectionCondition = SelectionCondition.LOOK_IN_TIME;
            interactable.lookInTime = interactionTime;

            interactable.onSelected.AddListener(() => EjectFood());
        }

        private void Update()
        {
            if (!ejected)
                return;

            transform.rotation = Quaternion.LookRotation(rb.velocity);

            transform.GetChild(0).transform.Rotate(0, 0, 500 * Time.deltaTime);
        }

        [Button]
        public void EjectFood()
        {
            transform.SetParent(null);
            ejected = true;

            rb.isKinematic = false;
            rb.useGravity = true;

            Vector3 direction = Vector3.up * Random.Range(0.5f, 1f) + Random.Range(-0.5f, 0.5f) * Vector3.right;

            rb.AddForce(direction * force);

            toothManager.MinusFood();

            StartCoroutine(DestroyableDelay());
        }

        private void OnCollisionStay(Collision collision)
        {

            if (rb.velocity != Vector3.zero && ejected && isDestroyable)
            {
                StartCoroutine(DestroyFood());
            }
        }

        private IEnumerator DestroyFood()
        {
            Explosion.SetActive(true);

            gameObject.transform.localScale = Vector3.zero;

            yield return new WaitForSeconds(1.5f);

            Destroy(gameObject);
        }

        private IEnumerator DestroyableDelay()
        {
            yield return new WaitForSeconds(delayBeforeDestroayable);

            isDestroyable = true;
        }
    }

}