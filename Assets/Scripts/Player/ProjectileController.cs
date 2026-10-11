using UnityEngine;
using UnityEngine.TextCore.Text;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float destroyIn;
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private AudioClip onHitClip;
    [SerializeField] private AudioClip onBounceClip;

    private void Start()
    {
        if (destroyIn > 0)
            Destroy(gameObject, destroyIn);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.TakeDamage(damage, transform);

            GameObject hitParticle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(onHitClip, transform.position, 1.0f);
            Destroy(hitParticle, 2.0f);
        }
        else
            AudioSource.PlayClipAtPoint(onBounceClip, transform.position, 1.0f);

        Destroy(gameObject);
    }
}
