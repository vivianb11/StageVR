using UnityEngine;

namespace JeuB
{
    public class BonusHP : Bonus
    {
        [SerializeField] int HpToAdd;

        protected override void EntityStart() { }

        protected override void EntityUpdate() { }

        public override void ApplyBonus(ProtectedToothBehaviours tooth)
        {
            tooth.Heal(HpToAdd);
        }

        public void Freeze() => moveSpeed = 0;

    }
}