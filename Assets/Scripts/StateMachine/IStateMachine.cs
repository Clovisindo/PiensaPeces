namespace Game.StateMachineManager
{
    public interface IStateMachine
    {
        IState currentState { get; }
        void ChangeState(IState newState);
        void Update();
    }
}
