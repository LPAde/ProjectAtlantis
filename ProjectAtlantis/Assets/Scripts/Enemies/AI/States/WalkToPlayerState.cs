using Enemies.AI.FiniteStateMachines;
using UnityEngine;

namespace Enemies.AI.States
{
    public class WalkToPlayerState : State
    {
        private const float MAXTargetTime = .4f;
        private float _targetTime;
        
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
            _targetTime -= Time.deltaTime;

            if (!(_targetTime < 0))
                return;
            
            fsm.Owner.WalkToPlayer();
            _targetTime = MAXTargetTime;
        }

        public override void OnExit()
        {
            fsm.Owner.Stop();
        }
    }
}
