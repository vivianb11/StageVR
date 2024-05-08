using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    public class Shield : MonoBehaviour
    {
        public GameObject targetObject;
        public float moveSpeed = 5f;
        public float returnSpeed = 2f;

        public float moveDuration = 0.5f;

        public UnityEvent OnShieldHit;

        private Tween tweener;
        private ProtectedToothBehaviours tooth;

        private void Awake()
        {
            tweener = GetComponent<Tween>();
            tooth = targetObject.GetComponent<ProtectedToothBehaviours>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Ennemy"))
            {
                tooth.enemyPoints = other.GetComponent<Mob>().scoreOnDeath;
                OnShieldHit.Invoke();

                tweener.PlayMontages();
            }
        }
    }
}
