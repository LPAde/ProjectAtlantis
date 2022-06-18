using Enemies.AI.FiniteStateMachines;

namespace Enemies.AI.States
{
    public class StunState : State
    {
        private float stunTime;
        
        public override void CheckTransition()
        {
            if(stunTime < 0)
                fsm.Transition(fsm.IdleState);
        }

        public override void OnEnter()
        {
        }

        public override void Update()
        {
            stunTime = fsm.Owner.EndureStun();
        }

        public override void OnExit()
        {
        }

        public StunState(FiniteStateMachine newFSM) : base(newFSM)
        {
        }
    }
}
