using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Player_AttackState : Player_EntityState
{
    private const int firstAttackIndex = 1;

    public int attackIndex { get; private set;}

    private float attackVelocityTime;
    private float lastTimeAttack;
    private bool comboAttackQueued;

    public Player_AttackState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        attackIndex = firstAttackIndex;
    }

    public override void Enter()
    {
        base.Enter();

        comboAttackQueued = false;

        ResetComboAttackIndex();

        player.SetIsAttacking(true);
        anim.SetInteger("attackIndex", attackIndex);
        ApplyAttackVelocity();

        AttackingTurn();
    }

    public override void Update()
    {
        base.Update();

        HandleAttackVeloctity();
        AttackingTurn();

        //if (input.Player.Attack.WasPressedThisFrame())
        //    QueueComboAttack();

        if (isTriggerCalled)
            HandleStateExit();
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            base.Exit();
            player.EnterAttackWithDelay();
        }
        else
        {
            attackIndex++;
            lastTimeAttack = Time.time;
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void QueueComboAttack()
    {
        if (attackIndex < player.comboAmountSound.Count)
            comboAttackQueued = true;
    }

    private void ApplyAttackVelocity()
    {
        attackVelocityTime = player.attackVelocityTime;

        //Later
        //player.SetVelocity(1, 1);
    }

    private void HandleAttackVeloctity()
    {
        attackVelocityTime -= Time.deltaTime;

        if (attackVelocityTime < 0)
            player.SetVelocity(0, 0);
    }

    private void AttackingTurn()
    {
        Vector3 lookAt = player.cam.transform.forward;
        lookAt.y = 0f;
        player.transform.rotation = Quaternion.Slerp(
            player.transform.rotation,
            Quaternion.LookRotation(lookAt),
            player.turnSpeed * Time.fixedDeltaTime);
    }

    private void ResetComboAttackIndex()
    {
        if (Time.time > lastTimeAttack + player.comboAttackResetTime)
            attackIndex = firstAttackIndex;

        if (attackIndex > player.comboAmountSound.Count)
            attackIndex = firstAttackIndex;
    }
}
