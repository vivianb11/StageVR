using UnityEngine;

namespace JeuB
{
    public class BonusPoints : Bonus
    {
        [SerializeField] int scoreToAdd;

        protected override void EntityStart() { }

        protected override void EntityUpdate() 
        { 
            var rotation = Quaternion.LookRotation(Vector3.up , Vector3.forward);
            transform.GetChild(0).rotation = rotation;
        }

        public override void ApplyBonus(ProtectedToothBehaviours tooth)
        {
            tooth.enemyPoints = scoreToAdd;
            tooth.ScoreMultiplier();
        }

        public void Freeze() => moveSpeed = 0;

    }
}