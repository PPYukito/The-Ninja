using UnityEngine;

public class EnemyAI_DamageState : EnemyAI_EntityState
{
    public EnemyAI_DamageState(EnemyAI enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        if (enemy.alive)
        {
            base.Update();

            if (isTriggerCalled)
                enemy.TryGetIntoAlertState();
        }
    }
}
