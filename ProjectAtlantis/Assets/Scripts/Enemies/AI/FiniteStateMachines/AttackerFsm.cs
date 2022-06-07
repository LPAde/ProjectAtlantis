namespace Enemies.AI.FiniteStateMachines
{
    public class AttackerFsm : FiniteStateMachine
    {
        protected AttackingEnemy attackingOwner;

        public AttackerFsm(AttackingEnemy newOwner) : base(newOwner)
        {
            attackingOwner = newOwner;
        }
    }
}
