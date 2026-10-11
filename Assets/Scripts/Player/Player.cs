using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : CharacterEntity
{
    public NinjaActionSet input;

    #region Public Parameters
    public Vector2 moveInput { get; private set; }
    public Camera cam { get; private set; }
    #endregion

    #region Player` States
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_AttackState attackState { get; private set; }
    public Player_ThrowState throwState { get; private set; }
    public Player_DamageState damageState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    #endregion

    #region Movement Details
    [Header("Movement Details")]
    [SerializeField] private float moveLerpSpeed;
    [SerializeField] private float onGroundMoveSpeed;
    [SerializeField] private float inAirMoveSpeed;
    public float turnSpeed;
    public float attackVelocityTime = 0.1f;
    #endregion

    #region Attack Details
    [Header("Attack Details")]
    public List<AudioClip> comboAmountSound;
    public float comboAttackResetTime = 1.0f;
    [SerializeField] private float delayAfterHitEnemy;
    private Coroutine attackQueueCoroutine;
    #endregion

    #region VFX
    [Header("VFX")]
    [SerializeField] private CameraController camControl;
    [SerializeField] private GameObject stompVFX;
    [SerializeField] private float stompVFXSize;
    [SerializeField] private float stompVFXDistance;
    #endregion

    private Vector3 direction = Vector3.zero;
    private bool isOnGround = true;
    private float hitStopTime = 0;
    private Rigidbody rb;

    private Player_Health playerHealth;
    
    public static event Action OnPlayerDead;

    protected override void Awake()
    {
        base.Awake();

        input = new NinjaActionSet();
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<Player_Health>();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "walk");
        attackState = new Player_AttackState(this, stateMachine, "attack");
        throwState = new Player_ThrowState(this, stateMachine, "throw");
        damageState = new Player_DamageState(this, stateMachine, "damage");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += value => moveInput = value.ReadValue<Vector2>();
        input.Player.Move.canceled += value => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialized(idleState);
        //OnAttackAnimHandle();
        SetIsAttacking(false);
    }

    public void SetOnDeadEvent(Action onPlayerEvent)
    {
        OnPlayerDead = onPlayerEvent;
    }

    public void SetVelocity(float xVelocity, float zVeloctiy)
    {
        Vector3 forwardDirect = cam.transform.forward;
        Vector3 rightDirect = cam.transform.right;

        forwardDirect.y = 0;
        rightDirect.y = 0;

        forwardDirect.Normalize();
        rightDirect.Normalize();

        Vector3 newDirection = (forwardDirect * zVeloctiy) + (rightDirect * xVelocity);

        if (newDirection.magnitude > 1.0f)
            newDirection.Normalize();

        direction = Vector3.Lerp(direction, newDirection, moveLerpSpeed);

        anim.SetFloat("running", newDirection.magnitude);

        if (direction != Vector3.zero)
        {
            //transform.position += direction * (isOnGround ? onGroundMoveSpeed : inAirMoveSpeed) * Time.deltaTime;
            rb.MovePosition(rb.position + direction * (isOnGround ? onGroundMoveSpeed : inAirMoveSpeed) * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.fixedDeltaTime);
        }
    }

    public void EnterAttackWithDelay()
    {
        if (attackQueueCoroutine != null)
            StopCoroutine(attackQueueCoroutine);

        attackQueueCoroutine = StartCoroutine(EnterAttackDelayCoroutine());
    }

    IEnumerator EnterAttackDelayCoroutine()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(attackState);
    }

    public override void SetEquipWeapon(EquipWeapon weapon)
    {
        base.SetEquipWeapon(weapon);

        currentWeapon.InitializeWeapon(ShakeCamera);
    }

    private void CancelAttackComboQueue()
    {
        StopCoroutine(attackQueueCoroutine);
    }

    public override void InstantiateStompGroundEffect()
    {
        base.InstantiateStompGroundEffect();

        GameObject groundStompEffect = Instantiate(stompVFX, transform.position + transform.forward * stompVFXDistance, Quaternion.Euler(-90, 0, 0));
        groundStompEffect.transform.localScale = Vector3.one * stompVFXSize;
        Destroy(groundStompEffect, 1.5f);
    }

    private void OnAttackAnimHandle()
    {
        if (hitStopTime > 0.0f)
        {
            anim.speed = 0.0f;
            hitStopTime -= Time.fixedDeltaTime;
        }
        else
            anim.speed = 1.0f;
    }

    private void ShakeCamera()
    {
        camControl.ShakeCamera(transform.right * 0.1f);
        hitStopTime = delayAfterHitEnemy;
    }

    public override void OnDead()
    {
        base.OnDead();

        Debug.LogWarning("Has died!!");
        OnPlayerDead?.Invoke();
        StopPlayer();
        GameSceneController.instance.OnGameOver(false);
    }

    public void StopPlayer()
    {
        camControl.SetActiveCamera(false);
        OnDisable();
    }

    public override void PlaySoundOnAttack()
    {
        weaponAudioSource.clip = comboAmountSound[attackState.attackIndex - 1];
        weaponAudioSource.Play(0);
    }

    public void Heal(int healPoint) => playerHealth.Heal(healPoint);
}
    
