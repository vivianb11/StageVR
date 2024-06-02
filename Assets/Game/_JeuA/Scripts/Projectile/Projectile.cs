using System.Collections;
using UnityEngine;

namespace JeuA
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public float projectilSpeed;
        public float randomness = 0.5f;
        public float lifeTime;

        protected Rigidbody body;

        protected bool canCollid = true;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            StartCoroutine(KillTimer());
        }

        public void ApplyImpulse()
        {
            body.velocity = Vector3.zero;

            Vector3 launchDirection = transform.forward + Vector3.right * Random.Range(-randomness, randomness) + Vector3.up * Random.Range(-randomness, randomness);

            body.AddForce(launchDirection * projectilSpeed);
        }

        private IEnumerator KillTimer()
        {
            yield return new WaitForSeconds(lifeTime);

            gameObject.SetActive(false);
        }
    }

}