using UnityEngine;

namespace Enemies.AI
{
    public class FiniteStateMachine
    {
        protected BaseEnemy owner;
        private State currentState;

        protected FiniteStateMachine (BaseEnemy newOwner)
        {
            owner = newOwner;
        }

        /// <summary>
        /// Changes State and makes the states do their respective onexit and onenter.
        /// </summary>
        /// <param name="newState"> The state you want to transition to. </param>
        public void Transition(State newState)
        {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }
}
