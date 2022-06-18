using Enemies.AI.States;

namespace Enemies.AI.FiniteStateMachines
{
    public class FiniteStateMachine
    {
        protected BaseEnemy owner;
        public State CurrentState { get; private set; }

        public State IdleState { get; }
        public State WalkState { get; }
        public State StunState { get; }

        public BaseEnemy Owner => owner;
        
        public FiniteStateMachine (BaseEnemy newOwner)
        {
            owner = newOwner;
            IdleState = new IdleState(this);
            WalkState = new WalkToPlayerState(this);
            StunState = new StunState(this);
        }
        
        public void Initialize(State initialState)
        {
            CurrentState = initialState;
            
            CurrentState.OnEnter();
        }

        public void Update()
        {
            CurrentState.CheckTransition();
            
            CurrentState.Update();
        }
        
        /// <summary>
        /// Changes State and makes the states do their respective onexit and onenter.
        /// </summary>
        /// <param name="newState"> The state you want to transition to. </param>
        public void Transition(State newState)
        {
            CurrentState.OnExit();
            CurrentState = newState;
            CurrentState.OnEnter();
        }
    }
}
