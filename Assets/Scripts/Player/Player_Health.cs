using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool didDamage = base.TakeDamage(damage, damageDealer);

        if (!didDamage)
            return false;

        if (currentHealth <= 0)
            player.stateMachine.ChangeState(player.deadState);
        else
            player.stateMachine.ChangeState(player.damageState);

        return true;
    }

    public void Heal(int healPoint)
    {
        currentHealth += healPoint;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //play sound and particle effect when heal;
        UpdateHealthBar();
    }
}
