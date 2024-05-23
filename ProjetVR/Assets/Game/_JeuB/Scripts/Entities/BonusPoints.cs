using UnityEngine;

namespace JeuB
{
    public class BonusPoints : Bonus
    {
        [SerializeField] int scoreToAdd;

        protected override void EntityStart()
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        protected override void EntityUpdate() { }

        public override void ApplyBonus(ProtectedToothBehaviours tooth)
        {
            tooth.ScoreMultiplier(scoreToAdd);
        }

        public override void Move()
        {
            Vector3 direction = (target.position - transform.position).normalized;

            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }

        public void Freeze() => moveSpeed = 0;

    }
}