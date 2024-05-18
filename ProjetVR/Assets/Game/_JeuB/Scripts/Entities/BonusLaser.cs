namespace JeuB
{
    public class BonusLaser : Bonus
    {
        public override void ApplyBonus(ProtectedToothBehaviours tooth)
        {
            Laser.Instance.EnbleLaser();
        }

        protected override void EntityStart()
        {
        }

        protected override void EntityUpdate()
        {
        }
    }
}
