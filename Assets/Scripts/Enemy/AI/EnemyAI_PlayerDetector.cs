using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAI_PlayerDetector : MonoBehaviour
{
    [SerializeField] private float waitToLostPlayer;

    #region Player Detect Details
    [Header("Player Detect Details")]
    [SerializeField] private Transform playerDetectTrannsform;
    [SerializeField] private float detectRadius;
    [SerializeField] private float detectAngle;
    [SerializeField] private LayerMask playerLayer;
    #endregion

    #region Line of Sight Detect Details
    [Header("Line of Sight Details")]
    [SerializeField] private float lineOfSightDistance;
    [SerializeField] private LayerMask lineOfSightLayer;
    #endregion

    private EnemyAI enemy;
    private Coroutine lostPlayerCoroutine;
    private bool isDetecting = true;

    private void Awake()
    {
        enemy = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (isDetecting)
            OverlapSphereDetecting();
    }

    public void SetIsDetecting(bool detecting) => isDetecting = detecting;

    private void OverlapSphereDetecting()
    {
        Collider[] detectedObjct = DetectPlayer();
        //Debug.LogWarning(detectedObjct.Length);
        foreach (Collider collider in detectedObjct)
        {
            Vector3 vectorToPlayer = collider.transform.position - playerDetectTrannsform.position;
            vectorToPlayer.y = 0;
            Debug.DrawRay(playerDetectTrannsform.position, vectorToPlayer, Color.yellow);

            Player_Health playerHP = collider.GetComponent<Player_Health>();
            if (playerHP.GetHP() <= 0)
                continue;

            RaycastHit hit = LineOfSight();
            if (hit.collider == null)
                continue;

            if (hit.collider.CompareTag("Player"))
            {
                if (Vector3.Angle(playerDetectTrannsform.forward, vectorToPlayer) < detectAngle)
                {
                    Player player = collider.GetComponent<Player>();
                    if (player != null)
                        enemy.SetPlayer(player);
                }
            }
        }
    }

    //private void StartCountDownLostPlayer()
    //{
    //    if (lostPlayerCoroutine != null)
    //        StopCoroutine(lostPlayerCoroutine);

    //    lostPlayerCoroutine = StartCoroutine(CountDownToLostPlayer());
    //}

    //IEnumerator CountDownToLostPlayer()
    //{
    //    yield return new WaitForSeconds(waitToLostPlayer);
    //    enemy.StopChasingPlayer();
    //}

    private Collider[] DetectPlayer()
    {
        return Physics.OverlapSphere(enemy.transform.position, detectRadius, playerLayer);
    }

    public RaycastHit LineOfSight()
    {
        RaycastHit hit = default;
        Vector3 lookDirection = playerDetectTrannsform.forward;
        lookDirection.y = 0;

        Physics.Raycast(
            playerDetectTrannsform.position,
            lookDirection,
            out hit,
            lineOfSightDistance,
            lineOfSightLayer);
        return hit;
    }

    public RaycastHit LineOfSight(Vector3 lookDirection)
    {
        RaycastHit hit = default;
        Physics.Raycast(
            playerDetectTrannsform.position,
            lookDirection,
            out hit,
            lineOfSightDistance,
            lineOfSightLayer);
        //Debug.DrawRay(playerDetectTrannsform.position, lookDirection, Color.yellow, 120.0f);
        return hit;
    }

    public RaycastHit LineOfSight(Vector3 originPoint, Vector3 lookDirection)
    {
        RaycastHit hit = default;
        Physics.Raycast(
            originPoint,
            lookDirection,
            out hit,
            lineOfSightDistance,
            lineOfSightLayer);
        return hit;
    }

    private void OnDrawGizmos()
    {
        if (enemy != null)
            Gizmos.DrawWireSphere(enemy.transform.position, detectRadius);

        Vector3 lookDirection = playerDetectTrannsform.forward;
        lookDirection.y = 0;
        Gizmos.DrawRay(playerDetectTrannsform.position, lookDirection * lineOfSightDistance);
    }
}
