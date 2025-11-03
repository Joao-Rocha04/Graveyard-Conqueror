using UnityEngine;
using System.Collections;

public class Sunstrike : MonoBehaviour
{
    public float velocidade = 10f;

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

        if (other.CompareTag("Enemy"))
        {
            jaAtivou = true;

            // Para o movimento
            velocidade = 0;

            // Aplica dano
            EnemyFollow2D inimigo = other.GetComponent<EnemyFollow2D>();
            if (inimigo != null)
            {
                inimigo.LevarDano();
            }

            // Aguarda a animação terminar antes de destruir
            StartCoroutine(DestruirDepoisDaAnimacao());
        }
    }

    private IEnumerator DestruirDepoisDaAnimacao()
    {
        if (animator != null)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        Destroy(gameObject);
    }
}
