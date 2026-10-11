using UnityEngine;

public class Player_ThrowState : Player_EntityState
{
    public Player_ThrowState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (isTriggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
