using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    protected CharacterEntity character;
    protected Entity_Combat combat;

    protected virtual void Awake()
    {
        character = GetComponentInParent<CharacterEntity>();
        combat = GetComponentInParent<Entity_Combat>();
    }

    private void PerformAttack() => combat.PerformAttack();
    private void TriggerCurrentState() => character.TriggerCurrentState();
    private void EnableWeaponTrailYeah() => character.EnableWeaponTrailYeah();
    private void DisableWeaponTrailYeah() => character.DisableWeaponTrailYeah();
    private void InstantiateStompGroundEffect() => character.InstantiateStompGroundEffect();
    private void OnDead() => character.OnDead();
    private void PlaySoundOnAttack() => character.PlaySoundOnAttack();
    private void PlaySoundOnMove() => character.PlaySoundOnMove();
}
