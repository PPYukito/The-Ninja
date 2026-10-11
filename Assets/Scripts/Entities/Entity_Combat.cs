using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    #region Attack Details
    [Header("Attack Details")]
    [SerializeField] private Transform targetCheckTransform;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask targetLayer;
    #endregion

    #region VFX
    [SerializeField] private Vector2 xRandomPoint;
    [SerializeField] private Vector2 yRandomPoint;
    [SerializeField] private Vector2 zRandomPoint;
    #endregion

    [Header("Debug")]
    public bool debug;
    public GameObject debugObj;

    protected CharacterEntity character;
    protected Collider[] targets = new Collider[0];

    private void Awake()
    {
        character = GetComponent<CharacterEntity>();
    }

    private void FixedUpdate()
    {
        if (character.isAttacking)
            GetTargetColliders();
    }

    private void GetTargetColliders()
    {
    }

    public void PerformAttack()
    {
        targets = Physics.OverlapSphere(targetCheckTransform.position, targetCheckRadius, targetLayer/*, QueryTriggerInteraction.Collide*/);
        foreach (Collider target in targets)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            //Entity_Health entityHealth = target.GetComponent<Entity_Health>();

            if (damagable == null)
            {
                Debug.LogWarning($"damageable = null\n");
                continue;
            }
            //Debug.LogWarning($"damageable found!!\n");

            bool gotHit = damagable.TakeDamage(character.currentWeapon.GetDamage, transform);

            if (gotHit)
            {
                //Debug.LogWarning($"got hit!!\n");
                character.currentWeapon.DidDamage(target.transform.position + RandomHitPoint());
            }
        }
    }

    private Vector3 RandomHitPoint()
    {
        return new Vector3(Random.Range(xRandomPoint.x, xRandomPoint.y), Random.Range(yRandomPoint.x, yRandomPoint.y), Random.Range(zRandomPoint.x, zRandomPoint.y));
    }

    private void OnDrawGizmos()
    {
        /*if (_debugSpherePosition == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_debugSpherePosition, targetCheckRadius);*/
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheckTransform.position, targetCheckRadius);
    }
}
