using UnityEngine;

public class EnemyAI_Health : Entity_Health
{
    private EnemyAI enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyAI>();
    }

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool didDamage = base.TakeDamage(damage, damageDealer);

        if (!didDamage)
            return false;

        enemy.OnAlert(new TransformData(damageDealer));

        if (currentHealth <= 0)
        {
            enemy.stateMachine.ChangeState(enemy.deadState);
            enemy.OnDead();
        }
        else
            enemy.stateMachine.ChangeState(enemy.damageState);

        return true;
    }
}
