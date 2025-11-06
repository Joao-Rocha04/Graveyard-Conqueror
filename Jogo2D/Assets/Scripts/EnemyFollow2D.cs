using UnityEngine;
using System.Collections;

public class EnemyFollow2D : MonoBehaviour
{
    public float velocidade_inimigo = 2f;
    private float base_velocidade;
    private Rigidbody2D rb;
    private Transform alvo;
    public ObjectPool pool;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool morrendo = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (base_velocidade <= 0f)
            base_velocidade = velocidade_inimigo;

        float mul = GameUpgrades.Instance ? GameUpgrades.Instance.enemySpeedMul : 1f;
        ApplySpeedMultiplier(mul);
    }

    void Start()
    {
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
            alvo = jogador.transform;
    }

    void Update()
    {
        if (alvo != null && !morrendo)
        {
            Vector2 direcao = ((Vector2)alvo.position - (Vector2)transform.position).normalized;
            // transform.position += (Vector3)(direcao * velocidade_inimigo * Time.deltaTime);
            rb.linearVelocity = direcao * velocidade_inimigo * Time.deltaTime;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direcao.x > 0f;
            }
        }
    }

    public void LevarDano()
    {
        if (!morrendo)
            StartCoroutine(DesaparecerAposTempo());
    }

    private IEnumerator DesaparecerAposTempo()
    {
        morrendo = true;
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("isDead", false);
        morrendo = false;

        if (pool != null)
            pool.ReturnObjectToPool(gameObject);
        else
            gameObject.SetActive(false);
    }

    public void ApplySpeedMultiplier(float mul)
    {
        if (base_velocidade <= 0f) base_velocidade = Mathf.Max(0.01f, velocidade_inimigo);
        velocidade_inimigo = base_velocidade * Mathf.Max(0.01f, mul);
    }
}
