using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI_BattleState : EnemyAI_EntityState
{
    private EnemyAI_PlayerDetector playerDetector;
    private float lastSawPlayerTime;

    public EnemyAI_BattleState(EnemyAI enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
        playerDetector = enemy.playerDetector;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetStopAgent(false);
        lastSawPlayerTime = float.MaxValue;
    }

    public override void Update()
    {
        if (enemy.alive)
        {
            base.Update();

            enemy.MoveToTarget();

            if (InAttackDistance())
                stateMachine.ChangeState(enemy.attackState);

            if (Time.time - lastSawPlayerTime > enemy.giveUpSearchForPlayerTime)
                enemy.StopChasingPlayer();
        }
    }

    private bool InAttackDistance()
    {
        if (Detecting())
        {
            lastSawPlayerTime = Time.time;
            //Debug.LogWarning("I am seeing you");
            return enemy.IsCloseEnoughToAttack();
        }

        return false;
    }

    private bool Detecting()
    {
        Vector3 lookingDirection = enemy.player.transform.position - enemy.transform.position;
        lookingDirection.y = 0;
        RaycastHit seePlayer = playerDetector.LineOfSight(lookingDirection);
        Debug.DrawRay(playerDetector.transform.position, lookingDirection, Color.blue);

        if (seePlayer.collider == null)
            return false;

        if (seePlayer.collider.CompareTag("Player"))
            return true;

        return false;
    }
}
