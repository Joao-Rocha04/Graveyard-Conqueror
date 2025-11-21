using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class BossHealth : MonoBehaviour
{
    [Header("Vida do Boss")]
    public int maxHealth = 500;
    public int currentHealth;

    [Header("Animação de morte")]
    public Animator animator;
    public string dieTriggerName = "Die";

    public event Action<int, int> OnHealthChanged;
    public event Action OnBossDeath;

    private bool isDead = false;

    // referências para desligar comportamento ao morrer
    private BossBehavior bossBehavior;
    private EnemyDamageOnContact damageOnContact;

    void Awake()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();

        bossBehavior = GetComponent<BossBehavior>();
        damageOnContact = GetComponent<EnemyDamageOnContact>();

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("[BossHealth] Boss morreu!");

        // animação de morte
        if (animator != null && !string.IsNullOrEmpty(dieTriggerName))
        {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
            animator.ResetTrigger("Attack4");
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger(dieTriggerName);
        }

        // desliga IA e dano
        if (bossBehavior != null)
            bossBehavior.enabled = false;

        if (damageOnContact != null)
            damageOnContact.enabled = false;

        OnBossDeath?.Invoke();

        // opcional: Destroy(gameObject, 2f);
    }
}
