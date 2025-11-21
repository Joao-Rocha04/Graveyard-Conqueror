using UnityEngine;
using System.Collections;

public class Sunstrike : MonoBehaviour
{
    public float velocidade = 10f;
    public int dano;

    private Animator animator;
    private bool jaAtivou = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!jaAtivou)
        {
            transform.position += Vector3.down * velocidade * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (jaAtivou) return;

        bool acertouAlgo = false;

        // inimigo comum
        EnemyFollow2D inimigo = other.GetComponentInParent<EnemyFollow2D>();
        if (inimigo != null)
        {
            inimigo.LevarDano(dano);
            acertouAlgo = true;
        }

        // boss
        BossHealth boss = other.GetComponentInParent<BossHealth>();
        if (boss != null)
        {
            boss.TakeDamage(dano);
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
