using Enemies.AI.FiniteStateMachines;

namespace Enemies.AI.States
{
    public class IdleState : State
    {
        private readonly float _triggerRange;
        
        public IdleState(FiniteStateMachine newFSM) : base(newFSM)
        {
            _triggerRange = fsm.Owner.Stats.TriggerRange*fsm.Owner.Stats.TriggerRange;
        }
        
        public override void CheckTransition()
        {
            
        }

        public override void OnEnter()
        {
        
        }

        public override void Update()
        {
        
        }

        public override void OnExit()
        {
        
        }
    }
}
