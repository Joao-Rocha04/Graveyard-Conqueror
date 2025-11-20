using UnityEngine;
using System.Collections;

public class Sunstrike : MonoBehaviour
{
    public float velocidade = 10f;

    private Animator animator;
    private bool jaAtivou = false;
    public int dano;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!jaAtivou)
        {
            // seu projétil está indo pra baixo
            transform.position += Vector3.down * velocidade * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (jaAtivou) return;

        Debug.Log($"[Sunstrike] Colidiu com {other.name} (tag={other.tag})");

        bool acertouAlgo = false;

        // 1) Inimigo normal
        EnemyFollow2D inimigo = other.GetComponentInParent<EnemyFollow2D>();
        if (inimigo != null)
        {
            Debug.Log($"[Sunstrike] Acertou inimigo {inimigo.name} por {dano}");
            inimigo.LevarDano(dano);
            acertouAlgo = true;
        }

        // 2) Boss
        BossHealth boss = other.GetComponentInParent<BossHealth>();
        if (boss != null)
        {
            Debug.Log($"[Sunstrike] Acertou BOSS {boss.name} por {dano}. Vida antes: {boss.currentHealth}");
            boss.TakeDamage(dano);
            Debug.Log($"[Sunstrike] Vida do boss depois: {boss.currentHealth}");
            acertouAlgo = true;
        }

        if (acertouAlgo)
        {
            jaAtivou = true;
            velocidade = 0;
            StartCoroutine(DestruirDepoisDaAnimacao());
        }
    }

    private IEnumerator DestruirDepoisDaAnimacao()
    {
        if (animator != null)
        {
            yield return new WaitForSeconds(
                animator.GetCurrentAnimatorStateInfo(0).length
            );
        }

        Destroy(gameObject);
    }
}
