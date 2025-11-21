using UnityEngine;

public class EnemyDamageOnContact : MonoBehaviour
{
    [Header("Dano ao Player")]
    public int damageAmount = 10;

    [Header("Alcance do ataque")]
    public float attackRange = 1.5f;

    [Header("Cooldown de dano contínuo")]
    public float damageCooldown = 0.5f;

    [Header("Debug")]
    public bool debugLogs = false;

    private float nextDamageTime = 0f;

    // TRIGGER
    void OnTriggerEnter2D(Collider2D other)
    {
        TryDamage(other, "OnTriggerEnter2D");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TryDamage(other, "OnTriggerStay2D");
    }

    // COLLISION
    void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.collider, "OnCollisionEnter2D");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        TryDamage(collision.collider, "OnCollisionStay2D");
    }

    void TryDamage(Collider2D other, string source)
    {
        PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null)
        {
            if (debugLogs)
                Debug.Log($"[EnemyDamage] {name}: sem PlayerHealth em {other.name} ({source})");
            return;
        }

        if (Time.time < nextDamageTime)
            return;

        float dist = Vector2.Distance(transform.position, ph.transform.position);
        if (dist > attackRange)
        {
            if (debugLogs)
                Debug.Log($"[EnemyDamage] {name}: longe demais (dist={dist:F2} > range={attackRange})");
            return;
        }

        ph.TakeDamage(damageAmount);
        nextDamageTime = Time.time + damageCooldown;

        if (debugLogs)
            Debug.Log($"[EnemyDamage] {name} ({source}) deu {damageAmount} de dano. Dist={dist:F2}. Vida atual={ph.currentHealth}");
    }
}
