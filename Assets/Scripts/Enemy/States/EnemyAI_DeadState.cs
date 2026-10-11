using UnityEngine;

public class EnemyAI_DeadState : EnemyAI_EntityState
{
    public EnemyAI_DeadState(EnemyAI enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StopMoving();
    }

    private void StopMoving()
    {
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
