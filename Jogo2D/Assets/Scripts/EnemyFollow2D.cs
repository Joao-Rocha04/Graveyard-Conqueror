using UnityEngine;
using System.Collections;

public class EnemyFollow2D : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade_inimigo = 2f;
    private float base_velocidade;
    private Transform alvo;
    public ObjectPool pool;

    [Header("Componentes")]
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool morrendo = false;

    [Header("XP Drop")]
    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private int xpAmount = 1;
    [SerializeField] private float xpSpreadRadius = 0.3f;
    private bool xpDropado = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // reset estado ao sair do pool
        morrendo = false;
        xpDropado = false;

        if (base_velocidade <= 0f)
            base_velocidade = velocidade_inimigo;

        float mul = GameUpgrades.Instance ? GameUpgrades.Instance.enemySpeedMul : 1f;
        ApplySpeedMultiplier(mul);
    }

    private void Start()
    {
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
            alvo = jogador.transform;
    }

    private void Update()
    {
        if (alvo != null && !morrendo)
        {
            Vector2 direcao = ((Vector2)alvo.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(direcao * velocidade_inimigo * Time.deltaTime);

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

        // 🔹 DROP DE XP NA HORA QUE COMEÇA A MORRER
        DropXP();

        if (animator != null)
        {
            animator.SetBool("isDead", true);

            yield return new WaitForSeconds(
                animator.GetCurrentAnimatorStateInfo(0).length
            );

            animator.SetBool("isDead", false);
        }
        else
        {
            yield return null;
        }

        if (pool != null)
            pool.ReturnObjectToPool(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void DropXP()
    {
        if (xpDropado) return;
        xpDropado = true;

        if (xpPrefab == null)
        {
            Debug.LogWarning("[EnemyFollow2D] xpPrefab NÃO atribuído em " + gameObject.name);
            return;
        }

        if (xpAmount <= 0)
        {
            Debug.LogWarning("[EnemyFollow2D] xpAmount <= 0 em " + gameObject.name);
            return;
        }

        Debug.Log("[EnemyFollow2D] Dropando " + xpAmount + " XP em " + gameObject.name);

        for (int i = 0; i < xpAmount; i++)
        {
            Vector2 offset2D = Random.insideUnitCircle * xpSpreadRadius;
            Vector3 pos = transform.position + new Vector3(offset2D.x, offset2D.y, 0f);
            Instantiate(xpPrefab, pos, Quaternion.identity);
        }
    }

    public void ApplySpeedMultiplier(float mul)
    {
        if (base_velocidade <= 0f)
            base_velocidade = Mathf.Max(0.01f, velocidade_inimigo);

        velocidade_inimigo = base_velocidade * Mathf.Max(0.01f, mul);
    }
}
