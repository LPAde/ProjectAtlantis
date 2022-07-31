using Enemies.AI.FiniteStateMachines;

namespace Enemies.AI.States
{
    public class FightState : State
    {
        private AttackingEnemy owner;
        private readonly float _attackRange;
        
        public FightState(AttackerFsm newFSM) : base(newFSM)
        {
            _attackRange = fsm.Owner.Stats.AttackRange * fsm.Owner.Stats.AttackRange;
            owner = (AttackingEnemy)newFSM.Owner;
        }
        
        public override void CheckTransition()
        {
            float distance = (fsm.Owner.transform.position - GameManager.Instance.Player.PlayerController.transform.position).sqrMagnitude;

            if(distance > _attackRange)
                fsm.Transition(fsm.WalkState);
        }

        public override void OnEnter()
        {
            owner.Stop();
        }

        public override void Update()
        {
            owner.transform.LookAt(GameManager.Instance.Player.PlayerController.transform);
            owner.Attack();
        }

        public override void OnExit()
        {
           
        }
    }
}
