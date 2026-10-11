using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamagable
{
    [Header("HP Details")]
    [SerializeField] protected float maxHealth;
    [SerializeField] private HealthBar healthBar;

    [Header("Audio Details")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deadSound;

    [Header("Debug")]
    [SerializeField] protected float currentHealth;

    public virtual void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (currentHealth <= 0)
            return false;

        currentHealth -= damage;

        if (currentHealth > 0)
            PlayDamageSound();
        else
            PlayDeadSound();

        UpdateHealthBar();

        return true;
    }

    protected void UpdateHealthBar() => healthBar.UpdateHealthBar(currentHealth / maxHealth);

    public float GetHP() => currentHealth;

    protected void PlayDamageSound() => AudioSource.PlayClipAtPoint(damageSound, transform.position, 1.0f);
    protected void PlayDeadSound() => AudioSource.PlayClipAtPoint(deadSound, transform.position, 1.0f);
}
