using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : MonoBehaviour
{
    public EquipWeapon currentWeapon;

    [Header("Audio Details")]
    [SerializeField] protected AudioSource weaponAudioSource;
    [Space]
    public List<AudioClip> walkSounds;
    [Tooltip("Check this bool to use AudioPlayAtPoint instead of using AudioSource component that attach to Character.")]
    [SerializeField] protected bool playMoveSoundAtPoint = false;
    [SerializeField] protected AudioSource moveAudioSource;

    public StateMachine stateMachine { get; protected set; }
    public Animator anim { get; protected set; }
    public bool isAttacking { get; protected set; }
    public bool alive { get; protected set; }

    private int walkSoundIndex;

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        anim = GetComponentInChildren<Animator>();
        walkSoundIndex = 0;
    }

    protected virtual void Start() => alive = true;
    
    protected virtual void FixedUpdate()
    {
        if (alive)
            stateMachine.UpdateActiveState();
    }

    public virtual void TriggerCurrentState() => stateMachine.currentState.CallStateTrigger();
    public virtual void EnableWeaponTrailYeah() => currentWeapon.SetTrailActive(true);
    public virtual void DisableWeaponTrailYeah() => currentWeapon.SetTrailActive(false);
    public virtual void InstantiateStompGroundEffect() { }
    public virtual void OnDamageEnd() { }
    public virtual void OnDead() => alive = false;
    public virtual void PlaySoundOnAttack() { }
    public virtual void PlaySoundOnMove()
    {
        if (playMoveSoundAtPoint)
            AudioSource.PlayClipAtPoint(walkSounds[walkSoundIndex], transform.position, 1.0f);
        else
            moveAudioSource.PlayOneShot(walkSounds[walkSoundIndex]);

        moveAudioSource.pitch = Random.Range(0.8f, 1.2f);

        walkSoundIndex++;
        if (walkSoundIndex >= walkSounds.Count)
            walkSoundIndex = 0;
    }

    public virtual void SetEquipWeapon(EquipWeapon weapon) => currentWeapon = weapon;

    public void SetIsAttacking(bool isAttacking) => this.isAttacking = isAttacking;
}
