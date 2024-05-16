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
            throw new System.NotImplementedException();
        }

        protected override void EntityUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
