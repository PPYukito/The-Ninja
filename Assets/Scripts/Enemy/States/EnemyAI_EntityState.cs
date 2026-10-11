using UnityEngine;

public class EnemyAI_EntityState : EntityState
{
    protected EnemyAI enemy;
    protected Rigidbody rb;

    public EnemyAI_EntityState(EnemyAI enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
        this.enemy = enemy;
        rb = enemy.GetComponent<Rigidbody>();
    }
}
