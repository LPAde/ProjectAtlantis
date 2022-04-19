namespace Enemies.AI
{
    public class FiniteStateMachine
    {
        protected BaseEnemy owner;
        public State CurrentState { get; private set; }

        public State IdleState { get; }
        public State FightState { get; }
        public State WalkState { get; }

        public BaseEnemy Owner => owner;
        
        public FiniteStateMachine (BaseEnemy newOwner)
        {
            owner = newOwner;
            FightState = new FightState(this);
            WalkState = new WalkToPlayerState(this);
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
