using Enemies.AI.FiniteStateMachines;

namespace Enemies.AI.States
{
    public class WalkToPlayerState : State
    {
        private readonly float _attackRange;
        
        public WalkToPlayerState(FiniteStateMachine newFSM) : base(newFSM)
        {
            _attackRange = fsm.Owner.Stats.AttackRange * fsm.Owner.Stats.AttackRange;
        }

        public override void CheckTransition()
        {
            float distance = (fsm.Owner.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude;

            if(distance < _attackRange)
                fsm.Transition(fsm.FightState);
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
