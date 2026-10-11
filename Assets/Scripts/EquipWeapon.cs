using UnityEngine;
using UnityEngine.Audio;
using static EquipWeapon;

public class EquipWeapon : MonoBehaviour
{
    public delegate void OnHitEnemy();
    private OnHitEnemy onHitEnemy;
    
    [Header("Combat Details")]
    [SerializeField] private float damage;
    [SerializeField] private float damageForce;

    [Header("VFX Details")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject flashVFX;
    [SerializeField] private float destroyParticleInSecs;
    [Space]
    [SerializeField] private GameObject trailObject;

    public float GetDamage { get { return damage; } }

    private AudioSource audioSource;

    public void InitializeWeapon(OnHitEnemy onHitEnemy)
    {
        this.onHitEnemy = onHitEnemy;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetTrailActive(false);
    }

    public void SetTrailActive(bool active) => trailObject.SetActive(active);

    public void DidDamage(Vector3 position)
    {
        audioSource.Play(0);

        GameObject hitEffect = Instantiate(hitVFX, position, Quaternion.identity);
        Destroy(hitEffect, destroyParticleInSecs);

        GameObject flashEffect = Instantiate(flashVFX, position, Quaternion.identity);
        Destroy(flashEffect, destroyParticleInSecs);
    }
}
