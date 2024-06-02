using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    public class Shield : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float returnSpeed = 2f;

        public float moveDuration = 0.5f;

        public UnityEvent OnShieldHit;

        public int damage = 1;

        private Tween tweener;

        private void Awake()
        {
            tweener = GetComponent<Tween>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Entity tempEntity))
            {
                tempEntity.ReceiveDamage(damage);

                if (other.gameObject.TryGetComponent(out Mob tempMob))
                {
                    OnShieldHit.Invoke();

                    tweener.PlayMontages();
                }
            }
        }
    }
}
