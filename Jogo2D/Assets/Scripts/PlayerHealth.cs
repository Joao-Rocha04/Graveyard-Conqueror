using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Vida do Player")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Animação de Morte")]
    public string dieTriggerName = "Die";        // nome do Trigger no Animator
    public MonoBehaviour[] scriptsToDisableOnDeath; // ex: PlayerMovement, PlayerAttack etc.

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private Animator anim;
    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
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

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // dispara animação de morte
        if (anim != null && !string.IsNullOrEmpty(dieTriggerName))
        {
            anim.SetTrigger(dieTriggerName);
        }

        // desabilita scripts de controle (movimento, tiro etc.)
        foreach (var s in scriptsToDisableOnDeath)
        {
            if (s != null) s.enabled = false;
        }

        OnDeath?.Invoke();
        Debug.Log("Player morreu!");

        // aqui depois vocês podem chamar Game Over / recarregar cena / menu etc.
        // por enquanto não destruo o player pra poder ver a animação
        // Destroy(gameObject);
    }
}
