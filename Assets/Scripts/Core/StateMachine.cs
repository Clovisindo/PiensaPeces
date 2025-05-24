using System;

public class StateMachine
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType()) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update() => currentState?.Update();

    //public Type GetCurrentStateType()
    //{
    //    return currentState?.GetType();
    //}
}
