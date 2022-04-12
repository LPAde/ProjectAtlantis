namespace Enemies.AI
{
    public abstract class State 
    {
        
        
        public abstract void CheckTransition();

        public abstract void OnEnter();

        public abstract void Update();
        
        public abstract void OnExit(); 
    }
}
