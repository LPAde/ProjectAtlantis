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
            float distance = (fsm.Owner.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude;
            
            if(distance < _triggerRange)
                fsm.Transition(fsm.WalkState);
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
