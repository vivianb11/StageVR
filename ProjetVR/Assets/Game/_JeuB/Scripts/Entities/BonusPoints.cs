using UnityEngine;

namespace JeuB
{
    public class BonusPoints : Bonus
    {
        [SerializeField] int scoreToAdd;

        protected override void EntityStart()
        {
            transform.GetChild(0).localRotation = Quaternion.EulerAngles(Vector3.zero);
        }

        protected override void EntityUpdate() { }

        public override void ApplyBonus(ProtectedToothBehaviours tooth)
        {
            tooth.ScoreMultiplier(scoreToAdd);
        }

        public void Freeze() => moveSpeed = 0;

    }
}