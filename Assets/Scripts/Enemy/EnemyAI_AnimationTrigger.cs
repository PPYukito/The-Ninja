using UnityEngine;

public class EnemyAI_AnimationTrigger : Entity_AnimationTrigger
{
    private EnemyAI enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = character.GetComponent<EnemyAI>();
    }

    private void OnDeadSequence() => enemy.EndDeadSequence();
}
