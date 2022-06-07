using Enemies.AI.FiniteStateMachines;

namespace Enemies.AI.States
{
    public abstract class State
    {
        protected readonly FiniteStateMachine fsm;

        protected State(FiniteStateMachine newFSM)
        {
            fsm = newFSM;
        }
        
        public abstract void CheckTransition();

        public abstract void OnEnter();

        public abstract void Update();
        
        public abstract void OnExit(); 
    }
}
