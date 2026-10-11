using UnityEngine;

public class Player_GroundState : Player_EntityState
{
    public Player_GroundState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetIsAttacking(false);
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Attack.IsPressed() && !player.isAttacking)
            stateMachine.ChangeState(player.attackState);

        if (input.Player.Shoot.IsPressed())
            stateMachine.ChangeState(player.throwState);
    }
}
