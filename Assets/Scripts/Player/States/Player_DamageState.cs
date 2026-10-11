using UnityEngine;

public class Player_DamageState : Player_EntityState
{
    public Player_DamageState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (isTriggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
