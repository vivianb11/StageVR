using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JeuB {
    public class BonusMob : Bonus
    {
        [SerializeField] int scoreToAdd;

        protected override void EntityStart() {}

        protected override void EntityUpdate() {}

        public override void ApplyBonus(ProtectedToothBehaviours tooth)
        {
            tooth.enemyPoints = scoreToAdd;
            tooth.ScoreMultiplier();
        }

        public void Freeze() => moveSpeed = 0;

    }
}