using UnityEngine;
using System.Collections;

public class EnemyFollow2D : MonoBehaviour
{
    public float velocidade = 2f;
    private Transform alvo;
    public ObjectPool pool;

    [SerializeField] private Animator animator;
    private bool morrendo = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            alvo = jogador.transform;
        }
    }

    void Update()
    {
        if (alvo != null && !morrendo)
        {
            Vector2 direcao = ((Vector2)alvo.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(direcao * velocidade * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D colisor)
    {
        if (colisor.CompareTag("Player") && !morrendo)
        {
            StartCoroutine(DesaparecerAposTempo());
        }
    }

    private IEnumerator DesaparecerAposTempo()
    {
        morrendo = true;

        animator.SetBool("isDead", true);

        // Espera a animação atual rodar inteira
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("isDead", false);
        morrendo = false;

        if (pool != null)
        {
            pool.ReturnObjectToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
