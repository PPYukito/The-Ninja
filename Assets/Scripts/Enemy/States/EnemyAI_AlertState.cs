using UnityEngine;

public class EnemyAI_AlertState : EnemyAI_EntityState
{
    public EnemyAI_AlertState(EnemyAI enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StartTurnToTarget();
        enemy.StartWaitingFromTurn();
    }

    public override void Update()
    {
        if (enemy.alive)
        {
            base.Update();
            enemy.TurningTwo();
        }

    }
}
