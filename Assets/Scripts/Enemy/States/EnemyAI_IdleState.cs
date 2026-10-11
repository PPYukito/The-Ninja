using UnityEngine;

public class EnemyAI_IdleState : EnemyAI_EntityState
{
    public EnemyAI_IdleState(EnemyAI enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetIsAttacking(false);
        enemy.EnableDetectingForPlayer(true);
    }

    public override void Update()
    {
        if (enemy.alive)
        {
            base.Update();

            if (enemy.player != null)
            {
                if (enemy.IsCloseEnoughToAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.battleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.EnableDetectingForPlayer(false);
    }
}
