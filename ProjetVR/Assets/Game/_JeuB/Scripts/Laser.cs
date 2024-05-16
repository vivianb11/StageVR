using System.Collections;
using UnityEngine;

namespace JeuB
{
    public class Laser : MonoBehaviour
    {
        public static Laser Instance;

        [SerializeField] float lifeTime = 10f;

        private void Awake()
        {
            Instance = this;
        }

        public void EnbleLaser()
        {
            gameObject.SetActive(true);
            StartCoroutine(LifeDelay());
        }

        private IEnumerator LifeDelay()
        {
            yield return new WaitForSeconds(lifeTime);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Entity tempEntity))
            {
                tempEntity.ReceiveDamage(99);
            }
        }
    }
}
