using UnityEngine;
using System.Collections;

public class EnemyFollow2D : MonoBehaviour
{
    public float velocidade_inimigo = 2f;
    private float base_velocidade;          // cache da velocidade original
    private Transform alvo;
    public ObjectPool pool;

    [SerializeField] private Animator animator;
    private bool morrendo = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // guarda a velocidade base apenas uma vez
        if (base_velocidade <= 0f)
            base_velocidade = velocidade_inimigo;

        // aplica multiplicador global vigente (se existir)
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
            transform.position += (Vector3)(direcao * velocidade_inimigo * Time.deltaTime);
        }
    }

    // Método público chamado por armas/magias para matar o inimigo
    public void LevarDano()
    {
        if (!morrendo)
            StartCoroutine(DesaparecerAposTempo());
    }

    private IEnumerator DesaparecerAposTempo()
    {
        morrendo = true;
        animator.SetBool("isDead", true);

        // espera a animação atual terminar
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("isDead", false);
        morrendo = false;

        if (pool != null)
            pool.ReturnObjectToPool(gameObject);
        else
            gameObject.SetActive(false);
    }

    // ===== Multiplicador de velocidade (usado pelos upgrades) =====
    public void ApplySpeedMultiplier(float mul)
    {
        // protege contra valores inválidos
        if (base_velocidade <= 0f) base_velocidade = Mathf.Max(0.01f, velocidade_inimigo);
        velocidade_inimigo = base_velocidade * Mathf.Max(0.01f, mul);
    }
}
