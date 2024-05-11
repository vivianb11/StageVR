using UnityEngine;

namespace JeuB {
    public abstract class Bonus : Entity
    {
        public void OnTriggerEnter(Collider other)
        {
            if (other.transform != target)
                return;

            ApplyBonus(target.GetComponent<ProtectedToothBehaviours>());
            Kill();
        }

        public abstract void ApplyBonus(ProtectedToothBehaviours tooth);

        public override void Kill() => Destroy(gameObject);
    }
}
