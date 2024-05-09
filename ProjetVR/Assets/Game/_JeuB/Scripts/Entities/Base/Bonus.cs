using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JeuB {
    public abstract class Bonus : Entity
    {
        // bonus abstract class

        public int scoreOnDeath;

        public void OnTriggerEnter(Collider other)
        {
            if (other.transform == target)
            {
                ApplyBonus(target.GetComponent<ProtectedToothBehaviours>());
                Kill();
            }
        }

        public abstract void ApplyBonus(ProtectedToothBehaviours tooth);

        public override void Kill()
        {
            Destroy(gameObject);
        }

    }
}
