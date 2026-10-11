using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : CharacterEntity
{
    #region States
    public EnemyAI_IdleState idleState { get; private set; }
    public EnemyAI_AlertState alertState { get; private set; }
    public EnemyAI_BattleState battleState { get; private set; }
    public EnemyAI_AttackState attackState { get; private set; }
    public EnemyAI_DamageState damageState { get; private set; }
    public EnemyAI_DeadState deadState { get; private set; }
    #endregion

    #region Alert Details
    [Header("Alert Details")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private float waitFromTurnSecs;
    #endregion

    #region Attack Details
    [Header("Attack Details")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackDistanceThreshold = 0.25f;
    #endregion

    #region Detect Details
    [Header("Detect Details")]
    public Vector3 playerDetectOffset;
    public float giveUpSearchForPlayerTime;
    #endregion

    #region VFX
    [Header("VFX")]
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private Transform explosionTransform;
    [SerializeField] private Vector3 particleSpawnOffset;
    [SerializeField] private float particleSpawnSize = 0.8f;
    [SerializeField] private float DestroyParticleInSecs = 1.5f;
    [SerializeField] private float DestroyGameObjectInSecs = 0.2f;
    #endregion

    [Header("Debug purposes")]
    public GameObject spawnTestObj;

    public Player player { get; private set; }
    public TransformData damageDealer { get; private set; }
    public EnemyAI_PlayerDetector playerDetector { get; private set; }

    private NavMeshAgent agent;
    private ItemDropper itemDropper;
    //private Rigidbody rb;

    private Vector3 turningToPoint;
    private Coroutine waitFromTurnCoroutine;
    private float turningValue;

    private Action OnEnemyDeadAction;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        itemDropper = GetComponent<ItemDropper>();
        //rb = GetComponent<Rigidbody>();
        playerDetector = GetComponent<EnemyAI_PlayerDetector>();

        idleState = new EnemyAI_IdleState(this, stateMachine, "idle");
        alertState = new EnemyAI_AlertState(this, stateMachine, "walk");
        battleState = new EnemyAI_BattleState(this, stateMachine, "walk");
        attackState = new EnemyAI_AttackState(this, stateMachine, "attack");
        damageState = new EnemyAI_DamageState(this, stateMachine, "damage");
        deadState = new EnemyAI_DeadState(this, stateMachine, "dead");

        damageDealer = new TransformData();
    }

    public void InitializeEnemy(Action OnEnemyDeadAction)
    {
        this.OnEnemyDeadAction = OnEnemyDeadAction;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialized(idleState);
    }

    public void SetStopAgent(bool isStop)
    {
        agent.isStopped = isStop;
    }

    public override void OnDamageEnd()
    {
        stateMachine.ChangeState(idleState);
    }

    public override void OnDead()
    {
        base.OnDead();

        SetStopAgent(true);
    }

    public void EndDeadSequence()
    {
        GameObject onDiedParicle = Instantiate(explosionParticle, explosionTransform.position + particleSpawnOffset, Quaternion.identity);
        onDiedParicle.transform.localScale = Vector3.one * particleSpawnSize;
        AudioSource.PlayClipAtPoint(explosionSound, explosionTransform.position, 1.0f);

        Destroy(onDiedParicle, DestroyParticleInSecs);

        OnEnemyDeadAction?.Invoke();
        itemDropper.DropItem();

        Destroy(gameObject, DestroyGameObjectInSecs);
    }

    public void MoveToTarget()
    {
        Vector3 vectorToPlayer = (transform.position - player.transform.position).normalized;
        //Calculate the point enemy should walk to from player's position + radius(attackDistance) for having a little space for attacking;
        agent.SetDestination(player.transform.position/* + (vectorToPlayer * attackDistance)*/);
        agent.stoppingDistance = attackDistance;
    }

    public bool IsCloseEnoughToAttack()
    {
        return Vector3.Distance(player.transform.position, transform.position) <= (attackDistance + attackDistanceThreshold);
    }

    public void StopChasingPlayer()
    {
        SetStopAgent(true);

        stateMachine.ChangeState(idleState);
        player = null;
    }

    public void OnAlert(TransformData damageDealer)
    {
        this.damageDealer = damageDealer;
    }

    public void TryGetIntoAlertState()
    {
        if (stateMachine.currentState != idleState && stateMachine.currentState != alertState && stateMachine.currentState != damageState)
            return;

        stateMachine.ChangeState(alertState);
    }

    public void StartTurnToTarget()
    {
        Vector3 pointToTurn = damageDealer.position - transform.position;
        turningToPoint = pointToTurn.normalized;
        turningValue = 0;
    }

    public void TurningTwo()
    {
        //direction = Vector3.Lerp(direction, turnVector, lerpSpeed);
        turningValue += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turningToPoint), turningValue);
    }

    public void StartWaitingFromTurn()
    {
        if (waitFromTurnCoroutine != null)
            StopCoroutine(waitFromTurnCoroutine);

        waitFromTurnCoroutine = StartCoroutine(WaitFromTurn());
    }

    private IEnumerator WaitFromTurn()
    {
        yield return new WaitForSeconds(waitFromTurnSecs);

        //if (Vector3.Distance(damageDealer.position, transform.position) < 1.3f)
        //{
        //    DetectedPlayer(damageDealer.gameObject.GetComponent<Collider>());
        //    yield break;
        //}

        Vector3 lookDirection = damageDealer.position - transform.position;
        RaycastHit seePlayer = playerDetector.LineOfSight(lookDirection);

        if (seePlayer.collider == null)
        {
            stateMachine.ChangeState(idleState);
            Debug.LogWarning("Not Found any thing!!");
            yield break;
        }

        DetectedPlayer(seePlayer.collider);
    }

    private void DetectedPlayer(Collider detecteCollider)
    {
        if (detecteCollider.CompareTag("Player"))
        {
            Player player = detecteCollider.GetComponent<Player>();
            if (player != null)
            {
                SetPlayer(player);
                //Debug.LogWarning("Found you!!");
                stateMachine.ChangeState(battleState);
            }
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        player.SetOnDeadEvent(HandlePlayerDeath);
    }

    private void HandlePlayerDeath()
    {
        player = null;
        stateMachine.ChangeState(idleState);
    }

    public void EnableDetectingForPlayer(bool enable) => playerDetector.SetIsDetecting(enable);

    private void OnCollisionEnter(Collision collision)
    {
        if (alive)
            DetectedPlayer(collision.collider);
    }

    public override void PlaySoundOnAttack()
    {
        weaponAudioSource.Play(0);
    }
}
