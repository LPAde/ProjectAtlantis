namespace Enemies.AI
{
    public class FightState : State
    {
        private readonly float _attackRange;
        
        public FightState(FiniteStateMachine newFSM) : base(newFSM)
        {
            _attackRange = fsm.Owner.Stats.AttackRange * fsm.Owner.Stats.AttackRange;
        }
        
        public override void CheckTransition()
        {
            float distance = (fsm.Owner.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude;

            if(distance > _attackRange)
                fsm.Transition(fsm.WalkState);
        }

        public override void OnEnter()
        {
        }

        public override void Update()
        {
            fsm.Owner.Attack();
        }

        public override void OnExit()
        {
        }
    }
}
