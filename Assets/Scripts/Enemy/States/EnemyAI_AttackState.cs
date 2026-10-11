using UnityEngine;

public class EnemyAI_AttackState : EnemyAI_EntityState
{
    public EnemyAI_AttackState(EnemyAI enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetIsAttacking(true);
        FacingPlayer();
    }

    public override void Update()
    {
        if (enemy.alive)
        {
            base.Update();

            if (isTriggerCalled)
            {
                enemy.SetIsAttacking(false);
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

    private void FacingPlayer()
    {
        enemy.transform.forward = enemy.player.transform.position - enemy.transform.position;
    }
}
