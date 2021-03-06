using Enemies.AI.FiniteStateMachines;

namespace Enemies.AI.States
{
    public class WalkToPlayerState : State
    {
        private readonly float _attackRange;
        
        public WalkToPlayerState(FiniteStateMachine newFSM) : base(newFSM)
        {
        }

        public override void CheckTransition()
        {
            
        }

        public override void OnEnter()
        {
        }

        public override void Update()
        {
            fsm.Owner.WalkToPlayer();
        }

        public override void OnExit()
        {
            fsm.Owner.Stop();
        }
    }
}
