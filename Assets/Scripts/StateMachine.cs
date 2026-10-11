using UnityEngine;

public class StateMachine
{
    public EntityState currentState;

    public void Initialized(EntityState enterState)
    {
        currentState = enterState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void ChangeState(EntityState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
