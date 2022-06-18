using Enemies.AI.States;

namespace Enemies.AI.FiniteStateMachines
{
    public class AttackerFsm : FiniteStateMachine
    {
        protected AttackingEnemy attackingOwner;

        public State FightState { get; }

        public AttackerFsm(AttackingEnemy newOwner) : base(newOwner)
        {
            attackingOwner = newOwner;
        }
    }
}
