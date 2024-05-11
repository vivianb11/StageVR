using UnityEngine;

namespace JeuB
{
    public class BonusPoints : Bonus
    {
        [SerializeField] int scoreToAdd;

        protected override void EntityStart() { }

        protected override void EntityUpdate() { }

        public override void ApplyBonus(ProtectedToothBehaviours tooth)
        {
            tooth.enemyPoints = scoreToAdd;
            tooth.ScoreMultiplier();
        }

        public void Freeze() => moveSpeed = 0;

    }
}