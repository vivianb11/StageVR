using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    public abstract class Bonus : Entity
    {
        public UnityEvent OnBonus;

        public void OnTriggerEnter(Collider other)
        {
            if (other.transform != target)
                return;

            ApplyBonus(target.GetComponent<ProtectedToothBehaviours>());
            Kill();
            OnBonus.Invoke();

        }

        public abstract void ApplyBonus(ProtectedToothBehaviours tooth);

        public override void Kill() => Destroy(gameObject);
    }
}
